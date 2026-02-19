using Architecture.Api.Domain.Models;
using Architecture.Api.Domain.Interfaces;

namespace Architecture.Api.Infrastructure.Database.Repositories
{
    public class UserRepository(Context context) : Repository<User>(context), IUserRepository
    {
        public async Task<bool> ExistsAsync(string email)
        {
            return await ExistsAsync(q => q
                .Where(x => x.Email == email));
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await GetAsync(q => q
                .Where(x => x.Email == email));
        }
    }
}