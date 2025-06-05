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

        public IEnumerable<User> GetAll()
        {
            return _context.Users.AsNoTracking().ToList();
        }

        public User? GetById(int id)
        {
            return _context.Users.Find(id);
        }

        public void Add(string name, string email, string phoneNumber, string login, string password)
        {
            var user = new User
            {
                Name = name,
                Email = email,
                PhoneNumber = phoneNumber,
                Login = login,
                Password = password
            };

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

        public void Remove(int userId)
        {
            var userToDelete = _context.Users.Find(userId) ?? throw new ArgumentException("User not found.");
            
            try
            {
                _context.Users.Remove(userToDelete);
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Failed to remove user from database", ex);
            }
        }
    }
}
