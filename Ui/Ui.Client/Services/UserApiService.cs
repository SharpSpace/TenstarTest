using System.Net.Http.Json;
using Application.Interfaces;
using Domain.Entities;

namespace Ui.Client.Services;

public sealed class UserApiService(HttpClient http): IUserService
{
    public Task<IEnumerable<User>> GetAllUsersAsync() => throw new NotImplementedException();

    public async Task SaveUsersAsync(List<User> users)
    {
        var response = await http.PostAsJsonAsync("api/users", users);
        response.EnsureSuccessStatusCode();
    }
}