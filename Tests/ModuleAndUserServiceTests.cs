using Application.ModelsDTO;
using Application.Services;
using AutoMapper;
using DomainData.Interfaces;
using DomainData.Models;
using FluentAssertions;
using Moq;

namespace Tests.Services
{
    public class ModuleServiceTests
    {
        private readonly Mock<IModuleRepository> _moduleRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ModuleService _sut;

        public ModuleServiceTests()
        {
            _moduleRepoMock = new Mock<IModuleRepository>();
            _mapperMock = new Mock<IMapper>();
            _sut = new ModuleService(_moduleRepoMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task CreateModuleAsync_DuplicateIdentifier_ThrowsException()
        {
            // Arrange
            var dto = new ModuleCreateDTO { Name = "Math" };
            var module = new Module { Identifier = Guid.NewGuid(), Name = "Math" };

            _mapperMock.Setup(x => x.Map<Module>(dto)).Returns(module);
            _moduleRepoMock.Setup(x => x.AnyAsync(module.Identifier)).ReturnsAsync(true);

            // Act
            var act = async () => await _sut.CreateModuleAsync(dto);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("*already exists*");
        }

        [Fact]
        public async Task CreateModuleAsync_ValidDto_CallsRepositoryOnce()
        {
            // Arrange
            var dto = new ModuleCreateDTO { Name = "Physics" };
            var module = new Module { Identifier = Guid.NewGuid(), Name = "Physics" };
            var response = new ModuleResponseDTO { Name = "Physics" };

            _mapperMock.Setup(x => x.Map<Module>(dto)).Returns(module);
            _moduleRepoMock.Setup(x => x.AnyAsync(module.Identifier)).ReturnsAsync(false);
            _moduleRepoMock.Setup(x => x.CreateModuleAsync(module)).ReturnsAsync(module);
            _mapperMock.Setup(x => x.Map<ModuleResponseDTO>(module)).Returns(response);

            // Act
            await _sut.CreateModuleAsync(dto);

            // Assert
            _moduleRepoMock.Verify(x => x.CreateModuleAsync(module), Times.Once);
        }

        [Fact]
        public async Task GetModuleByIdAsync_NotFound_ThrowsException()
        {
            // Arrange
            _moduleRepoMock.Setup(x => x.GetModuleByIdAsync(999)).ReturnsAsync((Module?)null);

            // Act
            var act = async () => await _sut.GetModuleByIdAsync(999);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("*999*");
        }

        [Fact]
        public async Task GetModuleByIdAsync_ExistingModule_ReturnsMappedDTO()
        {
            // Arrange
            var module = new Module { Id = 1, Name = "Chemistry" };
            var response = new ModuleResponseDTO { Id = 1, Name = "Chemistry" };

            _moduleRepoMock.Setup(x => x.GetModuleByIdAsync(1)).ReturnsAsync(module);
            _mapperMock.Setup(x => x.Map<ModuleResponseDTO>(module)).Returns(response);

            // Act
            var result = await _sut.GetModuleByIdAsync(1);

            // Assert
            result.Name.Should().Be("Chemistry");
        }

        [Fact]
        public async Task DeleteModuleAsync_NotFound_ThrowsException()
        {
            // Arrange
            _moduleRepoMock.Setup(x => x.GetModuleByIdAsync(42)).ReturnsAsync((Module?)null);

            // Act
            var act = async () => await _sut.DeleteModuleAsync(42);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("*42*");
        }

        [Fact]
        public async Task DeleteModuleAsync_ExistingModule_CallsDeleteOnce()
        {
            // Arrange
            var module = new Module { Id = 5, Name = "ToDelete" };
            var response = new ModuleResponseDTO { Id = 5 };

            _moduleRepoMock.Setup(x => x.GetModuleByIdAsync(5)).ReturnsAsync(module);
            _moduleRepoMock.Setup(x => x.DeleteModuleAsync(module)).ReturnsAsync(module);
            _mapperMock.Setup(x => x.Map<ModuleResponseDTO>(module)).Returns(response);

            // Act
            await _sut.DeleteModuleAsync(5);

            // Assert
            _moduleRepoMock.Verify(x => x.DeleteModuleAsync(module), Times.Once);
        }
    }

    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UserService _sut;

        public UserServiceTests()
        {
            _userRepoMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();
            _sut = new UserService(_userRepoMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetUserByIdAsync_UserNotFound_ThrowsException()
        {
            // Arrange
            int userId = 404;
            _userRepoMock.Setup(x => x.GetUserByIdAsync(userId)).ThrowsAsync(new Exception($"user by id {userId} does not found."));

            // Act
            var act = async () => await _sut.GetUserByIdAsync(userId);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("*404*");
        }

        [Fact]
        public async Task GetUserByIdAsync_ExistingUser_ReturnsMappedDTO()
        {
            // Arrange
            var user = new User { Id = 1, Username = "alice" };
            var response = new UserResponseDTO { Id = 1, Username = "alice" };

            _userRepoMock.Setup(x => x.GetUserByIdAsync(1)).ReturnsAsync(user);
            _mapperMock.Setup(x => x.Map<UserResponseDTO>(user)).Returns(response);

            // Act
            var result = await _sut.GetUserByIdAsync(1);

            // Assert
            result.Username.Should().Be("alice");
        }

        [Fact]
        public async Task DeleteUserAsync_UserNotFound_ThrowsException()
        {
            // Arrange
            _userRepoMock.Setup(x => x.GetUserByIdAsync(99)).ReturnsAsync((User?)null);

            // Act
            var act = async () => await _sut.DeleteUserAsync(99);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("*99*");
        }

        [Fact]
        public async Task DeleteUserAsync_ExistingUser_CallsDeleteOnce()
        {
            // Arrange
            var user = new User { Id = 7, Username = "bob" };
            var response = new UserResponseDTO { Id = 7 };

            _userRepoMock.Setup(x => x.GetUserByIdAsync(7)).ReturnsAsync(user);
            _userRepoMock.Setup(x => x.DeleteUserAsync(user)).ReturnsAsync(user);
            _mapperMock.Setup(x => x.Map<UserResponseDTO>(user)).Returns(response);

            // Act
            await _sut.DeleteUserAsync(7);

            // Assert
            _userRepoMock.Verify(x => x.DeleteUserAsync(user), Times.Once);
        }

        [Fact]
        public async Task UpdateUserAsync_PasswordTooShort_ThrowsException()
        {
            // Arrange
            var dto = new UserUpdateDTO
            {
                Username = "user",
                Password = "ab",
                Email = "u@u.com"
            };

            _userRepoMock.Setup(x => x.ExistsAsync(dto.Number, dto.Email)).ReturnsAsync(false);

            // Act
            var act = async () => await _sut.UpdateUserAsync(dto);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("*Password*4*");
        }
    }
}
