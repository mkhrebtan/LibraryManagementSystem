# 📚 Library Management System

A .NET console application for managing library operations including book lending, user management, and notification subscriptions.

## Features

- **Book Management**: Add, remove, search, and track book availability
- **User Management**: Create and manage user accounts with authentication
- **Loan System**: Book lending, returns, and history tracking
- **Notification Subscriptions**: Users can subscribe to be notified when unavailable books become available

## Architecture

This project demonstrates **Clean Architecture** principles with clear separation of concerns across multiple layers:

### Clean Architecture Implementation
- **Domain Layer**: Core business entities and interfaces
- **Application Layer**: Business logic and use cases
- **Infrastructure Layer**: External services and notifications
- **Persistence Layer**: Data access with Entity Framework Core
- **Presentation Layer**: Console application interface

### Design Patterns Used

#### Repository Pattern & Unit of Work
- **Repository Pattern**: Abstracts data access with generic `IRepository<T>` interface
- **Unit of Work**: Manages transactions and ensures data consistency across multiple repositories

#### Notification System (Observer + Decorator + Factory)
- **Observer Pattern**: Event-driven notifications when books become available
- **Decorator Pattern**: Composable notification types (Basic, Email, SMS)
- **Factory Pattern**: Easy extension and maintenance of notification providers

The notification system simulates real notifications by logging to files, demonstrating how the patterns work together for extensibility.

## Technology Stack

- **.NET 9.0**
- **Entity Framework Core**
- **PostgreSQL**
- **Console Application**

## Setup & Installation

### Prerequisites
- .NET 9.0 SDK
- PostgreSQL database

### Database Setup
1. Install PostgreSQL and create a database
2. Update connection string in your user secrets
3. Run Entity Framework migrations:
```bash
dotnet ef database update --project LibraryManagement.Persistence.Postgres
```

### Running the Application
```bash
dotnet run --project LibraryManagement.ConsoleApp
```

## Usage

The console application provides an interactive menu system for:
- **Users**: Browse books, loan/return books, view history, subscribe to notifications
- **Admins**: Manage books and users

## Notification Simulation

The notification system demonstrates the design patterns by logging notifications to files:
- Basic notifications: `notifications.txt`
- Email notifications: `email_notifications.txt`  
- SMS notifications: `sms_notifications.txt`

This simulation shows how the Observer, Decorator, and Factory patterns work together to create an extensible notification system.

## License

This project is made for educational purposes to learn and demonstrate Clean Architecture principles in a library management context, as well as patterns like Repository, Unit of Work, Observer, Decorator, and Factory. It is not intended for production use.