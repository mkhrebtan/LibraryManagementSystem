using LibraryManagement.Domain.Models;
using LibraryManagement.Domain.Repos;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Persistence.Postgres.Repos
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(LibraryDbContext context) : base(context) { }
    }
}
