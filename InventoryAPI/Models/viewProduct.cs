namespace InventoryAPI.Models
{
    public class viewProduct
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
        public IFormFile? Image { get; set; }
        public byte[]? ImageBytes { get; set; }

        public bool Feature { get; set; } = false;
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public int SupplierID { get; set; }
    }
}

