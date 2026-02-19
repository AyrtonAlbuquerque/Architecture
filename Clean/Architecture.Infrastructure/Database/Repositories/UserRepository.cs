using Architecture.Domain.Interfaces;
using Architecture.Domain.Models;

namespace Architecture.Infrastructure.Database.Repositories
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