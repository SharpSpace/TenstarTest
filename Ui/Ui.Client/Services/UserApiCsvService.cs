using System.Net.Http.Json;
using Application.DTOs;
using Application.Interfaces;

namespace Ui.Client.Services;

public sealed class UserApiCsvService(HttpClient http) : IUserCsvService
{
    public async IAsyncEnumerable<UserValidationResult> ParseAndValidateAsync(string csv)
    {
        var response = await http.PostAsJsonAsync("api/users/parse", csv);
        if (!response.IsSuccessStatusCode) yield break;

        var results = await response.Content.ReadFromJsonAsync<List<UserValidationResult>>();
        if (results is null) yield break;

        foreach (var result in results)
        {
            yield return result;
        }
    }
}