using Domain.Entities;

namespace Application.Interfaces;

public interface IUserService
{
    public Task<IEnumerable<User>> GetAllUsersAsync();

    public Task SaveUsersAsync(List<User> users);
}