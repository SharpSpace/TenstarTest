using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Services;

public sealed class UserCsvService : IUserCsvService
{
    public async IAsyncEnumerable<UserValidationResult> ParseAndValidateAsync(string csv)
    {
        if (string.IsNullOrWhiteSpace(csv))
            yield break;

        var lines = csv.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
        if (lines.Length <= 1)
            yield break;

        foreach (var line in lines.Skip(1)) // skip header
        {
            var result = await ParseAndValidateLineAsync(line);
            if (result != null)
                yield return result;
        }
    }

    private static Task<UserValidationResult?> ParseAndValidateLineAsync(string line)
    {
        if (string.IsNullOrWhiteSpace(line)) return Task.FromResult<UserValidationResult?>(null);

        var fields = line.Split(',');
        if (fields.Length < 4) return Task.FromResult<UserValidationResult?>(null);

        return Task.FromResult<UserValidationResult?>(new UserValidationResult(new User
        {
            FullName = fields[0],
            Username = fields[1],
            Email = fields[2],
            Password = fields[3]
        }));
    }
}
