# Persons, Accounts and Transactions Management System (PAT)

A new ASP.NET Core MVC web solution for managing people, their accounts, and financial transactions with secure authentication and business logic validation.



---

## Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Technologies](#technologies)
- [Screenshots](#screenshots)
- [Quick Start](#quick-start)
- [Documentation](#documentation)
- [Project Structure](#project-structure)
- [Color Scheme](#color-scheme)
- [Business Rules](#business-rules)
- [Contributing](#contributing)
- [License](#license)
- [Contact](#contact)

---

## Overview

The Persons, Accounts and Transactions Management System (PAT) is a modern web application designed to manage customer information, financial accounts, and transactions. It provides a secure, user-friendly interface with comprehensive validation and business rule enforcement.

**Purpose**: Technical Skills Assessment Project  
**Developer**: Nathan Mazonde  
**Date**: 09 November 2025  
**Framework**: ASP.NET Core MVC (.NET 8)

---

## Features

### Authentication & Security
- Secure login system with BCrypt password hashing
- Session-based authentication
- User role management
- Password strength validation

### Person Management
- (CRUD) Create, Read, Update, Delete operations
- Unique ID number validation (13-digit South African ID)
- Contact information management (email, phone, address)
- Search by ID Number or Surname
- Account relationship tracking

### Account Management
- Multiple and unlimited number of accounts per person
- Account status tracking (Open/Closed)
- Automatic balance calculation
- Unique account number enforcement
- Transaction history per account
- Close account validation (balance must be zero)

### Transaction Management
- Debit and Credit transaction recording
- Automatic balance updates
- Date validation (cannot be in future)
- Amount validation (cannot be zero)
- Capture date tracking
- Edit and delete with balance recalculation
- Prevents transactions on closed accounts

### Search Functionality
- Search by Person Surname
- Search by ID Number
- Search by Account Number
- Integrated search on Persons page

### Dashboard
- Real-time statistics overview
- Total persons, accounts, and transactions
- Account status breakdown (Open/Closed)
- Total balance summary
- Recent activity tracking
- Quick action buttons

---

## Technologies

### Backend
- **ASP.NET Core MVC** (.NET 8)
- **Entity Framework Core** (ORM)
- **SQL Server LocalDB** (Database)
- **BCrypt.Net** (Password Hashing)
- **C# 10/11** (Programming Language)

### Frontend
- **Razor Views** (View Engine)
- **Bootstrap 5** (UI Framework)
- **Font Awesome 6** (Icons)
- **jQuery** (JavaScript Library)
- **Custom CSS** (Styling)

### Architecture & Patterns
- **MVC Pattern** (Model-View-Controller)
- **Repository Pattern** (Service Layer)
- **Dependency Injection** (IoC Container)
- **SOLID Principles** (Code Design)
- **Entity Framework Migrations** (Database Versioning)

---

## Screenshots

### Dashboard
![Dashboard](Documentation/screenshots/dashboard.jpeg)

### Person Management
![Persons List](Documentation/screenshots/persons-list.jpeg)

### Account Details
![Account Details](Documentation/screenshots/account-details.jpeg)

### Transaction Management
![Transactions](Documentation/screenshots/transactions.jpeg)

---

## Quick Start

### Requirements
- Visual Studio 2022 or later
- .NET 8.0 SDK
- SQL Server 2019+ or SQL Server LocalDB (included with Visual Studio)

### Installation

1. **Clone the repository**
```bash
   git clone https://github.com/natelab/traq-web-project-assessment.git
   cd traq-web-project-assessment
```

2. **Open in Visual Studio**
   - Open `traq-web-project-assessment.sln`

3. **Restore NuGet Packages**
   - Right-click solution → Restore NuGet Packages
   - Or run: `dotnet restore`

4. **Update Database Connection** (Optional)
   - Open `appsettings.json`
   - Modify connection string if needed (defaults to LocalDB)

5. **Run Migrations**
```powershell
   Update-Database
```
   Or using CLI:
```bash
   dotnet ef database update
```

6. **Run the Application**
   - Press `F5` in Visual Studio
   - Or run: `dotnet run`

7. **Login**
   - **Username**: admin
   - **Password**: Admin123

---

## Documentation

Detailed documentation is available in the `Documentation/` folder:

- **[SETUP.md](Documentation/SETUP.md)** - Complete installation and configuration guide
- **[USER_GUIDE.md](Documentation/USER_GUIDE.md)** - How to use the application
- **[DATABASE_SCHEMA.md](Documentation/DATABASE_SCHEMA.md)** - Database structure and relationships
- **[API_DOCUMENTATION.md](Documentation/API_DOCUMENTATION.md)** - API endpoints (if applicable)

---

## 📁 Project Structure
```
traq-web-project-assessment/
├── Controllers/               # MVC Controllers
│   ├── HomeController.cs     # Dashboard & static pages
│   ├── AuthController.cs     # Authentication
│   ├── PersonsController.cs  # Person management
│   ├── AccountsController.cs # Account management
│   └── TransactionsController.cs # Transaction management
├── Models/                    # Entity models
│   ├── Person.cs
│   ├── Account.cs
│   ├── Transaction.cs
│   ├── User.cs
│   └── Status.cs
├── Views/                     # Razor views
│   ├── Home/
│   ├── Auth/
│   ├── Persons/
│   ├── Accounts/
│   ├── Transactions/
│   └── Shared/
│       └── _Layout.cshtml
├── ViewModels/                # Data transfer objects
│   ├── PersonViewModel.cs
│   ├── AccountViewModel.cs
│   ├── TransactionViewModel.cs
│   └── DashboardViewModel.cs
├── Services/                  # Business logic layer
│   ├── IPersonService.cs
│   ├── PersonService.cs
│   ├── IAccountService.cs
│   ├── AccountService.cs
│   ├── ITransactionService.cs
│   ├── TransactionService.cs
│   ├── IAuthService.cs
│   └── AuthService.cs
├── Data/                      # Database context
│   └── ApplicationDbContext.cs
├── Migrations/                # EF Core migrations
├── wwwroot/                   # Static files
│   ├── css/
│   ├── js/
│   └── lib/
├── Documentation/             # Project documentation
├── appsettings.json          # Configuration
├── Program.cs                # Application entry point
└── README.md                 # This file
```

---

## Color Scheme

The application uses a modern, professional color palette:

| Color Name        | Hex Code  | Usage                          |
|-------------------|-----------|--------------------------------|
| Charcoal          | `#1C1C1E` | Navbar, Footer, Dark Elements  |
| Dark Pastel Blue  | `#2F527A` | Primary Actions, Links, Accents|
| Light Grey        | `#F5F5F7` | Page Background, Subtle Areas  |
| White             | `#FFFFFF` | Cards, Content Backgrounds     |

All UI elements feature rounded corners (8-15px radius) for a modern aesthetic.

---

## Business Rules

### Person Management
- ✅ ID Number must be unique (13 digits)
- ✅ Person can have unlimited accounts
- ✅ Person can only be deleted if they have no accounts OR all accounts are closed
- ✅ First Name and Surname are required
- ✅ Email and Phone are optional but validated if provided

### Account Management
- ✅ Account Number must be unique
- ✅ New accounts can only be added AFTER person is created
- ✅ User cannot manually change account balance
- ✅ Account can have unlimited transactions
- ✅ Outstanding balance updates automatically with transactions
- ✅ Account can only be closed if balance is zero
- ✅ No transactions can be posted to closed accounts

### Transaction Management
- ✅ Transaction date cannot be in the future
- ✅ New transactions can only be added AFTER account is created
- ✅ User cannot change capture date (set automatically)
- ✅ User must enter either debit OR credit, not both
- ✅ Transaction amount cannot be zero
- ✅ Balance recalculates automatically on add/edit/delete

---

## Contributing

This is a skills assessment project, but suggestions and feedback are welcome!

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

---

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

## 📧 Contact

**Developer**: Nathan Mazonde  
**Email**: mazonebiz@gmail.com  
**LinkedIn**: [linkedin.com/in/nathan-mazonde](https://linkedin.com/in/nathan-mazonde)  
**GitHub**: [github.com/natelab](https://github.com/natelab)  
**Location**: Johannesburg, South Africa  

**Project Repository**: [https://github.com/natelab/traq-web-project-assessment](https://github.com/yourusername/traq-web-project-assessment)

---

## Acknowledgments

- Font Awesome for their icons
- Bootstrap team for the UI framework
- Microsoft for ASP.NET Core
- BCrypt.Net-Next for secure password hashing
- Entity Framework Core team

---

## Project Statistics

- **Lines of Code**: ~5,000+
- **Development Time**: 3-4 Days
- **Controllers**: 5
- **Models**: 5
- **Views**: 25+
- **Services**: 4

---

<div align="center">

**⭐ If you find this project useful, please consider giving it a star! ⭐**

Made by Nate :)

</div>
