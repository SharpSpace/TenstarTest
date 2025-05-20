using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services;

public sealed class UserService(IUserRepository repo) : IUserService
{
    public Task<IEnumerable<User>> GetAllUsersAsync() => repo.GetAllAsync();

    public Task SaveUsersAsync(List<User> users) => repo.AddAsync(users);
}