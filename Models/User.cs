using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Models;

public class User
{
#region Properties

    private int _id;

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id
    {
        get => _id;
        private set
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
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("Email cannot be null, or empty.");
            }  
            else if (!value.Contains("@"))
            {
                throw new ArgumentException("Invalid email.");
            }
            _email = value;
        }
    }

    private string _phoneNumber = "";
    public required string PhoneNumber
    {
        get => _phoneNumber;
        set
        {
            if (string.IsNullOrEmpty(value) || !value.All(char.IsDigit))
            {
                throw new ArgumentException("Invalid phone number.");
            }
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
            {
                throw new ArgumentException("Login cannot be null or empty.");
            }
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
            {
                throw new ArgumentException("Password cannot be null or empty.");
            }
            _password = value;
        }
    }
    
#endregion

    public override string ToString()
    {
        return $"ID: {Id}, Name: {Name}, Email: {Email}, Phone: {PhoneNumber}, Login: {Login}";
    }
}