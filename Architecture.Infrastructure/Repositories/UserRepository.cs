using Architecture.Domain.Interfaces;
using Architecture.Domain.Models;
using Architecture.Infrastructure.Abstractions;
using Architecture.Infrastructure.Database;

namespace Architecture.Infrastructure.Repositories
{
    public class UserRepository(Context context) : Repository<User>(context), IUserRepository
    {
    }

}