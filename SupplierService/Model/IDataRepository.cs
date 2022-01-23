using System.Collections.Generic;
using System.Threading.Tasks;

namespace SupplierService.Model
{
    public interface IDataRepository
    {
        List<Supplier> AddSupplier(Supplier supplier);
        List<Supplier> GetSuppliers();
        Supplier PutSupplier(Supplier supplier);
        Supplier GetSupplierById(System.Int64 id);
        bool DeleteSupplierById(System.Int64 id);
    }
}