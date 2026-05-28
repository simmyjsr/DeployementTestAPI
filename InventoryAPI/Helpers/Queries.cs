namespace InventoryAPI.Helpers
{
    public static class Queries
    {
        public const string GetFilteredProducts = @"
           SELECT 
            p.ProductID,
            p.ProductNumber,
            p.ProductName,
            p.StockQuantity,
            c.CategoryName
           
        FROM Products p
        JOIN Categories c ON c.CategoryID = p.CategoryID
        WHERE 
            (@searchText IS NULL OR p.ProductNumber LIKE '%' + @searchText + '%') or
            (@searchText IS NULL OR p.ProductName LIKE '%' + @searchText + '%') or
         
            (@searchText IS NULL OR c.CategoryName LIKE '%' + @searchText + '%');
    ";
        public const string UpdateProducts = @"UPDATE Products
SET 
    ProductName = @ProductName,
    Description = @Description,
    Price = @Price,
    StockQuantity = @StockQuantity,
    CategoryID = @CategoryID,
    SupplierID = @SupplierID,
    CreatedAt = @CreatedAt,
    Feature = @Feature,
    ProductNumber = @ProductNumber,
    ImageBinary = @ImageBinary,
CreatedBy=@CreatedBy
WHERE 
    ProductID = @ProductID;";
        
    }
}

