using System.ComponentModel.DataAnnotations;

namespace InventoryAPI.Models
{
    public class Brand
    {
        [Key]
        public int BrandID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public bool Status { get; set; }

        public DateTime CreatedDateTime { get; set; }
    }
}
