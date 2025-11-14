
UPDATE Accounts
SET OutstandingBalance = (
    SELECT ISNULL(SUM(t.CreditAmount), 0) - ISNULL(SUM(t.DebitAmount), 0)
    FROM Transactions t
    WHERE t.AccountId = Accounts.Id
);