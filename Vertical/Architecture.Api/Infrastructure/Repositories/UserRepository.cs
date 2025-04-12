using Architecture.Api.Domain.Entities;
using Architecture.Api.Domain.Interfaces;
using Architecture.Api.Infrastructure.Database;

namespace Architecture.Api.Infrastructure.Repositories
{
    public class UserRepository(Context context) : Repository<User>(context), IUserRepository
    {
    }
}