using Domain.Entities;

namespace Domain.Interfaces;

public interface IUserRepository
{
    Task AddAsync(IEnumerable<User> users);

    Task AddAsync(User user);

    Task<IEnumerable<User>> GetAllAsync();
}