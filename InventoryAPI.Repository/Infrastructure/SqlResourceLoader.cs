using System.Reflection;

namespace InventoryAPI.Infrastructure
{
    internal static class SqlResourceLoader
    {
        public static string GetSql(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            // Resource names use '.' as separators; embedded path: InventoryAPI.Sql.FileName.sql
            var fullName = assembly.GetManifestResourceNames()
                .FirstOrDefault(n => n.EndsWith(resourceName, StringComparison.OrdinalIgnoreCase));

            if (fullName == null)
                throw new InvalidOperationException($"SQL resource not found: {resourceName}");

            using var stream = assembly.GetManifestResourceStream(fullName)!;
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }
}
