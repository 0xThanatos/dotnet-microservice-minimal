using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupplierService.Model
{
    public class DataRepository : IDataRepository
    {
        private readonly SupplierDbContext db;

        public DataRepository(SupplierDbContext db)
        {
            this.db = db;
        }

        public List<Supplier> GetSuppliers() => db.Supplier.ToList();

        public Supplier PutSupplier(Supplier supplier)
        {
            db.Supplier.Update(supplier);
            db.SaveChanges();
            return db.Supplier.Where(x => x.SupplierId == supplier.SupplierId).FirstOrDefault();
        }

        public List<Supplier> AddSupplier(Supplier supplier)
        {
            db.Supplier.Add(supplier);
            db.SaveChanges();
            return db.Supplier.ToList();
        }

        public Supplier GetSupplierById(System.Int64 Id)
        {
            return db.Supplier.Where(x => x.SupplierId == Id).FirstOrDefault();
        }

        public bool DeleteSupplierById(System.Int64 Id)
        {
            var supplier = db.Supplier.Where(x => x.SupplierId == Id).FirstOrDefault();
            if (supplier == null) return false;

            db.Supplier.Remove(supplier);
            db.SaveChanges();
            return true;
        }

    }
}
