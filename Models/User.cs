using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryManagementSystem.Enums;

namespace LibraryManagementSystem.Models
{
    public class User
    {
        #region Properties
        private int _id;
        public required int Id
        {
            get => _id;
            set
            {
                if (value < 0)
                    throw new ArgumentException("ID cannot be negative.");
                _id = value;
            }
        }

        private string _name = "";
        public required string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("Name cannot be null or empty.");
                _name = value;
            }
        }

        private string _email = "";
        public required string Email
        {
            get => _email;
            set
            {
                if (string.IsNullOrEmpty(value) || !value.Contains("@"))
                    throw new ArgumentException("Email cannot be null, empty, or invalid.");
                _email = value;
            }
        }

        private string _phoneNumber = "";
        public required string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                // if (string.IsNullOrEmpty(value) || !value.All(char.IsDigit))
                //     throw new ArgumentException("Phone number cannot be null, empty, or invalid.");
                _phoneNumber = value;
            }
        }

        private string _login = "";
        public required string Login
        {
            get => _login;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("Login cannot be null or empty.");
                _login = value;
            }
        }

        private string _password = "";
        public required string Password
        {
            get => _password;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("Password cannot be null or empty.");
                _password = value;
            }
        }

        public Dictionary<int, NotificationType> Subscriptions { get; private set; } = new Dictionary<int, NotificationType>();
        
        #endregion
    
        public void SubscribeToBookAvailability(int bookId, NotificationType notificationType)
        {
            if (Subscriptions.ContainsKey(bookId))
            {
                Subscriptions[bookId] = notificationType;
            }
            else
            {
                Subscriptions.Add(bookId, notificationType);
            }
        }

        public void UnsubscribeFromBookAvailability(int bookId)
        {
            if (Subscriptions.ContainsKey(bookId))
            {
                Subscriptions.Remove(bookId);
            }
        }
    }
}