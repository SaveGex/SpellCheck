using Application.ModelsDTO;

namespace Application.Interfaces;

public interface IAuthService
{
    Task<UserResponseDTO> RegisterUserAsync(UserRegisterDTO dto);
    Task<AuthResponseDTO> AuthenticateUserAsync(UserLoginDTO dto, string ipAddress);
    Task<AuthResponseDTO> RefreshTokenAsync(string refreshToken, string clientId, string ipAddress);
    Task<bool> RevokeRefreshTokenAsync(string refreshToken, string ipAddress);

}
