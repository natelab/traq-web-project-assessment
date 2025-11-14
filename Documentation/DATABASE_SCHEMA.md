# Database Schema Documentation

Complete database structure and relationships for the Person Account Management System.

---

## Table of Contents

- [Overview](#overview)
- [Entity Relationship Diagram](#entity-relationship-diagram)
- [Tables](#tables)
- [Relationships](#relationships)
- [Indexes](#indexes)
- [Constraints](#constraints)
- [Seed Data](#seed-data)
- [Database Queries](#database-queries)

---

## Overview

**Database Name:** `PersonAccountDB`

**Database Engine:** SQL Server (2019+) or LocalDB

**ORM:** Entity Framework Core

**Total Tables:** 5
- Persons
- Accounts
- Transactions
- Users
- Statuses

---

## Entity Relationship Diagram
```
┌─────────────┐
│    Users    │
│             │
│ Id (PK)     │
│ Username    │
│ PasswordHash│
│ FullName    │
│ CreatedDate │
│ IsActive    │
└─────────────┘

┌─────────────────┐         ┌──────────────────┐         ┌──────────────────┐
│    Persons      │         │    Accounts      │         │  Transactions    │
│                 │         │                  │         │                  │
│ Id (PK)         │────────<│ PersonId (FK)    │────────<│ AccountId (FK)   │
│ IdNumber (UQ)   │    1:N  │ Id (PK)          │    1:N  │ Id (PK)          │
│ FirstName       │         │ AccountNumber(UQ)│         │ TransactionDate  │
│ Surname         │         │ AccountName      │         │ DebitAmount      │
│ Address         │         │ OutstandingBal   │         │ CreditAmount     │
│ PhoneNumber     │         │ StatusId (FK)    │         │ Description      │
│ Email           │         └──────────────────┘         │ CaptureDate      │
└─────────────────┘                  │                   └──────────────────┘
                                     │
                                     │ N:1
                                     │
                            ┌────────┴────────┐
                            │    Statuses     │
                            │                 │
                            │ Id (PK)         │
                            │ StatusName      │
                            └─────────────────┘
```

**Legend:**
- `PK` = Primary Key
- `FK` = Foreign Key
- `UQ` = Unique Constraint
- `1:N` = One to Many relationship
- `N:1` = Many to One relationship

---

## Tables

### 1. Persons Table

Stores information about individuals in the system.
```sql
CREATE TABLE [dbo].[Persons] (
    [Id] INT NOT NULL IDENTITY(1,1),
    [IdNumber] NVARCHAR(13) NOT NULL,
    [FirstName] NVARCHAR(100) NOT NULL,
    [Surname] NVARCHAR(100) NOT NULL,
    [Address] NVARCHAR(200) NULL,
    [PhoneNumber] NVARCHAR(20) NULL,
    [Email] NVARCHAR(100) NULL,
    CONSTRAINT [PK_Persons] PRIMARY KEY ([Id]),
    CONSTRAINT [UQ_Persons_IdNumber] UNIQUE ([IdNumber])
);
```

**Columns:**

| Column | Type | Nullable | Description |
|--------|------|----------|-------------|
| Id | INT | No | Primary key, auto-increment |
| IdNumber | NVARCHAR(13) | No | South African ID number (13 digits), unique |
| FirstName | NVARCHAR(100) | No | Person's first name |
| Surname | NVARCHAR(100) | No | Person's surname/last name |
| Address | NVARCHAR(200) | Yes | Physical address (optional) |
| PhoneNumber | NVARCHAR(20) | Yes | Contact phone number (optional) |
| Email | NVARCHAR(100) | Yes | Email address (optional) |

**Business Rules:**
- ID Number must be exactly 13 characters
- ID Number must be unique across all persons
- First Name and Surname are mandatory
- Email must be valid format if provided
- Phone must be valid format if provided

---

### 2. Statuses Table

Lookup table for account statuses.
```sql
CREATE TABLE [dbo].[Statuses] (
    [Id] INT NOT NULL IDENTITY(1,1),
    [StatusName] NVARCHAR(50) NOT NULL,
    CONSTRAINT [PK_Statuses] PRIMARY KEY ([Id])
);
```

**Columns:**

| Column | Type | Nullable | Description |
|--------|------|----------|-------------|
| Id | INT | No | Primary key, auto-increment |
| StatusName | NVARCHAR(50) | No | Status name ("Open" or "Closed") |

**Seed Data:**
- Id: 1, StatusName: "Open"
- Id: 2, StatusName: "Closed"

---

### 3. Accounts Table

Stores account information linked to persons.
```sql
CREATE TABLE [dbo].[Accounts] (
    [Id] INT NOT NULL IDENTITY(1,1),
    [AccountNumber] NVARCHAR(20) NOT NULL,
    [AccountName] NVARCHAR(100) NOT NULL,
    [OutstandingBalance] DECIMAL(18, 2) NOT NULL DEFAULT 0,
    [PersonId] INT NOT NULL,
    [StatusId] INT NOT NULL DEFAULT 1,
    CONSTRAINT [PK_Accounts] PRIMARY KEY ([Id]),
    CONSTRAINT [UQ_Accounts_AccountNumber] UNIQUE ([AccountNumber]),
    CONSTRAINT [FK_Accounts_Persons] FOREIGN KEY ([PersonId]) 
        REFERENCES [dbo].[Persons]([Id]) ON DELETE RESTRICT,
    CONSTRAINT [FK_Accounts_Statuses] FOREIGN KEY ([StatusId]) 
        REFERENCES [dbo].[Statuses]([Id]) ON DELETE RESTRICT
);
```

**Columns:**

| Column | Type | Nullable | Description |
|--------|------|----------|-------------|
| Id | INT | No | Primary key, auto-increment |
| AccountNumber | NVARCHAR(20) | No | Unique account identifier |
| AccountName | NVARCHAR(100) | No | Descriptive name for account |
| OutstandingBalance | DECIMAL(18,2) | No | Current balance, defaults to 0 |
| PersonId | INT | No | Foreign key to Persons table |
| StatusId | INT | No | Foreign key to Statuses table, defaults to 1 (Open) |

**Business Rules:**
- Account Number must be unique across all accounts
- Outstanding Balance is calculated automatically from transactions
- Users cannot manually edit Outstanding Balance
- New accounts default to "Open" status
- Accounts can only be closed if balance is exactly 0
- Cannot delete person if they have accounts (RESTRICT)

---

### 4. Transactions Table

Records all financial transactions for accounts.
```sql
CREATE TABLE [dbo].[Transactions] (
    [Id] INT NOT NULL IDENTITY(1,1),
    [TransactionDate] DATETIME2 NOT NULL,
    [DebitAmount] DECIMAL(18, 2) NOT NULL DEFAULT 0,
    [CreditAmount] DECIMAL(18, 2) NOT NULL DEFAULT 0,
    [Description] NVARCHAR(500) NOT NULL,
    [CaptureDate] DATETIME2 NOT NULL,
    [AccountId] INT NOT NULL,
    CONSTRAINT [PK_Transactions] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Transactions_Accounts] FOREIGN KEY ([AccountId]) 
        REFERENCES [dbo].[Accounts]([Id]) ON DELETE CASCADE
);
```

**Columns:**

| Column | Type | Nullable | Description |
|--------|------|----------|-------------|
| Id | INT | No | Primary key, auto-increment |
| TransactionDate | DATETIME2 | No | Date of the transaction |
| DebitAmount | DECIMAL(18,2) | No | Amount debited (money out), defaults to 0 |
| CreditAmount | DECIMAL(18,2) | No | Amount credited (money in), defaults to 0 |
| Description | NVARCHAR(500) | No | Transaction description |
| CaptureDate | DATETIME2 | No | System date when transaction was recorded |
| AccountId | INT | No | Foreign key to Accounts table |

**Business Rules:**
- Transaction Date cannot be in the future
- Either DebitAmount OR CreditAmount must be > 0 (not both, not neither)
- Transaction amount cannot be zero
- Description is mandatory
- Capture Date is set automatically by the system
- Users cannot modify Capture Date
- Cannot post transactions to closed accounts
- Deleting account cascades delete to all transactions

**Balance Calculation:**
```
Outstanding Balance = SUM(CreditAmount) - SUM(DebitAmount)
```

---

### 5. Users Table

Stores user authentication information.
```sql
CREATE TABLE [dbo].[Users] (
    [Id] INT NOT NULL IDENTITY(1,1),
    [Username] NVARCHAR(50) NOT NULL,
    [PasswordHash] NVARCHAR(255) NOT NULL,
    [FullName] NVARCHAR(100) NULL,
    [CreatedDate] DATETIME2 NOT NULL,
    [IsActive] BIT NOT NULL DEFAULT 1,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id]),
    CONSTRAINT [UQ_Users_Username] UNIQUE ([Username])
);
```

**Columns:**

| Column | Type | Nullable | Description |
|--------|------|----------|-------------|
| Id | INT | No | Primary key, auto-increment |
| Username | NVARCHAR(50) | No | Login username, unique |
| PasswordHash | NVARCHAR(255) | No | BCrypt hashed password |
| FullName | NVARCHAR(100) | Yes | User's full name |
| CreatedDate | DATETIME2 | No | Date user was created |
| IsActive | BIT | No | Whether user account is active, defaults to 1 (true) |

**Security:**
- Passwords are hashed using BCrypt with work factor 12
- Username must be unique
- Plain text passwords are never stored

**Seed Data:**
- Username: "admin"
- Password: "Admin123" (hashed)
- Full Name: "System Administrator"
- Created Date: 2024-01-01
- Is Active: true

---

## Relationships

### Person → Accounts (One-to-Many)
```
Persons (1) ────< Accounts (N)
```

- One person can have multiple accounts
- Each account belongs to exactly one person
- Delete behavior: **RESTRICT** (cannot delete person with accounts)

**C# Navigation:**
```csharp
// From Person
person.Accounts // Collection of Account objects

// From Account
account.Person // Parent Person object
account.PersonId // Foreign key value
```

---

### Account → Transactions (One-to-Many)
```
Accounts (1) ────< Transactions (N)
```

- One account can have multiple transactions
- Each transaction belongs to exactly one account
- Delete behavior: **CASCADE** (deleting account deletes all its transactions)

**C# Navigation:**
```csharp
// From Account
account.Transactions // Collection of Transaction objects

// From Transaction
transaction.Account // Parent Account object
transaction.AccountId // Foreign key value
```

---

### Status → Accounts (One-to-Many)
```
Statuses (1) ────< Accounts (N)
```

- One status can be assigned to multiple accounts
- Each account has exactly one status
- Delete behavior: **RESTRICT** (cannot delete status in use)

**C# Navigation:**
```csharp
// From Status
status.Accounts // Collection of Account objects

// From Account
account.Status // Status object
account.StatusId // Foreign key value
```

---

## Indexes

### Unique Indexes (Enforced by Constraints)
```sql
-- Person ID Number (Unique)
CREATE UNIQUE INDEX [UQ_Persons_IdNumber] 
ON [dbo].[Persons]([IdNumber]);

-- Account Number (Unique)
CREATE UNIQUE INDEX [UQ_Accounts_AccountNumber] 
ON [dbo].[Accounts]([AccountNumber]);

-- Username (Unique)
CREATE UNIQUE INDEX [UQ_Users_Username] 
ON [dbo].[Users]([Username]);
```

### Foreign Key Indexes (Automatically Created)
```sql
-- Account PersonId
CREATE INDEX [IX_Accounts_PersonId] 
ON [dbo].[Accounts]([PersonId]);

-- Account StatusId
CREATE INDEX [IX_Accounts_StatusId] 
ON [dbo].[Accounts]([StatusId]);

-- Transaction AccountId
CREATE INDEX [IX_Transactions_AccountId] 
ON [dbo].[Transactions]([AccountId]);
```

### Recommended Additional Indexes (Performance)
```sql
-- For searching by surname
CREATE INDEX [IX_Persons_Surname] 
ON [dbo].[Persons]([Surname]);

-- For filtering transactions by date
CREATE INDEX [IX_Transactions_TransactionDate] 
ON [dbo].[Transactions]([TransactionDate]);

-- For filtering active users
CREATE INDEX [IX_Users_IsActive] 
ON [dbo].[Users]([IsActive]);
```

---

## Constraints

### Primary Key Constraints
```sql
ALTER TABLE [dbo].[Persons] 
    ADD CONSTRAINT [PK_Persons] PRIMARY KEY ([Id]);

ALTER TABLE [dbo].[Accounts] 
    ADD CONSTRAINT [PK_Accounts] PRIMARY KEY ([Id]);

ALTER TABLE [dbo].[Transactions] 
    ADD CONSTRAINT [PK_Transactions] PRIMARY KEY ([Id]);

ALTER TABLE [dbo].[Users] 
    ADD CONSTRAINT [PK_Users] PRIMARY KEY ([Id]);

ALTER TABLE [dbo].[Statuses] 
    ADD CONSTRAINT [PK_Statuses] PRIMARY KEY ([Id]);
```

### Foreign Key Constraints
```sql
-- Account to Person
ALTER TABLE [dbo].[Accounts]
    ADD CONSTRAINT [FK_Accounts_Persons] 
    FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Persons]([Id])
    ON DELETE RESTRICT; -- Cannot delete person with accounts

-- Account to Status
ALTER TABLE [dbo].[Accounts]
    ADD CONSTRAINT [FK_Accounts_Statuses] 
    FOREIGN KEY ([StatusId]) REFERENCES [dbo].[Statuses]([Id])
    ON DELETE RESTRICT; -- Cannot delete status in use

-- Transaction to Account
ALTER TABLE [dbo].[Transactions]
    ADD CONSTRAINT [FK_Transactions_Accounts] 
    FOREIGN KEY ([AccountId]) REFERENCES [dbo].[Accounts]([Id])
    ON DELETE CASCADE; -- Deleting account removes transactions
```

### Unique Constraints
```sql
-- Unique ID Number
ALTER TABLE [dbo].[Persons]
    ADD CONSTRAINT [UQ_Persons_IdNumber] UNIQUE ([IdNumber]);

-- Unique Account Number
ALTER TABLE [dbo].[Accounts]
    ADD CONSTRAINT [UQ_Accounts_AccountNumber] UNIQUE ([AccountNumber]);

-- Unique Username
ALTER TABLE [dbo].[Users]
    ADD CONSTRAINT [UQ_Users_Username] UNIQUE ([Username]);
```

### Check Constraints (Application Layer)

While not enforced at database level, these are validated in the application:
```sql
-- Transaction Date not in future (application validates)
-- Transaction amount not zero (application validates)
-- Either Debit OR Credit, not both (application validates)
```

---

## Seed Data

### Statuses Table
```sql
INSERT INTO [dbo].[Statuses] ([Id], [StatusName])
VALUES 
    (1, 'Open'),
    (2, 'Closed');
```

### Users Table
```sql
INSERT INTO [dbo].[Users] 
    ([
**Note:** PasswordHash is BCrypt hash of "Admin123"

---

## Database Queries

### Common SQL Queries

#### 1. Get All Persons with Account Count
```sql
SELECT 
    p.Id,
    p.IdNumber,
    p.FirstName,
    p.Surname,
    p.Email,
    p.PhoneNumber,
    COUNT(a.Id) AS AccountCount
FROM Persons p
LEFT JOIN Accounts a ON p.Id = a.PersonId
GROUP BY p.Id, p.IdNumber, p.FirstName, p.Surname, p.Email, p.PhoneNumber
ORDER BY p.Surname, p.FirstName;
```

---

#### 2. Get Person with All Accounts
```sql
SELECT 
    p.*,
    a.Id AS AccountId,
    a.AccountNumber,
    a.AccountName,
    a.OutstandingBalance,
    s.StatusName
FROM Persons p
LEFT JOIN Accounts a ON p.Id = a.PersonId
LEFT JOIN Statuses s ON a.StatusId = s.Id
WHERE p.Id = @PersonId
ORDER BY a.AccountNumber;
```

---

#### 3. Get Account with Transaction Summary
```sql
SELECT 
    a.Id,
    a.AccountNumber,
    a.AccountName,
    a.OutstandingBalance,
    s.StatusName,
    p.FirstName + ' ' + p.Surname AS PersonName,
    COUNT(t.Id) AS TransactionCount,
    SUM(t.DebitAmount) AS TotalDebits,
    SUM(t.CreditAmount) AS TotalCredits
FROM Accounts a
INNER JOIN Persons p ON a.PersonId = p.Id
INNER JOIN Statuses s ON a.StatusId = s.Id
LEFT JOIN Transactions t ON a.Id = t.AccountId
WHERE a.Id = @AccountId
GROUP BY a.Id, a.AccountNumber, a.AccountName, a.OutstandingBalance, 
         s.StatusName, p.FirstName, p.Surname;
```

---

#### 4. Get All Transactions for an Account
```sql
SELECT 
    t.Id,
    t.TransactionDate,
    t.DebitAmount,
    t.CreditAmount,
    t.Description,
    t.CaptureDate,
    a.AccountNumber,
    p.FirstName + ' ' + p.Surname AS PersonName
FROM Transactions t
INNER JOIN Accounts a ON t.AccountId = a.Id
INNER JOIN Persons p ON a.PersonId = p.Id
WHERE a.Id = @AccountId
ORDER BY t.TransactionDate DESC, t.CaptureDate DESC;
```

---

#### 5. Search Person by Surname
```sql
SELECT 
    p.Id,
    p.IdNumber,
    p.FirstName,
    p.Surname,
    p.Email,
    p.PhoneNumber,
    COUNT(a.Id) AS AccountCount
FROM Persons p
LEFT JOIN Accounts a ON p.Id = a.PersonId
WHERE p.Surname LIKE '%' + @Surname + '%'
GROUP BY p.Id, p.IdNumber, p.FirstName, p.Surname, p.Email, p.PhoneNumber
ORDER BY p.Surname, p.FirstName;
```

---

#### 6. Search by Account Number
```sql
SELECT 
    a.Id AS AccountId,
    a.AccountNumber,
    a.AccountName,
    a.OutstandingBalance,
    s.StatusName,
    p.Id AS PersonId,
    p.IdNumber,
    p.FirstName,
    p.Surname,
    p.Email,
    p.PhoneNumber
FROM Accounts a
INNER JOIN Persons p ON a.PersonId = p.Id
INNER JOIN Statuses s ON a.StatusId = s.Id
WHERE a.AccountNumber = @AccountNumber;
```

---

#### 7. Dashboard Statistics
```sql
-- Total Counts
SELECT 
    (SELECT COUNT(*) FROM Persons) AS TotalPersons,
    (SELECT COUNT(*) FROM Accounts) AS TotalAccounts,
    (SELECT COUNT(*) FROM Transactions) AS TotalTransactions,
    (SELECT COUNT(*) FROM Accounts WHERE StatusId = 1) AS OpenAccounts,
    (SELECT COUNT(*) FROM Accounts WHERE StatusId = 2) AS ClosedAccounts,
    (SELECT ISNULL(SUM(OutstandingBalance), 0) FROM Accounts) AS TotalBalance;
```

---

#### 8. Check if Person Can Be Deleted
```sql
SELECT 
    CASE 
        WHEN NOT EXISTS (
            SELECT 1 FROM Accounts 
            WHERE PersonId = @PersonId
        ) THEN 1  -- No accounts, can delete
        WHEN NOT EXISTS (
            SELECT 1 FROM Accounts a
            INNER JOIN Statuses s ON a.StatusId = s.Id
            WHERE a.PersonId = @PersonId AND s.StatusName = 'Open'
        ) THEN 1  -- All accounts closed, can delete
        ELSE 0    -- Has open accounts, cannot delete
    END AS CanDelete;
```

---

#### 9. Check if Account Can Be Closed
```sql
SELECT 
    CASE 
        WHEN a.OutstandingBalance = 0 AND s.StatusName = 'Open' 
        THEN 1  -- Balance is zero and account is open, can close
        ELSE 0  -- Cannot close
    END AS CanClose
FROM Accounts a
INNER JOIN Statuses s ON a.StatusId = s.Id
WHERE a.Id = @AccountId;
```

---

#### 10. Calculate Account Balance from Transactions
```sql
SELECT 
    a.Id,
    a.AccountNumber,
    SUM(t.CreditAmount) AS TotalCredits,
    SUM(t.DebitAmount) AS TotalDebits,
    (SUM(t.CreditAmount) - SUM(t.DebitAmount)) AS CalculatedBalance,
    a.OutstandingBalance AS StoredBalance
FROM Accounts a
LEFT JOIN Transactions t ON a.Id = t.AccountId
WHERE a.Id = @AccountId
GROUP BY a.Id, a.AccountNumber, a.OutstandingBalance;
```

---

#### 11. Recent Activity (Dashboard)
```sql
-- Recent Persons (Last 5)
SELECT TOP 5 
    Id, IdNumber, FirstName, Surname, Email, PhoneNumber
FROM Persons
ORDER BY Id DESC;

-- Recent Accounts (Last 5)
SELECT TOP 5 
    a.Id, a.AccountNumber, a.AccountName, a.OutstandingBalance,
    p.FirstName + ' ' + p.Surname AS PersonName,
    s.StatusName
FROM Accounts a
INNER JOIN Persons p ON a.PersonId = p.Id
INNER JOIN Statuses s ON a.StatusId = s.Id
ORDER BY a.Id DESC;

-- Recent Transactions (Last 5)
SELECT TOP 5 
    t.Id, t.TransactionDate, t.DebitAmount, t.CreditAmount, 
    t.Description, t.CaptureDate,
    a.AccountNumber,
    p.FirstName + ' ' + p.Surname AS PersonName
FROM Transactions t
INNER JOIN Accounts a ON t.AccountId = a.Id
INNER JOIN Persons p ON a.PersonId = p.Id
ORDER BY t.CaptureDate DESC;
```

---

#### 12. Validate Transaction Business Rules
```sql
-- Check if transaction is valid
SELECT 
    CASE 
        WHEN @TransactionDate > GETDATE() 
        THEN 'Transaction date cannot be in future'
        WHEN @DebitAmount = 0 AND @CreditAmount = 0 
        THEN 'Transaction amount cannot be zero'
        WHEN @DebitAmount > 0 AND @CreditAmount > 0 
        THEN 'Cannot have both debit and credit'
        WHEN s.StatusName = 'Closed' 
        THEN 'Cannot post to closed account'
        ELSE 'Valid'
    END AS ValidationResult
FROM Accounts a
INNER JOIN Statuses s ON a.StatusId = s.Id
WHERE a.Id = @AccountId;
```

---

## Data Integrity Rules

### Enforced by Database

✅ **Primary Keys** - Uniqueness of records  
✅ **Foreign Keys** - Referential integrity  
✅ **Unique Constraints** - ID Number, Account Number, Username  
✅ **NOT NULL** - Required fields  
✅ **Data Types** - Column type validation  
✅ **Delete Cascade/Restrict** - Relationship rules  

### Enforced by Application

✅ **Date Validation** - Transaction date not in future  
✅ **Amount Validation** - Debit/Credit logic  
✅ **Balance Calculation** - Automatic updates  
✅ **Status Rules** - Cannot close with balance  
✅ **Length Validation** - ID Number exactly 13 digits  
✅ **Format Validation** - Email, Phone formats  
✅ **Password Hashing** - BCrypt security  

---

## Backup & Maintenance

### Backup Database
```sql
-- Full Backup
BACKUP DATABASE [PersonAccountDB]
TO DISK = 'C:\Backups\PersonAccountDB_Full.bak'
WITH FORMAT, INIT, NAME = 'Full Backup of PersonAccountDB';
```

### Restore Database
```sql
-- Restore from Backup
RESTORE DATABASE [PersonAccountDB]
FROM DISK = 'C:\Backups\PersonAccountDB_Full.bak'
WITH REPLACE;
```

### Rebuild Indexes
```sql
-- Rebuild all indexes
ALTER INDEX ALL ON [dbo].[Persons] REBUILD;
ALTER INDEX ALL ON [dbo].[Accounts] REBUILD;
ALTER INDEX ALL ON [dbo].[Transactions] REBUILD;
ALTER INDEX ALL ON [dbo].[Users] REBUILD;
```

### Update Statistics
```sql
-- Update statistics for better query performance
UPDATE STATISTICS [dbo].[Persons];
UPDATE STATISTICS [dbo].[Accounts];
UPDATE STATISTICS [dbo].[Transactions];
```

---

## Performance Considerations

### Query Optimization Tips

1. **Use Indexes Wisely**
   - Indexed columns: IdNumber, AccountNumber, Username, Foreign Keys
   - Avoid over-indexing (slows down INSERT/UPDATE)

2. **Avoid N+1 Queries**
   - Use `.Include()` in Entity Framework
   - Example: `context.Persons.Include(p => p.Accounts).ThenInclude(a => a.Status)`

3. **Pagination for Large Results**
   - Use OFFSET/FETCH or Skip/Take
   - Don't load all records at once

4. **Calculated Fields**
   - Balance is stored, not calculated on-the-fly
   - Recalculated only when transactions change

---

## Migration History

### Initial Migration: `InitialCreate`

Created all 5 tables with relationships and constraints.

**Created:**
- Persons table
- Statuses table
- Accounts table
- Transactions table
- Users table

**Seeded:**
- 2 Statuses (Open, Closed)
- 1 User (admin)

### Future Migrations

When adding migrations:
```powershell
# Create migration
Add-Migration MigrationName

# Apply migration
Update-Database

# Rollback migration
Update-Database PreviousMigrationName
```

---

## Entity Framework Core Models

### C# Model Definitions

The database tables map to these C# classes:
```csharp
// Person.cs
public class Person
{
    public int Id { get; set; }
    public string IdNumber { get; set; }
    public string FirstName { get; set; }
    public string Surname { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    
    // Navigation
    public virtual ICollection<Account> Accounts { get; set; }
}

// Account.cs
public class Account
{
    public int Id { get; set; }
    public string AccountNumber { get; set; }
    public string AccountName { get; set; }
    public decimal OutstandingBalance { get; set; }
    public int PersonId { get; set; }
    public int StatusId { get; set; }
    
    // Navigation
    public virtual Person Person { get; set; }
    public virtual Status Status { get; set; }
    public virtual ICollection<Transaction> Transactions { get; set; }
}

// Transaction.cs
public class Transaction
{
    public int Id { get; set; }
    public DateTime TransactionDate { get; set; }
    public decimal DebitAmount { get; set; }
    public decimal CreditAmount { get; set; }
    public string Description { get; set; }
    public DateTime CaptureDate { get; set; }
    public int AccountId { get; set; }
    
    // Navigation
    public virtual Account Account { get; set; }
}

// Status.cs
public class Status
{
    public int Id { get; set; }
    public string StatusName { get; set; }
    
    // Navigation
    public virtual ICollection<Account> Accounts { get; set; }
}

// User.cs
public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string? FullName { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool IsActive { get; set; }
}
```

---

## Database Size Estimates

### Expected Growth

| Table | Rows/Year | Size/Year |
|-------|-----------|-----------|
| Persons | 1,000 | ~100 KB |
| Accounts | 2,000 | ~200 KB |
| Transactions | 50,000 | ~5 MB |
| Users | 10 | ~1 KB |
| Statuses | 0 (fixed) | ~1 KB |

**Total Estimated Growth:** ~5-6 MB per year (1,000 persons, 50K transactions)

---

## Appendix: SQL Server Data Types

### Mapping Reference

| C# Type | SQL Server Type | Notes |
|---------|-----------------|-------|
| int | INT | 4 bytes, -2.1B to 2.1B |
| string | NVARCHAR(n) | Unicode text, n characters |
| decimal | DECIMAL(18,2) | 18 total digits, 2 after decimal |
| DateTime | DATETIME2 | More precise than DATETIME |
| bool | BIT | 0 or 1 |

---

## Security Notes

### Sensitive Data

- **PasswordHash**: Never stored as plain text, BCrypt hashed
- **Connection Strings**: Stored in appsettings.json (not in source control in production)
- **User Sessions**: Stored in memory, expire after 30 minutes

### SQL Injection Prevention

- All queries use **parameterized queries** via Entity Framework
- No raw SQL with string concatenation
- Example: `context.Persons.Where(p => p.IdNumber == idNumber)` ✅

---

## Glossary

**Primary Key (PK):** Unique identifier for each row  
**Foreign Key (FK):** Reference to a Primary Key in another table  
**Unique Constraint:** Ensures no duplicate values in a column  
**Index:** Database structure to speed up queries  
**Cascade Delete:** Automatically delete related records  
**Restrict Delete:** Prevent deletion if related records exist  
**Migration:** Versioned database schema change  
**Seed Data:** Initial data populated when database is created  

---

**End of Database Schema Documentation**

For application usage, see [USER_GUIDE.md](USER_GUIDE.md)

For installation, see [SETUP.md](SETUP.md)

For project overview, see [README.md](../README.md)