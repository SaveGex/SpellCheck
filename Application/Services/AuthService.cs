
using Application.Interfaces;
using Application.ModelsDTO;
using AutoMapper;
using DomainData.Interfaces;
using DomainData.Models;
using Microsoft.AspNetCore.Identity;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        public IUserRepository UserRepository { get; init; }

        public ITokenService TokenService { get; init; }

        public IClientCacheService ClientCacheService { get; init; }

        public IMapper Mapper { get; init; }

        public IRefreshTokenRepository RefreshTokenRepository { get; init; }

        public IPasswordHasher<User> PasswordHasher { get; init; }

        public AuthService(IUserRepository userRepository, ITokenService tokenService, IClientCacheService clientCacheService, IRefreshTokenRepository refreshTokenRepository, IMapper mapper, IPasswordHasher<User> passwordHasher)
        {
            UserRepository = userRepository;
            TokenService = tokenService;
            ClientCacheService = clientCacheService;
            RefreshTokenRepository = refreshTokenRepository;
            Mapper = mapper;
            PasswordHasher = passwordHasher;
        }


        public async Task<UserResponseDTO> RegisterUserAsync(UserRegisterDTO dto)
        {
            if (string.IsNullOrEmpty(dto.Number) && string.IsNullOrEmpty(dto.Email))
            {
                throw new Exception("You must provide either Number or Email.");
            }

            bool exists = await UserRepository.ExistsAsync(
                dto.Number, dto.Email);

            if (exists)
            {
                throw new Exception("User already exists.");
            }

            if (dto.Password.Length < 4)
            {
                throw new Exception("Password must be at least 4 characters long.");
            }

            User createdUser = await UserRepository.CreateUserAsync(
                Mapper.Map<User>(dto));

            return Mapper.Map<UserResponseDTO>(createdUser);

        }

        public async Task<AuthResponseDTO> AuthenticateUserAsync(UserLoginDTO dto, string ipAddress)
        {
            User user = (dto) switch
            {
                { Email: not null } => await UserRepository.GetByEmailIncludeRolesAsync(dto.Email) ?? throw new Exception("User not found."),
                { Number: not null } => await UserRepository.GetByPhoneIncludeRolesAsync(dto.Number) ?? throw new Exception("User not found."),
                _ => throw new Exception("You must provide either Email or Number."),
            };

            if(PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password) == PasswordVerificationResult.Failed)
            {
                throw new Exception("Invalid password.");
            }

            List<string> roles = user.Roles.Select(r => r.Name).ToList();

            Client client = await ClientCacheService.GetClientByClientIdAsync(dto.ClientId) ?? throw new Exception("Client not found.");

            string accessToken = await TokenService.GenerateAccessTokenAsync(user, roles, out Guid jwtId, client);

            RefreshToken refreshToken = await TokenService.GenerateRefreshTokenAsync(user.Id, jwtId, client, ipAddress);

            await RefreshTokenRepository.AddRefreshTokenAsync(refreshToken);

            int accessTokenExpireMinutes = TokenService.GetAccessTokenExperationMinutes();

            return new AuthResponseDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token,
                AccessTokenExpiresAt = DateTime.UtcNow.AddMinutes(accessTokenExpireMinutes)
            };

        }

        public async Task<AuthResponseDTO> RefreshTokenAsync(string refreshToken, string clientId, string ipAddress)
        {
            Client client = await ClientCacheService.GetClientByClientIdAsync(clientId) ?? throw new Exception("Client not found.");

            RefreshToken existingToken = await RefreshTokenRepository.GetRefreshTokenIncludeUserAndRolesByRefreshTokenAndClientIdAsync(refreshToken, client.Id);

            if(existingToken.IsActive == false)
            {
                throw new Exception("Refresh token is not active.");
            }

            existingToken.Revoke();

            User user = existingToken.AssociatedUser;
            List<string> roles = user.Roles.Select(r => r.Name).ToList();

            string newAccessToken = await TokenService.GenerateAccessTokenAsync(user, roles, out Guid newJwtId, client);

            RefreshToken newRefreshToken = await TokenService.GenerateRefreshTokenAsync(user.Id, newJwtId, client, ipAddress);

            newRefreshToken = await RefreshTokenRepository.AddRefreshTokenAsync(newRefreshToken);

            int accessTokenExpireMinutes = TokenService.GetAccessTokenExperationMinutes();

            return new AuthResponseDTO
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken.Token,
                AccessTokenExpiresAt = DateTime.UtcNow.AddMinutes(accessTokenExpireMinutes)
            };
        }

        public async Task<bool> RevokeRefreshTokenAsync(string refreshToken, string ipAddress)
        {
            RefreshToken existingToken;
            try
            {
                existingToken = await RefreshTokenRepository.GetRefreshTokenByTokenAsync(refreshToken);
            }
            catch
            {
                return false;
            }

            existingToken.Revoke();

            return true;
        }
    }
}
