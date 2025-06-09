using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagement.Domain.Models;
using LibraryManagement.Domain.Repos;

namespace LibraryManagement.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IEnumerable<User> GetAll()
        {
            return _userRepository.GetAll();
        }

        public User? GetById(int id)
        {
            return _userRepository.GetById(id);
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

            _userRepository.Add(user);
        }

        public void Remove(int userId)
        {
            User userToRemove = _userRepository.GetById(userId) ?? throw new ArgumentException("User not found");
            _userRepository.Remove(userToRemove);
        }
    }
}
