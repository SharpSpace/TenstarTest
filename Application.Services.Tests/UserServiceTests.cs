using Domain.Entities;
using Domain.Interfaces;
using Moq;
using Xunit;

namespace Application.Services.Tests;

public sealed class UserServiceTests
{
    [Fact]
    public async Task GetAllUsersAsync_ReturnsAllUsers()
    {
        // Arrange
        var users = new List<User>
        {
            new() { Id = 1, FullName = "Anna Andersson", Username = "anna", Email = "anna@example.com", Password = "Password1!" },
            new() { Id = 2, FullName = "Bertil Bengtsson", Username = "bertil", Email = "bertil@example.com", Password = "Password2!" }
        };

        var repoMock = new Mock<IUserRepository>();
        repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(users);

        var service = new UserService(repoMock.Object);

        // Act
        var result = (await service.GetAllUsersAsync()).ToList();

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Equal("Anna Andersson", result.First().FullName);
    }

    [Fact]
    public async Task SaveUsersAsync_CallsRepositoryWithUsers()
    {
        // Arrange
        var users = new List<User>
        {
            new() { Id = 1, FullName = "Anna Andersson", Username = "anna", Email = "anna@example.com", Password = "Password1!" }
        };

        var repoMock = new Mock<IUserRepository>();
        repoMock.Setup(r => r.AddAsync(It.IsAny<IEnumerable<User>>())).Returns(Task.CompletedTask).Verifiable();

        var service = new UserService(repoMock.Object);

        // Act
        await service.SaveUsersAsync(users);

        // Assert
        repoMock.Verify(r => r.AddAsync(It.Is<IEnumerable<User>>(u => u.SequenceEqual(users))), Times.Once);
    }
}