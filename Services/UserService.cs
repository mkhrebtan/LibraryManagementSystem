using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Services
{
    public class UserService
    {
        private readonly LibraryDbContext _context;

        public UserService(LibraryDbContext context)
        {
            _context = context;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _context.Users.AsNoTracking().ToList();
        }

        public User? GetUserById(int id)
        {
            return _context.Users.Find(id);
        }

        public void AddUser(User user)
        {
            try
            {
                _context.Users.Add(user);
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Failed to add user to database.", ex);
            }
        }

        public void RemoveUser(User userToDelete)
        {
            try
            {
                _context.Users.Remove(userToDelete);
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Failed to remove user from database. It may not exist.", ex);
            }
        }
    }
}
