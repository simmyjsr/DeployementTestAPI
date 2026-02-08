using InventoryAPI.Models;
using InventoryAPI.Repositories;

namespace InventoryAPI.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _repository;

        public SupplierService(ISupplierRepository repository)
        {
            _repository = repository;
        }
        public async Task<int> AddSupplier(Supplier supplier)
        {
            return await _repository.AddSupplier(supplier);
        }

        public async Task<bool> DeleteSupplier(int id)
        {
            return await _repository.DeleteSupplier(id);
        }

        public async Task< IEnumerable<Supplier>> GetAllSupplier() =>  await _repository.GetAllSupplier();   

        public async Task<Supplier> GetSupplierById(int id)
        {
            return await _repository.GetSupplierById(id);
        }

        public async Task<bool> UpdateSupplier(Supplier supplier)
        {
            return await _repository.UpdateSupplier(supplier);
        }
    }
}
