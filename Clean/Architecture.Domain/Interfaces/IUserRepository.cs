using Architecture.Domain.Entities;

namespace Architecture.Domain.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<bool> ExistsAsync(string email);
        Task<User> GetByEmailAsync(string email);
    }
}