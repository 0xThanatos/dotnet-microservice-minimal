using System.Collections.Generic;
using System.Linq;

namespace SupplierService.Model
{
    public class DataSeeder
    {
        private readonly SupplierDbContext supplierDbContext;

        public DataSeeder(SupplierDbContext supplierDbContext)
        {
            this.supplierDbContext = supplierDbContext;
        }

        public void Seed()
        {
            if(!supplierDbContext.Supplier.Any())
            {
                var suppliers = new List<Supplier>()
                {
                        new Supplier()
                        {
                            SupplierId = 1,
                            Name = "Industry 1",
                        },
                        new Supplier()
                        {
                            SupplierId = 2,
                            Name = "Industry 2",
                        },
                        new Supplier()
                        {
                            SupplierId = 3,
                            Name = "Industry 3",
                        }
                };

                supplierDbContext.Supplier.AddRange(suppliers);
                supplierDbContext.SaveChanges();
            }
        }
    }
}
