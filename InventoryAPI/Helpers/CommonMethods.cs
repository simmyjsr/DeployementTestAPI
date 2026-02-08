namespace InventoryAPI.Helpers
{
    public class CommonMethods
    {
        public static class ProductNumberGenerator
        {
            public static string Generate()
            {
                return $"PRD-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString().Substring(0, 8)}";
            }
        }
    }
}
