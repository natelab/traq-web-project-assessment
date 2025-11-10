# Setup & Installation Guide

Complete step-by-step instructions for setting up the Person Account Management System.

---

## Table of Contents

- [System Requirements](#system-requirements)
- [Installation Steps](#installation-steps)
- [Database Setup](#database-setup)
- [Configuration](#configuration)
- [Running the Application](#running-the-application)
- [Troubleshooting](#troubleshooting)
- [Development Setup](#development-setup)

---

## System Requirements

### Minimum Requirements

| Component | Requirement |
|-----------|-------------|
| Operating System | Windows 10/11, macOS 10.15+, or Linux |
| IDE | Visual Studio 2022 (Community or higher) |
| .NET SDK | .NET 8.0 |
| Database | SQL Server 2019+ or LocalDB |
| RAM | 4GB minimum, 8GB recommended |
| Disk Space | 2GB free space |
| Browser | Chrome, Edge, Firefox, or Safari (latest versions) |

### Recommended Tools

- **Microsoft SQL Server Management Studio (SSMS)** - For database management
- **Git** - For version control
- **Postman** - For API testing (if using Web API)

---

## Installation Steps

### Step 1: Install Prerequisites

#### 1.1 Install Visual Studio 2022

1. Download from [visualstudio.microsoft.com](https://visualstudio.microsoft.com/)
2. During installation, select these workloads:
   - ASP.NET and web development
   - Data storage and processing (includes SQL Server LocalDB)
3. Individual components (optional):
   - SQL Server Express LocalDB

#### 1.2 Verify .NET SDK Installation

Open Command Prompt or Terminal:
```bash
dotnet --version
```

Should display:  `8.0.x`

If not installed, download from [dot.net](https://dot.net)

---

### Step 2: Clone the Repository

#### Option A: Using Git Command Line
```bash
git clone https://github.com/natelab/traq-web-project-assessment.git
cd traq-web-project-assessment
```

#### Option B: Using Visual Studio

1. Open Visual Studio
2. **File** → **Clone Repository**
3. Enter repository URL
4. Choose local path
5. Click **Clone**

#### Option C: Download ZIP

1. Go to GitHub repository
2. Click **Code** → **Download ZIP**
3. Extract to desired location

---

### Step 3: Open Solution in Visual Studio

1. Navigate to project folder
2. Double-click `traq-web-project-assessment.sln`
3. Visual Studio will open the solution

---

### Step 4: Restore NuGet Packages

**Automatic (Recommended):**
- Visual Studio automatically restores packages on first load

**Manual:**
```bash
dotnet restore
```

Or in Visual Studio:
- Right-click **Solution** → **Restore NuGet Packages**

---

## Database Setup

### Option 1: Using SQL Server LocalDB (Recommended)

LocalDB is included with Visual Studio and requires no configuration.

#### Verify LocalDB is Installed
```powershell
sqllocaldb info
```

You should see `mssqllocaldb` listed.

#### If Not Installed, Create Instance
```powershell
sqllocaldb create mssqllocaldb
sqllocaldb start mssqllocaldb
```

#### Connection String (Already Configured)

In `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=PersonAccountDB;Trusted_Connection=true;MultipleActiveResultSets=true"
}
```

---

### Option 2: Using SQL Server Express

#### Install SQL Server Express

1. Download from [microsoft.com/sql-server](https://www.microsoft.com/sql-server/sql-server-downloads)
2. Install **Express** edition
3. Note the instance name (usually `SQLEXPRESS`)

#### Update Connection String

In `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.\\SQLEXPRESS;Database=PersonAccountDB;Trusted_Connection=true;MultipleActiveResultSets=true"
}
```

---

### Option 3: Using Full SQL Server

#### Update Connection String

**Windows Authentication:**
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=PersonAccountDB;Trusted_Connection=true;MultipleActiveResultSets=true"
}
```

**SQL Authentication:**
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=PersonAccountDB;User Id=sa;Password=YourPassword;MultipleActiveResultSets=true"
}
```

---

### Step 5: Run Database Migrations

#### Using Package Manager Console (Visual Studio)

1. **Tools** → **NuGet Package Manager** → **Package Manager Console**
2. Ensure **Default project** is set to your project
3. Run:
```powershell
Update-Database
```

#### Using .NET CLI
```bash
dotnet ef database update
```

#### What This Does:
- Creates `PersonAccountDB` database
- Creates all tables (Persons, Accounts, Transactions, Users, Statuses)
- Seeds initial data:
  - Status: "Open" and "Closed"
  - Admin User: username `admin`, password `Admin123`

---

### Step 6: Verify Database Creation

#### Using SQL Server Object Explorer (Visual Studio)

1. **View** → **SQL Server Object Explorer**
2. Expand: **(localdb)\mssqllocaldb** → **Databases**
3. You should see **PersonAccountDB**
4. Expand **Tables** to see:
   - dbo.Accounts
   - dbo.Persons
   - dbo.Statuses
   - dbo.Transactions
   - dbo.Users

#### Using SSMS (Optional)

1. Open SQL Server Management Studio
2. Connect to: `(localdb)\mssqllocaldb`
3. Browse to **PersonAccountDB**

---

## Configuration

### appsettings.json Overview
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Your connection string here"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

### Environment-Specific Settings

**Development** (`appsettings.Development.json`):
- Already configured with LocalDB
- Detailed logging enabled

**Production** (create `appsettings.Production.json`):
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Your production connection string"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Warning"
    }
  }
}
```

---

## Running the Application

### Option 1: Using Visual Studio

1. Press **F5** (Run with debugging)
2. Or **Ctrl + F5** (Run without debugging)
3. Browser opens automatically to `https://localhost:XXXX`

### Option 2: Using .NET CLI
```bash
dotnet run
```

Then open browser to displayed URL (e.g., `https://localhost:5001`)

### Option 3: Using IIS Express

1. In Visual Studio toolbar
2. Click dropdown next to green play button
3. Select **IIS Express**
4. Click play button

---

## First Login

1. Navigate to the application URL
2. Click **Login**
3. Enter credentials:
   - **Username**: `admin`
   - **Password**: `Admin123`
4. Click **Login**

You should be redirected to the Dashboard.

---

## Troubleshooting

### Issue: "Build failed"

**Solution:**
```bash
dotnet clean
dotnet restore
dotnet build
```

---

### Issue: "Cannot connect to database"

**Check 1: LocalDB is running**
```powershell
sqllocaldb info mssqllocaldb
sqllocaldb start mssqllocaldb
```

**Check 2: Connection string is correct**
- Open `appsettings.json`
- Verify server name matches your setup

**Check 3: Database exists**
- Run `Update-Database` again

---

### Issue: "Login fails with correct credentials"

**Solution:** Re-run migrations to ensure admin user is seeded:
```powershell
dotnet ef database drop
dotnet ef database update
```

---

### Issue: "NuGet packages not restored"

**Solution:**
```bash
dotnet restore --force
```

---

### Issue: "Migrations not found"

**Solution:** Ensure EF Core tools are installed:
```bash
dotnet tool install --global dotnet-ef
```

---

### Issue: "Port already in use"

**Solution:** Change port in `launchSettings.json`:
```json
"applicationUrl": "https://localhost:7001;http://localhost:5001"
```

---

## Development Setup

### Recommended VS Extensions

- **Web Essentials** - Enhanced CSS/JS/HTML editing
- **ReSharper** - Code quality (optional, paid)
- **CodeMaid** - Code cleanup
- **GitLens** - Enhanced Git integration

### Creating New Migrations

When you modify models:
```powershell
Add-Migration YourMigrationName
Update-Database
```

Or using CLI:
```bash
dotnet ef migrations add YourMigrationName
dotnet ef database update
```

### Resetting Database

**Drop and recreate:**
```powershell
Drop-Database
Update-Database
```

Or:
```bash
dotnet ef database drop
dotnet ef database update
```

---

## Building for Production

### Publish Using Visual Studio

1. Right-click project → **Publish**
2. Choose target (Folder, Azure, IIS, etc.)
3. Configure settings
4. Click **Publish**

### Publish Using CLI
```bash
dotnet publish -c Release -o ./publish
```

Files will be in `./publish` folder.

---

## Next Steps

After successful setup:

1. Read [USER_GUIDE.md](USER_GUIDE.md) to learn how to use the application
2. Review [DATABASE_SCHEMA.md](DATABASE_SCHEMA.md) to understand the data model
3. Explore the codebase starting with `Program.cs`

---

## Support

If you encounter issues not covered here:

1. Check the [Troubleshooting](#troubleshooting) section
2. Review the [GitHub Issues](https://github.com/natelab/traq-web-project-assessment/issues)
3. Contact: mazonebiz@gmail.com

---

**Good news the setup is complete! You're ready to start using the PAT application.** 