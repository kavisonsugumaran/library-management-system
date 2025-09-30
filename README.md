# Library Management System

A modern, scalable Library Management System built with C# 12 and .NET 8. This application provides robust features for managing books, members, and loan transactions in a library environment.

## Features

- **Book Management**: Add, update, delete, and view books.
- **Member Management**: Register, update, delete, and view library members.
- **Loan Management**: Issue and return books, track loan status, and manage due dates.
- **Repository & Unit of Work Pattern**: Clean separation of concerns for data access.
- **DTOs & AutoMapper**: Efficient data transfer and mapping between entities and view models.
- **RESTful API**: Easily integrate with front-end or other services.

## Technologies Used

- **C# 12**
- **.NET 8**
- **ASP.NET Core**
- **Entity Framework Core**
- **AutoMapper**

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/)
- SQL Server or SQLite (configurable in `appsettings.json`)

### Installation

1. **Clone the repository:**
   ```sh
   git clone https://github.com/kavisonsugumaran/library-management-system.git
   ```

2. **Navigate to the project directory:**
   ```sh
   cd library-management-system
   ```
   
3. **Restore dependencies:**
   ```sh
   dotnet restore
   ```

4. **Update database (if using EF Core migrations):**
   ```sh
   dotnet ef database update
   ```

5. **Run the application:**


## Usage

- Access the API endpoints via Swagger UI or Postman.
- Example endpoints:
- `GET /api/books` - List all books
- `POST /api/loans` - Create a new loan
- `PUT /api/loans/{id}/return` - Return a book

## Project Structure

- `Library.Application` - Application logic, services, DTOs
- `Library.Domain` - Domain entities and enums
- `Library.Infrastructure` - Data access, repositories
- `Library Management System` - API controllers and startup

## Contact

For questions or support, open an issue or contact [Kavison Sugumaran](https://github.com/kavisonsugumaran).

   
