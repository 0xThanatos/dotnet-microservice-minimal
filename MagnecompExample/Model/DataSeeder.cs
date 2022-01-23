using ProductService.ServiceClient;
using System.Collections.Generic;
using System.Linq;

namespace ProductService.Model
{
    public class DataSeeder
    {
        private readonly ProductDbContext productDbContext;

        public DataSeeder(ProductDbContext productDbContext)
        {
            this.productDbContext = productDbContext;
        }

        public void Seed()
        {
            if(!productDbContext.Product.Any())
            {
                var products = new List<Product>()
                {
                        new Product()
                        {
                            ProductId = 1,
                            Name = "Apple",
                            Price = 100,
                            SupplierId = 1,
                        },
                        new Product()
                        {
                            ProductId = 2,
                            Name = "Banana",
                            Price = 200,
                            SupplierId = 2,
                        },
                        new Product()
                        {
                            ProductId = 3,
                            Name = "Carrot",
                            Price = 300,
                            SupplierId = 3,
                        }
                };

                productDbContext.Product.AddRange(products);
                productDbContext.SaveChanges();
            }
        }
    }
}
