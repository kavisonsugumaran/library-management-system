# Library Management System

A modern, cross-platform Library Management System built with ASP.NET Core (.NET 8). This project provides RESTful APIs for managing books, authors, members, and loans in a library context.

## Features

- Manage books, authors, members, and loans
- CRUD operations for all entities
- Validation using FluentValidation
- Entity Framework Core for data access
- AutoMapper for DTO mapping
- Swagger/OpenAPI documentation
- Database seeding and migration support

## Technologies Used

- .NET 8 (ASP.NET Core Web API)
- Entity Framework Core
- AutoMapper
- FluentValidation
- Swagger (Swashbuckle)
- SQL Server (default, configurable)

# Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server (or update connection string for your DB)
- Visual Studio 2022 or VS Code

### Setup Instructions

1. **Clone the repository**
   
2. **Configure the database**
- Update the connection string in `appsettings.json` if needed.

3. **Apply migrations and seed data**
- Open a terminal in the project root and run:
  ```sh
  dotnet ef database update
  ```
- The application will also apply migrations and seed data on startup.

4. **Run the application**
