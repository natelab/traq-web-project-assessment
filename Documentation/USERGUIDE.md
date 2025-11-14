# User Guide

Complete guide to using the Person Account Management System.

---

## Table of Contents

- [Getting Started](#getting-started)
- [Dashboard Overview](#dashboard-overview)
- [Person Management](#person-management)
- [Account Management](#account-management)
- [Transaction Management](#transaction-management)
- [Search Functionality](#search-functionality)
- [User Account](#user-account)
- [Common Workflows](#common-workflows)
- [Tips & Best Practices](#tips--best-practices)

---

## Getting Started

### Logging In

1. Navigate to the application URL 
2. Click **Login** in the navigation bar
3. Enter your credentials:
   - **Default Admin**: `admin` / `Admin123`
4. Click **Login**

Upon successful login, you'll be redirected to the Dashboard.

### Navigation

The main navigation bar contains:
- **Home** - Dashboard with statistics
- **Persons** - Manage persons and search
- **About** - Information about the system
- **Contact** - Contact information
- **User Menu** (top-right) - Logout option

---

## Dashboard Overview

The Dashboard provides a quick overview of system data.

### Statistics Cards

Four main statistics are displayed:

1. **Total Persons** - Count of all registered persons
2. **Total Accounts** - Count of all accounts (open and closed)
3. **Total Transactions** - Count of all recorded transactions
4. **Total Balance** - Sum of all account balances

### Account Status

Shows breakdown of:
- **Open Accounts** - Accounts available for transactions
- **Closed Accounts** - Inactive accounts

### Recent Activity

Three panels showing:
- **Recent Persons** - Last 5 persons created
- **Recent Accounts** - Last 5 accounts created
- **Recent Transactions** - Last 5 transactions recorded

### Quick Actions

- **Add New Person** - Direct link to create person form
- **Search Records** - Jump to search functionality

---

## Person Management

### Viewing All Persons

1. Click **Persons** in the navigation
2. View list of all registered persons
3. Table displays:
   - ID Number
   - Full Name
   - Email
   - Phone Number
   - Number of Accounts
   - Action buttons

### Searching for Persons

On the Persons page, use the search bar at the top:

1. **Select Search Type:**
   - **Surname** - Search by last name (partial match)
   - **ID Number** - Search by exact 13-digit ID
   - **Account Number** - Find person by their account number

2. **Enter Search Term** in the text field

3. **Click Search** button

4. **View Results:**
   - Persons matching criteria appear in the table
   - If searching by Account Number, account details card is shown
   - Click **Clear Search** to return to full list

### Creating a New Person

1. Click **Create New Person** button (green button)
2. Fill in the form:

   **Required Fields:**
   - **First Name** (max 100 characters)
   - **Surname** (max 100 characters)
   - **ID Number** (exactly 13 digits, must be unique)

   **Optional Fields:**
   - **Email** (must be valid email format)
   - **Phone Number** (valid phone format)
   - **Address** (max 200 characters)

3. Click **Create Person**

**Validation Rules:**
- ✅ ID Number must be exactly 13 characters
- ✅ ID Number must be unique (no duplicates)
- ✅ First Name and Surname are required
- ✅ Email must be valid format if provided
- ✅ Phone must be valid format if provided

**Success:** You'll be redirected to the Person Details page.

### Viewing Person Details

1. From Persons list, click **Eye icon** (View) button
2. Person Details page shows:
   - Personal information
   - Contact details
   - List of all accounts
   - Account statistics

**Available Actions:**
- **Edit Person** - Modify person information
- **Delete Person** - Remove person (only if allowed)
- **Add Account** - Create new account for this person

### Editing a Person

1. From Person Details, click **Edit Person** button
2. Modify the information (same fields as Create)
3. Click **Save Changes**

**Note:** ID Number can be changed but must remain unique.

### Deleting a Person

1. From Person Details, click **Delete Person** button
2. Review the person's information
3. Click **Confirm Delete**

**Important Rules:**
- ❌ Cannot delete person with open accounts
- ✅ Can delete if no accounts exist
- ✅ Can delete if ALL accounts are closed

If deletion fails, you'll see an error message explaining why.

---

## Account Management

### Viewing Account Details

1. Navigate to a Person's Details page
2. Click on an account from the list
   - Or click **Eye icon** next to an account

Account Details page displays:
- Account number and name
- Account holder information
- Current status (Open/Closed)
- Outstanding balance
- Transaction summary (total debits/credits)
- List of all transactions

### Creating a New Account

Accounts can **only** be created for existing persons.

1. Go to Person Details page
2. Click **Add Account** button
3. Fill in the form:
   - **Account Number** (max 20 characters, must be unique)
   - **Account Name** (max 100 characters, descriptive name)

4. Click **Create Account**

**Automatic Values:**
- Initial Balance: R 0.00
- Status: Open

**Validation Rules:**
- ✅ Account Number must be unique across all accounts
- ✅ Account can only be created after person exists
- ✅ All fields are required

### Editing an Account

1. From Account Details, click **Edit Account** button
2. Modify:
   - Account Number (must remain unique)
   - Account Name

**Note:** 
- ❌ Cannot manually change Outstanding Balance
- ❌ Cannot manually change Status
- ✅ Balance updates automatically via transactions
- ✅ Status changes via "Close Account" action

### Closing an Account

1. From Account Details page
2. Click **Close Account** button
3. Confirm the action

**Requirements:**
- ✅ Balance must be exactly R 0.00
- ❌ Cannot close account with outstanding balance

**Effect:**
- Status changes to "Closed"
- No new transactions can be added
- Account remains visible in system

**Tip:** To close an account with a balance:
1. Add offsetting transactions to bring balance to zero
2. Then close the account

---

## Transaction Management

### Viewing Transactions

Transactions are displayed on the Account Details page in a table showing:
- Transaction Date
- Description
- Debit Amount (money out)
- Credit Amount (money in)
- Capture Date (when transaction was recorded)
- Action buttons

### Creating a New Transaction

Transactions can **only** be added to existing accounts.

1. From Account Details page
2. Ensure account status is **Open**
3. Click **Add Transaction** button
4. Fill in the form:

   **Required Fields:**
   - **Transaction Date** (cannot be in future)
   - **Description** (max 500 characters)
   - **Amount** - Enter in EITHER:
     - **Debit Amount** (money going out) OR
     - **Credit Amount** (money coming in)

5. Click **Create Transaction**

**Validation Rules:**
- ✅ Transaction date cannot be in the future
- ✅ Must enter either Debit OR Credit (not both)
- ✅ Amount cannot be zero
- ❌ Cannot add transaction to closed account
- ✅ Description is required

**Automatic Actions:**
- Capture Date is set to current date/time
- Account balance updates automatically:
  - **Credits** increase balance
  - **Debits** decrease balance

### Transaction Types

**Credit Transaction (Money In):**
- Deposits
- Payments received
- Interest earned
- Refunds received
- **Effect:** Increases account balance

**Debit Transaction (Money Out):**
- Withdrawals
- Payments made
- Fees charged
- Purchases
- **Effect:** Decreases account balance

### Editing a Transaction

1. From Account Details, click **Edit icon** (pencil) next to transaction
2. Modify:
   - Transaction Date (still cannot be in future)
   - Debit/Credit Amount
   - Description

3. Click **Save Changes**

**Important:**
- ❌ Cannot change Capture Date (updates automatically to current time)
- ✅ Account balance recalculates automatically
- ✅ Can change amount type (debit ↔ credit)

### Deleting a Transaction

1. From Account Details, click **Delete icon** (trash) next to transaction
2. Review transaction details
3. Click **Confirm Delete**

**Effect:**
- Transaction is permanently removed
- Account balance recalculates automatically
- Cannot be undone

---

## Search Functionality

The integrated search on the Persons page supports three search types:

### Search by Surname

**Use Case:** Find persons by their last name

**How:**
1. Select **"Surname"** from dropdown
2. Enter partial or full surname
3. Click **Search**

**Results:** All persons with matching surnames (partial match)

**Example:** Searching "Smi" will find "Smith", "Smit", "Smithson"

---

### Search by ID Number

**Use Case:** Find a specific person by exact ID

**How:**
1. Select **"ID Number"** from dropdown
2. Enter the 13-digit ID number
3. Click **Search**

**Results:** Single person with exact ID match (if exists)

**Note:** Must be exact match, no partial matching

---

### Search by Account Number

**Use Case:** Find which person owns a specific account

**How:**
1. Select **"Account Number"** from dropdown
2. Enter the account number
3. Click **Search**

**Results:** 
- Account details card showing account information
- Person who owns the account appears in table
- Quick links to view account or person details

---

### Clearing Search

Click **Clear Search** button to return to viewing all persons.

---

## User Account

### Logging Out

1. Click your **username** in top-right corner
2. Click **Logout** from dropdown menu
3. You'll be redirected to the login page

### Session Management

- Sessions expire after **30 minutes** of inactivity
- You'll be automatically logged out
- Any unsaved work will be lost

**Tip:** Save your work frequently!

---

## Common Workflows

### Workflow 1: Adding a New Customer with Account

1. **Create Person**
   - Click Persons → Create New Person
   - Enter ID Number, Name, Contact Info
   - Click Create Person

2. **Create Account**
   - From Person Details, click Add Account
   - Enter Account Number and Name
   - Click Create Account

3. **Add Initial Deposit**
   - From Account Details, click Add Transaction
   - Enter Credit Amount
   - Description: "Initial deposit"
   - Click Create Transaction

✅ **Result:** New customer with funded account ready for transactions

---

### Workflow 2: Processing a Customer Payment

1. **Find Customer**
   - Go to Persons page
   - Search by Surname or ID Number

2. **Select Account**
   - View Person Details
   - Click on the relevant account

3. **Record Payment**
   - Click Add Transaction
   - Enter Credit Amount
   - Description: "Payment received - Invoice #123"
   - Click Create Transaction

✅ **Result:** Payment recorded and balance updated

---

### Workflow 3: Closing an Account

1. **Navigate to Account**
   - Find person and their account

2. **Zero the Balance**
   - If balance is positive: Add debit transaction
   - If balance is negative: Add credit transaction
   - Bring balance to exactly R 0.00

3. **Close Account**
   - Click Close Account button
   - Confirm action

✅ **Result:** Account status changed to Closed

---

### Workflow 4: Finding Transaction History

1. **Locate Account**
   - Search for person
   - View Person Details
   - Click on account

2. **Review Transactions**
   - Scroll to Transactions table
   - View all debits and credits
   - Check dates and descriptions

3. **Export or Note Information**
   - Transaction table shows complete history
   - Can edit or delete if needed

✅ **Result:** Complete transaction history visible

---

## Tips & Best Practices

### Data Entry Tips

✅ **DO:**
- Use descriptive account names (e.g., "Savings Account", "Business Account")
- Write clear transaction descriptions
- Verify ID numbers before submission
- Keep contact information up to date
- Review balance after each transaction

❌ **DON'T:**
- Use duplicate ID numbers
- Leave required fields empty
- Enter future dates for transactions
- Try to close accounts with balances
- Delete persons with open accounts

---

### Transaction Best Practices

**Be Specific in Descriptions:**
- ❌ "Payment"
- ✅ "Payment received - Invoice #12345"

**Use Consistent Naming:**
- ❌ "Dep", "deposit", "cash in"
- ✅ "Deposit" (standardized)

**Record Immediately:**
- Don't wait to record transactions
- Ensure accurate balances

---

### Search Tips

**For Quick Results:**
- Use Account Number search when you know it
- Use ID Number for exact person lookup
- Use Surname for browsing similar names

**Remember:**
- Surname search is case-insensitive
- ID Number must be exact
- Account Number must be exact

---

### Troubleshooting Common Issues

**Problem:** "ID Number already exists"
- **Solution:** This person is already in the system. Search for them instead.

**Problem:** "Cannot delete person"
- **Solution:** Close or delete all their accounts first.

**Problem:** "Cannot close account"
- **Solution:** Bring the balance to exactly R 0.00 first.

**Problem:** "Transaction date cannot be in future"
- **Solution:** Use today's date or earlier.

**Problem:** "Transaction amount cannot be zero"
- **Solution:** Enter a value greater than 0.

**Problem:** "Cannot add transaction to closed account"
- **Solution:** Account is closed. Reopen it or use a different account.

---

### Keyboard Shortcuts

While most actions require clicking buttons, here are some useful shortcuts:

- **Tab** - Move between form fields
- **Enter** - Submit form (when button is focused)
- **Esc** - Cancel/Go back (on some browsers)
- **Ctrl + F** - Find on page (browser function)

---

## Data Safety

### Automatic Safeguards

The system includes several safeguards:

- ✅ Cannot delete data with dependencies
- ✅ Balances update automatically
- ✅ Cannot manually edit balances
- ✅ Capture dates are system-set
- ✅ Validation prevents invalid data

### Manual Cautions

- ⚠️ Deleted transactions cannot be recovered
- ⚠️ Deleted persons cannot be recovered
- ⚠️ Session timeout may lose unsaved work

**Recommendation:** Double-check before deleting anything.

---

## Getting Help

If you need assistance:

1. **Check this User Guide** for step-by-step instructions
2. **Review error messages** - they usually explain what's wrong
3. **Try the About page** for system information
4. **Contact support** via the Contact page

---

## Appendix: Field Specifications

### Person Fields

| Field | Type | Required | Max Length | Validation |
|-------|------|----------|------------|------------|
| ID Number | Text | Yes | 13 | Exactly 13 digits, unique |
| First Name | Text | Yes | 100 | Letters and spaces |
| Surname | Text | Yes | 100 | Letters and spaces |
| Email | Email | No | 100 | Valid email format |
| Phone Number | Text | No | 20 | Valid phone format |
| Address | Text | No | 200 | Any text |

### Account Fields

| Field | Type | Required | Max Length | Validation |
|-------|------|----------|------------|------------|
| Account Number | Text | Yes | 20 | Unique |
| Account Name | Text | Yes | 100 | Any text |
| Outstanding Balance | Decimal | Auto | - | Calculated automatically |
| Status | Select | Auto | - | Open or Closed |

### Transaction Fields

| Field | Type | Required | Max Length | Validation |
|-------|------|----------|------------|------------|
| Transaction Date | Date | Yes | - | Not in future |
| Debit Amount | Decimal | Either | - | > 0, not with Credit |
| Credit Amount | Decimal | Either | - | > 0, not with Debit |
| Description | Text | Yes | 500 | Any text |
| Capture Date | DateTime | Auto | - | Set automatically |

---

**End of User Guide**

For technical details, see [DATABASE_SCHEMA.md](DATABASE_SCHEMA.md)

For installation help, see [SETUP.md](SETUP.md)