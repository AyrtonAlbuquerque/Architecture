using Architecture.Domain.Interfaces;
using Architecture.Domain.Entities;
using Architecture.Infrastructure.Database;

namespace Architecture.Infrastructure.Repositories
{
    public class UserRepository(Context context) : Repository<User>(context), IUserRepository
    {
        public async Task<User> GetByEmailAsync(string email)
        {
            return await GetAsync(q => q
                .Where(x => x.Email == email));
        }
    }
}