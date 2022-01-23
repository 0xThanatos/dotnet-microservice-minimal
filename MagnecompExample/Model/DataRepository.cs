using ProductService.ServiceClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Model
{
    public class DataRepository : IDataRepository
    {
        private readonly ProductDbContext db;
        private readonly SupplierClient supplierClient;

        public DataRepository(ProductDbContext db, SupplierClient supplierClient)
        {
            this.db = db;
            this.supplierClient = supplierClient;
        }

        public List<Product> GetProducts() => db.Product.ToList();

        public Product PutProduct(Product product)
        {
            db.Product.Update(product);
            db.SaveChanges();
            return db.Product.Where(x => x.ProductId == product.ProductId).FirstOrDefault();
        }

        public List<Product> AddProduct(Product product)
        {
            db.Product.Add(product);
            db.SaveChanges();
            return db.Product.ToList();
        }

        public async Task<Product> GetProductById(System.Int64 Id)
        {
            var product = db.Product.Where(x => x.ProductId == Id).FirstOrDefault();
            var supplier = await supplierClient.GetSupplierAsync(product.SupplierId);

            return new Product
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Price = product.Price,
                SupplierId = supplier.SupplierId,
                Supplier = new Supplier
                {
                    SupplierId = supplier.SupplierId,
                    Name = supplier.Name,
                },
            };
        }

        public bool DeleteProductById(System.Int64 Id)
        {
            var product = db.Product.Where(x => x.ProductId == Id).FirstOrDefault();
            if (product == null) return false;

            db.Product.Remove(product);
            db.SaveChanges();
            return true;
        }

    }
}
