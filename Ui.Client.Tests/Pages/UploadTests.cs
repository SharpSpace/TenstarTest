using Application.DTOs;
using Application.Interfaces;
using Bunit;
using Domain.Entities;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Ui.Client.Tests.Pages;

public sealed class UploadTests(ITestOutputHelper output) : TestContext
{
    [Fact]
    public void SaveButton_Disabled_WhenNoValidUsers()
    {
        // Arrange
        var invalidResult = new UserValidationResult(
            new User { FullName = "Test", Username = "test", Email = "invalid", Password = "short" }
        );

        var mockCsvService = new Mock<IUserCsvService>();
        mockCsvService
            .Setup(s => s.ParseAndValidateAsync(It.IsAny<string>()))
            .Returns(ToAsyncEnumerable(invalidResult));

        Services.AddSingleton(mockCsvService.Object);
        Services.AddSingleton(Mock.Of<IUserService>());
        Services.AddSingleton(Mock.Of<ILogger<Ui.Client.Pages.Upload>>());

        var cut = RenderComponent<Ui.Client.Pages.Upload>();

        // Simulate file upload using bUnit's TestInputFile
        var inputFile = cut.FindComponent<InputFile>();
        inputFile.UploadFiles(InputFileContent.CreateFromText("irrelevant", "users.csv", null, "text/csv"));

        // Assert
        var button = cut.Find("button.btn-success");
        Assert.True(button.HasAttribute("disabled"));
    }

    [Fact]
    public void SaveButton_SavesValidUsers_AndShowsSuccessMessage()
    {
        // Arrange
        var validUser = new UserValidationResult(
            new User { FullName = "Valid User", Username = "valid", Email = "valid@example.com", Password = "Password1!" }
        );

        var mockCsvService = new Mock<IUserCsvService>();
        mockCsvService
            .Setup(s => s.ParseAndValidateAsync(It.IsAny<string>()))
            .Returns(ToAsyncEnumerable(validUser));

        var mockUserService = new Mock<IUserService>();
        mockUserService
            .Setup(s => s.SaveUsersAsync(It.IsAny<List<User>>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        Services.AddSingleton(mockCsvService.Object);
        Services.AddSingleton(mockUserService.Object);
        Services.AddSingleton(Mock.Of<ILogger<Ui.Client.Pages.Upload>>());

        var cut = RenderComponent<Ui.Client.Pages.Upload>();

        // Act
        var inputFile = cut.FindComponent<InputFile>();
        inputFile.UploadFiles(InputFileContent.CreateFromText("irrelevant", "users.csv", null, "text/csv"));

        // Click save button
        cut.Find("button.btn-success").Click();

        // Assert
        mockUserService.Verify(s => s.SaveUsersAsync(It.Is<List<User>>(l => l.Count == 1)), Times.Once);
        Assert.Contains("1 users saved!", cut.Markup);
    }

    [Fact]
    public void ShowsValidationErrors_WhenInvalidUsersUploaded()
    {
        // Arrange
        var mockCsvService = new Mock<IUserCsvService>();
        var invalidResult = new UserValidationResult(
            new User { FullName = "Test", Username = "test", Email = "invalid", Password = "short" }
        );

        mockCsvService
            .Setup(s => s.ParseAndValidateAsync(It.IsAny<string>()))
            .Returns(ToAsyncEnumerable(invalidResult));

        Services.AddSingleton(mockCsvService.Object);
        Services.AddSingleton(Mock.Of<IUserService>());
        Services.AddSingleton(Mock.Of<ILogger<Ui.Client.Pages.Upload>>());

        var cut = RenderComponent<Ui.Client.Pages.Upload>();

        // Act
        var inputFile = cut.FindComponent<InputFile>();
        inputFile.UploadFiles(InputFileContent.CreateFromText("irrelevant", "users.csv", null, "text/csv"));

        // Assert
        Assert.Contains("The Email field is not a valid e-mail address.", cut.Markup);
        Assert.Contains("Password must contain at least one upper case letter, one lower case letter, one digit, and one special character.", cut.Markup);
    }

    private static IAsyncEnumerable<UserValidationResult> ToAsyncEnumerable(params UserValidationResult[] results)
    {
        return GetAsyncEnumerable();

        async IAsyncEnumerable<UserValidationResult> GetAsyncEnumerable()
        {
            foreach (var result in results)
            {
                yield return result;
                await Task.Yield();
            }
        }
    }
}
