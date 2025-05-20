using Xunit;

namespace Application.Services.Tests;

public sealed class UserCsvServiceTests
{
    [Fact]
    public void ParseAndValidateAsync_EmptyFile_ReturnsEmptyList()
    {
        // Arrange
        const string csv = "";
        var service = new UserCsvService();

        // Act
        var results = service.ParseAndValidateAsync(csv).ToBlockingEnumerable();

        // Assert
        Assert.Empty(results);
    }

    [Fact]
    public void ParseAndValidateAsync_InvalidEmail_ReturnsValidationError()
    {
        // Arrange
        const string csv = """
                           FullName,Username,Email,Password
                           Anna Andersson,anna,not-an-email,Password1!
                           """;

        var service = new UserCsvService();

        // Act
        var results = service.ParseAndValidateAsync(csv).ToBlockingEnumerable().ToList();

        // Assert
        Assert.Single(results);
        var result = results[0];
        Assert.True(result.HasErrors);
        Assert.True(result.HasError(u => u.Email));
    }

    [Fact]
    public void ParseAndValidateAsync_MissingFields_SkipsLine()
    {
        // Arrange
        const string csv = """
                           FullName,Username,Email,Password
                           Anna Andersson,anna,anna@example.com,Password1!
                           MissingFields,onlyonefield
                           """;

        var service = new UserCsvService();

        // Act
        var results = service.ParseAndValidateAsync(csv).ToBlockingEnumerable().ToList();

        // Assert
        Assert.Single(results);
        Assert.Equal("Anna Andersson", results[0].User.FullName);
    }

    [Fact]
    public void ParseAndValidateAsync_TooShortPassword_ReturnsValidationError()
    {
        // Arrange
        const string csv = """
                           FullName,Username,Email,Password
                           Anna Andersson,anna,anna@example.com,short
                           """;

        var service = new UserCsvService();

        // Act
        var results = service.ParseAndValidateAsync(csv).ToBlockingEnumerable().ToList();

        // Assert
        Assert.Single(results);
        var result = results[0];
        Assert.True(result.HasErrors);
        Assert.True(result.HasError(u => u.Password));
    }

    [Fact]
    public void ParseAndValidateAsync_ValidCsv_ReturnsValidUser()
    {
        // Arrange
        const string csv = """
                           FullName,Username,Email,Password
                           Anna Andersson,anna,anna@example.com,Password1!
                           """;

        var service = new UserCsvService();

        // Act
        var results = service.ParseAndValidateAsync(csv).ToBlockingEnumerable().ToList();

        // Assert
        Assert.Single(results);
        var result = results[0];
        Assert.False(result.HasErrors);
        Assert.Equal("Anna Andersson", result.User.FullName);
        Assert.Equal("anna", result.User.Username);
        Assert.Equal("anna@example.com", result.User.Email);
        Assert.Equal("Password1!", result.User.Password);
    }
}
