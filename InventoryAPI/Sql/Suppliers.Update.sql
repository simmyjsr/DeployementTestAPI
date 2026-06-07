UPDATE Suppliers
SET SupplierName = @SupplierName,
    ContactPerson = @ContactPerson,
    Email = @Email,
    Phone = @Phone,
    Address = @Address,
    Status = @Status
WHERE SupplierID = @SupplierID