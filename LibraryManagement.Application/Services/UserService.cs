using LibraryManagement.Domain.Models;
using LibraryManagement.Domain.Repos;
using LibraryManagement.Domain.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Services
{
    public class UserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
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

            _unitOfWork.CreateTransaction();

            _userRepository.Add(user);

            try
            {
                _unitOfWork.SaveChanges();
                _unitOfWork.CommitTransaction();
            }
            catch(InvalidOperationException)
            {
                _unitOfWork.RollbackTransaction();
                throw;
            } 
        }

        public void Remove(int userId)
        {
            User userToRemove = _userRepository.GetById(userId) ?? throw new ArgumentException("User not found");

            _unitOfWork.CreateTransaction();
            _userRepository.Remove(userToRemove);

            try
            {
                _unitOfWork.SaveChanges();
                _unitOfWork.CommitTransaction();
            }
            catch (InvalidOperationException)
            {
                _unitOfWork.RollbackTransaction();
                throw;
            }
        }
    }
}
