using ProductService.ServiceClient;

namespace ProductService.Model
{
    public class Product
    {
        public System.Int64 ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public System.Int64 SupplierId { get; set; }
        public Supplier Supplier { get; set; }
    }

}
