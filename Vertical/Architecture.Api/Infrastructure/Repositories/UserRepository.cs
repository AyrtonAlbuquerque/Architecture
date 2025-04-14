using Architecture.Api.Domain.Entities;
using Architecture.Api.Domain.Interfaces;
using Architecture.Api.Infrastructure.Database;

namespace Architecture.Api.Infrastructure.Repositories
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