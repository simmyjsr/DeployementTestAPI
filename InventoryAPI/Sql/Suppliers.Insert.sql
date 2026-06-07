INSERT INTO Suppliers (SupplierName, ContactPerson, Email, Phone, Address, CreatedAt, Status)
VALUES (@SupplierName, @ContactPerson, @Email, @Phone, @Address, @CreatedAt, @Status);
SELECT CAST(SCOPE_IDENTITY() as int);