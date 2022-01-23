using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductService.Model
{
    public interface IDataRepository
    {
        List<Product> AddProduct(Product product);
        List<Product> GetProducts();
        Product PutProduct(Product product);
        Task<Product> GetProductById(System.Int64 id);
        bool DeleteProductById(System.Int64 id);
    }
}