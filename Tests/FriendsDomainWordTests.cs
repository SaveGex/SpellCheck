using Application.ModelsDTO;
using Application.Services;
using AutoMapper;
using DomainData.Interfaces;
using DomainData.Models;
using FluentAssertions;
using Moq;

namespace Tests.Services
{
    public class FriendsServiceTests
    {
        private readonly Mock<IFriendsRepository> _friendsRepoMock;
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly FriendsService _sut;

        public FriendsServiceTests()
        {
            _friendsRepoMock = new Mock<IFriendsRepository>();
            _userRepoMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();
            _sut = new FriendsService(_friendsRepoMock.Object, _userRepoMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task AddFriendAsync_SameUserIds_ThrowsException()
        {
            // Arrange
            var dto = new FriendCreateDTO { FromIndividualId = 1, ToIndividualId = 1 };

            _userRepoMock.Setup(x => x.GetUserByIdAsync(1)).ReturnsAsync(new User { Id = 1 });

            // Act
            var act = async () => await _sut.AddFriendAsync(dto);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("*himself*");
        }

        [Fact]
        public async Task AddFriendAsync_ValidDto_CallsRepositoryOnce()
        {
            // Arrange
            var dto = new FriendCreateDTO { FromIndividualId = 1, ToIndividualId = 2 };
            var friend = new Friend { Id = 1, FromIndividualId = 1, ToIndividualId = 2 };
            var response = new FriendResponseDTO { Id = 1 };

            _userRepoMock.Setup(x => x.GetUserByIdAsync(1)).ReturnsAsync(new User { Id = 1 });
            _userRepoMock.Setup(x => x.GetUserByIdAsync(2)).ReturnsAsync(new User { Id = 2 });
            _mapperMock.Setup(x => x.Map<Friend>(dto)).Returns(friend);
            _friendsRepoMock.Setup(x => x.AddFriendAsync(friend)).ReturnsAsync(friend);
            _mapperMock.Setup(x => x.Map<FriendResponseDTO>(friend)).Returns(response);

            // Act
            await _sut.AddFriendAsync(dto);

            // Assert
            _friendsRepoMock.Verify(x => x.AddFriendAsync(friend), Times.Once);
        }

        [Fact]
        public async Task UpdateFriendAsync_SameUserIds_ThrowsException()
        {
            // Arrange
            var dto = new FriendUpdateDTO { FromIndividualId = 5, ToIndividualId = 5 };

            // Act
            var act = async () => await _sut.UpdateFriendAsync(dto);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("*himself*");
        }

        [Fact]
        public async Task DeleteFriendAsync_ExistingFriend_CallsDeleteOnce()
        {
            // Arrange
            var friend = new Friend { Id = 3, FromIndividualId = 1, ToIndividualId = 2 };
            var response = new FriendResponseDTO { Id = 3 };

            _friendsRepoMock.Setup(x => x.GetFriendAsync(3)).ReturnsAsync(friend);
            _friendsRepoMock.Setup(x => x.DeleteFriendAsync(friend)).ReturnsAsync(friend);
            _mapperMock.Setup(x => x.Map<FriendResponseDTO>(friend)).Returns(response);

            // Act
            await _sut.DeleteFriendAsync(3);

            // Assert
            _friendsRepoMock.Verify(x => x.DeleteFriendAsync(friend), Times.Once);
        }
    }
}

namespace Tests.Domain
{
    public class RefreshTokenTests
    {
        [Fact]
        public void Revoke_SetsIsRevokedTrue()
        {
            // Arrange
            var token = new RefreshToken
            {
                Token = "some_token",
                IsRevoked = false,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };

            // Act
            token.Revoke();

            // Assert
            token.IsRevoked.Should().BeTrue();
        }

        [Fact]
        public void Revoke_SetsRevokedAtToNow()
        {
            // Arrange
            var token = new RefreshToken
            {
                Token = "some_token",
                IsRevoked = false,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };
            var before = DateTime.UtcNow;

            // Act
            token.Revoke();

            // Assert
            token.RevokedAt.Should().NotBeNull();
            token.RevokedAt!.Value.Should().BeOnOrAfter(before);
        }

        [Fact]
        public void IsActive_RevokedToken_ReturnsFalse()
        {
            // Arrange
            var token = new RefreshToken
            {
                IsRevoked = true,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };

            // Assert
            token.IsActive.Should().BeFalse();
        }

        [Fact]
        public void IsActive_ExpiredToken_ReturnsFalse()
        {
            // Arrange
            var token = new RefreshToken
            {
                IsRevoked = false,
                ExpiresAt = DateTime.UtcNow.AddDays(-1) // expired
            };

            // Assert
            token.IsActive.Should().BeFalse();
        }

        [Fact]
        public void IsActive_ValidToken_ReturnsTrue()
        {
            // Arrange
            var token = new RefreshToken
            {
                IsRevoked = false,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };

            // Assert
            token.IsActive.Should().BeTrue();
        }
    }

    public class WordServiceBugTests
    {
        private readonly Mock<IWordRepository> _wordRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly WordService _sut;

        public WordServiceBugTests()
        {
            _wordRepoMock = new Mock<IWordRepository>();
            _mapperMock = new Mock<IMapper>();
            _sut = new WordService(_wordRepoMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task UpdateWordAsync_ShouldCallRepositoryAndReturnMappedResult()
        {
            // Arrange
            var dto = new WordUpdateDTO { Expression = "new" };

            var mappedWord = new Word { Id = 1, Expression = "mapped-from-dto" };
            var repoResult = new Word { Id = 1, Expression = "from-repo" };
            var mappedResponse = new WordResponseDTO { Expression = "mapped-response" };

            _mapperMock
                .Setup(x => x.Map<Word>(dto))
                .Returns(mappedWord);

            _wordRepoMock
                .Setup(x => x.UpdateWordAsync(mappedWord))
                .ReturnsAsync(repoResult);

            _mapperMock
                .Setup(x => x.Map<WordResponseDTO>(repoResult))
                .Returns(mappedResponse);

            // Act
            var response = await _sut.UpdateWordAsync(dto);

            // Assert

            // 1. Перевірка, що репозиторій викликаний правильно
            _wordRepoMock.Verify(x => x.UpdateWordAsync(mappedWord), Times.Once);

            // 2. Перевірка, що mapper викликався для response
            _mapperMock.Verify(x => x.Map<WordResponseDTO>(repoResult), Times.Once);

            // 3. Перевірка, що повернувся саме той об'єкт, що віддав mapper
            response.Should().BeSameAs(mappedResponse);
        }
    }
}


