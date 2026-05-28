namespace InventoryAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; } // Store hashed in real apps!
        public string Role { get; set; }

    }
}
