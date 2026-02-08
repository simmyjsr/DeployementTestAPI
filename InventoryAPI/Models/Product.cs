namespace InventoryAPI.Models
{
    public class Product
    {
        public Guid ProductID { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public int CategoryID
        {
            get; set;
        }
        public string? ProductNumber { get; set; }
        public string? Image { get; set; }
        public byte[] ImageBinary { get; set; }

        public bool Feature { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public int SupplierID { get; set; }

        // Not mapped to database directly
        public string ImageBase64 => ImageBinary != null ? $"data:image/jpeg;base64,{Convert.ToBase64String(ImageBinary)}" : null;
    

}
    public class CreateProductRequest 
    {
       
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public int CategoryID { get; set; }
        public IFormFile? Image { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public bool Feature { get; set; } = false;
        public int SupplierID { get; set; }
    }
}
