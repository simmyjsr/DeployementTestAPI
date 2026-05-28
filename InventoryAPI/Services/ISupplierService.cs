using InventoryAPI.Models;

namespace InventoryAPI.Services
{
    public interface ISupplierService
    {
        Task< IEnumerable<Supplier>> GetAllSupplier();
        Task <Supplier> GetSupplierById(int id);
        Task <int> AddSupplier(Supplier supplier);
        Task <bool> UpdateSupplier(Supplier supplier);
        Task <bool> DeleteSupplier(int id);
        
    }
}
