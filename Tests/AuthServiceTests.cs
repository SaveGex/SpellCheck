using Application.Interfaces;
using Application.ModelsDTO;
using Application.Services;
using AutoMapper;
using DomainData.Interfaces;
using DomainData.Models;
using DomainData.Roles;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Tests.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly Mock<IClientCacheService> _clientCacheMock;
        private readonly Mock<IRefreshTokenRepository> _refreshTokenRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IPasswordHasher<User>> _passwordHasherMock;
        private readonly AuthService _sut;

        public AuthServiceTests()
        {
            _userRepoMock = new Mock<IUserRepository>();
            _tokenServiceMock = new Mock<ITokenService>();
            _clientCacheMock = new Mock<IClientCacheService>();
            _refreshTokenRepoMock = new Mock<IRefreshTokenRepository>();
            _mapperMock = new Mock<IMapper>();
            _passwordHasherMock = new Mock<IPasswordHasher<User>>();

            _sut = new AuthService(
                _userRepoMock.Object,
                _tokenServiceMock.Object,
                _clientCacheMock.Object,
                _refreshTokenRepoMock.Object,
                _mapperMock.Object,
                _passwordHasherMock.Object
            );
        }

        #region RegisterUserAsync

        [Fact]
        public async Task RegisterUserAsync_NoEmailOrNumber_ThrowsException()
        {
            // Arrange
            var dto = new UserRegisterDTO
            {
                Username = "testuser",
                Password = "password123",
                Email = null,
                Number = null
            };

            // Act
            var act = async () => await _sut.RegisterUserAsync(dto);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("*Number*Email*");
        }

        [Fact]
        public async Task RegisterUserAsync_UserAlreadyExists_ThrowsException()
        {
            // Arrange
            var dto = new UserRegisterDTO
            {
                Username = "existing",
                Email = "existing@test.com",
                Password = "password123"
            };

            _userRepoMock.Setup(x => x.ExistsAsync(dto.Number, dto.Email)).ReturnsAsync(true);

            // Act
            var act = async () => await _sut.RegisterUserAsync(dto);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("*already exists*");
        }

        [Fact]
        public async Task RegisterUserAsync_PasswordTooShort_ThrowsException()
        {
            // Arrange
            var dto = new UserRegisterDTO
            {
                Username = "user",
                Email = "user@test.com",
                Password = "ab"
            };

            _userRepoMock.Setup(x => x.ExistsAsync(dto.Number, dto.Email)).ReturnsAsync(false);

            // Act
            var act = async () => await _sut.RegisterUserAsync(dto);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("*Password*4*");
        }

        [Fact]
        public async Task RegisterUserAsync_ValidDto_ReturnsUserResponseDTO()
        {
            // Arrange
            var dto = new UserRegisterDTO
            {
                Username = "newuser",
                Email = "new@test.com",
                Password = "password123"
            };
            var user = new User { Id = 1, Username = dto.Username, Email = dto.Email };
            var expectedResponse = new UserResponseDTO { Id = 1, Username = dto.Username };

            _userRepoMock.Setup(x => x.ExistsAsync(dto.Number, dto.Email)).ReturnsAsync(false);
            _mapperMock.Setup(x => x.Map<User>(dto)).Returns(user);
            _userRepoMock.Setup(x => x.CreateUserAsync(user)).ReturnsAsync(user);
            _mapperMock.Setup(x => x.Map<UserResponseDTO>(user)).Returns(expectedResponse);

            // Act
            var result = await _sut.RegisterUserAsync(dto);

            // Assert
            result.Should().NotBeNull();
            result.Username.Should().Be("newuser");
        }

        [Fact]
        public async Task RegisterUserAsync_WithAdminRole_PassesRoleToRepository()
        {
            // Arrange
            var dto = new UserRegisterDTO
            {
                Username = "admin",
                Email = "admin@test.com",
                Password = "strongpass"
            };
            var user = new User { Id = 99, Username = "admin" };

            _userRepoMock.Setup(x => x.ExistsAsync(It.IsAny<string?>(), It.IsAny<string?>())).ReturnsAsync(false);
            _mapperMock.Setup(x => x.Map<User>(dto)).Returns(user);
            _userRepoMock
                .Setup(x => x.CreateUserAsync(user, RoleNames.Admin))
                .ReturnsAsync(user);
            _mapperMock.Setup(x => x.Map<UserResponseDTO>(user)).Returns(new UserResponseDTO { Id = 99 });

            // Act
            await _sut.RegisterUserAsync(dto, RoleNames.Admin);

            // Assert
            _userRepoMock.Verify(x => x.CreateUserAsync(user, RoleNames.Admin), Times.Once);
        }

        #endregion

        #region AuthenticateUserAsync

        [Fact]
        public async Task AuthenticateUserAsync_ByEmail_UserNotFound_ThrowsException()
        {
            // Arrange
            var dto = new UserLoginDTO
            {
                Email = "notfound@test.com",
                Password = "pass",
                ClientId = "client1"
            };

            _userRepoMock.Setup(x => x.GetByEmailIncludeRolesAsync(dto.Email))
                .ReturnsAsync((User?)null);

            // Act
            var act = async () => await _sut.AuthenticateUserAsync(dto, "127.0.0.1");

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("*not found*");
        }

        [Fact]
        public async Task AuthenticateUserAsync_WrongPassword_ThrowsException()
        {
            // Arrange
            var dto = new UserLoginDTO
            {
                Email = "user@test.com",
                Password = "wrongpass",
                ClientId = "client1"
            };
            var user = new User { Id = 1, Email = dto.Email, PasswordHash = "hash" };

            _userRepoMock.Setup(x => x.GetByEmailIncludeRolesAsync(dto.Email)).ReturnsAsync(user);
            _passwordHasherMock
                .Setup(x => x.VerifyHashedPassword(user, user.PasswordHash, dto.Password))
                .Returns(PasswordVerificationResult.Failed);

            // Act
            var act = async () => await _sut.AuthenticateUserAsync(dto, "127.0.0.1");

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("*password*");
        }

        [Fact]
        public async Task AuthenticateUserAsync_ClientNotFound_ThrowsException()
        {
            // Arrange
            var dto = new UserLoginDTO
            {
                Email = "user@test.com",
                Password = "correctpass",
                ClientId = "unknownClient"
            };
            var user = new User { Id = 1, Email = dto.Email, PasswordHash = "hash" };

            _userRepoMock.Setup(x => x.GetByEmailIncludeRolesAsync(dto.Email)).ReturnsAsync(user);
            _passwordHasherMock
                .Setup(x => x.VerifyHashedPassword(user, user.PasswordHash, dto.Password))
                .Returns(PasswordVerificationResult.Success);
            _clientCacheMock.Setup(x => x.GetClientByClientIdAsync(dto.ClientId))
                .ReturnsAsync((Client?)null);

            // Act
            var act = async () => await _sut.AuthenticateUserAsync(dto, "127.0.0.1");

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("*Client*not found*");
        }

        [Fact]
        public async Task AuthenticateUserAsync_ValidCredentials_ReturnsAuthResponse()
        {
            // Arrange
            var dto = new UserLoginDTO
            {
                Email = "user@test.com",
                Password = "correctpass",
                ClientId = "client1"
            };
            var user = new User { Id = 1, Email = dto.Email, PasswordHash = "hash", Roles = new List<Role> { new Role { Name = "User" } } };
            var client = new Client { Id = 1, ClientId = "client1", URL = "http://client.com" };
            var refreshToken = new RefreshToken { Token = "rt_token", ExpiresAt = DateTime.UtcNow.AddDays(7) };

            _userRepoMock.Setup(x => x.GetByEmailIncludeRolesAsync(dto.Email)).ReturnsAsync(user);
            _passwordHasherMock
                .Setup(x => x.VerifyHashedPassword(user, user.PasswordHash, dto.Password))
                .Returns(PasswordVerificationResult.Success);
            _clientCacheMock.Setup(x => x.GetClientByClientIdAsync(dto.ClientId)).ReturnsAsync(client);
            _tokenServiceMock
                .Setup(x => x.GenerateAccessTokenAsync(user, It.IsAny<IEnumerable<string>>(), out It.Ref<Guid>.IsAny, client))
                .Returns((User u, IEnumerable<string> roles, ref Guid jwtId, Client c) =>
                {
                    jwtId = Guid.NewGuid();
                    return Task.FromResult("access_token");
                });
            _tokenServiceMock
                .Setup(x => x.GenerateRefreshTokenAsync(user.Id, It.IsAny<Guid>(), client, "127.0.0.1"))
                .ReturnsAsync(refreshToken);
            _refreshTokenRepoMock.Setup(x => x.AddRefreshTokenAsync(refreshToken)).ReturnsAsync(refreshToken);
            _tokenServiceMock.Setup(x => x.GetAccessTokenExperationMinutes()).Returns(15);

            // Act
            var result = await _sut.AuthenticateUserAsync(dto, "127.0.0.1");

            // Assert
            result.Should().NotBeNull();
            result.AccessToken.Should().Be("access_token");
            result.RefreshToken.Should().Be("rt_token");
        }

        #endregion

        #region RefreshTokenAsync

        [Fact]
        public async Task RefreshTokenAsync_ClientNotFound_ThrowsException()
        {
            // Arrange
            _clientCacheMock.Setup(x => x.GetClientByClientIdAsync("unknown"))
                .ReturnsAsync((Client?)null);

            // Act
            var act = async () => await _sut.RefreshTokenAsync("rt", "unknown", "127.0.0.1");

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("*Client*not found*");
        }

        [Fact]
        public async Task RefreshTokenAsync_InactiveToken_ThrowsException()
        {
            // Arrange
            var client = new Client { Id = 1, ClientId = "client1" };
            var inactiveToken = new RefreshToken
            {
                Token = "old_token",
                IsRevoked = true,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                AssociatedUser = new User { Roles = new List<Role>() }
            };

            _clientCacheMock.Setup(x => x.GetClientByClientIdAsync("client1")).ReturnsAsync(client);
            _refreshTokenRepoMock
                .Setup(x => x.GetRefreshTokenIncludeUserAndRolesByRefreshTokenAndClientIdAsync("old_token", client.Id))
                .ReturnsAsync(inactiveToken);

            // Act
            var act = async () => await _sut.RefreshTokenAsync("old_token", "client1", "127.0.0.1");

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("*not active*");
        }

        #endregion

        #region RevokeRefreshTokenAsync

        [Fact]
        public async Task RevokeRefreshTokenAsync_TokenNotFound_ReturnsFalse()
        {
            // Arrange
            _refreshTokenRepoMock
                .Setup(x => x.GetRefreshTokenByTokenAsync("nonexistent"))
                .ThrowsAsync(new Exception("Not found"));

            // Act
            var result = await _sut.RevokeRefreshTokenAsync("nonexistent", "127.0.0.1");

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task RevokeRefreshTokenAsync_ValidToken_ReturnsTrue()
        {
            // Arrange
            var token = new RefreshToken
            {
                Token = "valid_token",
                IsRevoked = false,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };
            _refreshTokenRepoMock
                .Setup(x => x.GetRefreshTokenByTokenAsync("valid_token"))
                .ReturnsAsync(token);

            // Act
            var result = await _sut.RevokeRefreshTokenAsync("valid_token", "127.0.0.1");

            // Assert
            result.Should().BeTrue();
            token.IsRevoked.Should().BeTrue();
        }

        #endregion
    }
}
