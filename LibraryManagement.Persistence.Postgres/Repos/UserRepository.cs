using LibraryManagement.Domain.Models;
using LibraryManagement.Domain.Repos;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Persistence.Postgres.Repos
{
    public class UserRepository : IUserRepository
    {
        private readonly LibraryDbContext _context;

        public UserRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public void Add(User entity)
        {
            try
            {
                _context.Users.Add(entity);
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Failed to add user to database.", ex);
            }
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users.AsNoTracking().ToList();
        }

        public User? GetById(int id)
        {
            return _context.Users.Find(id);
        }

        public void Remove(User enity)
        {
            try
            {
                _context.Users.Remove(enity);
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Failed to remove user from database.", ex);
            }
        }

        public void Update(User entity)
        {
            try
            {
                _context.Users.Update(entity);
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Failed to update user in database.", ex);
            }
        }
    }

}
