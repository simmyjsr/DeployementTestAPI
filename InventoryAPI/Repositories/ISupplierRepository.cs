using InventoryAPI.Models;

namespace InventoryAPI.Repositories
{
    public interface ISupplierRepository
    {
        Task <IEnumerable<Supplier>> GetAllSupplier();
        Task <Supplier ?> GetSupplierById(int id);
        Task <int> AddSupplier(Supplier supplier);
        Task <bool> UpdateSupplier(Supplier supplier);
        Task <bool> DeleteSupplier(int id);
    }
}
