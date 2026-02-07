using Architecture.Api.Domain.Models;

namespace Architecture.Api.Domain.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<bool> ExistsAsync(string email);
        Task<User> GetByEmailAsync(string email);
    }
}