using Application.DTOs;

namespace Application.Interfaces;

public interface IUserCsvService
{
    public IAsyncEnumerable<UserValidationResult> ParseAndValidateAsync(
        string csv
    );
}