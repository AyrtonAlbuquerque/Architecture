using Architecture.Api.Domain.Entities;

namespace Architecture.Api.Domain.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByEmailAsync(string email);
    }
}