using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ProductService.ServiceClient
{
    public class SupplierClient
    {
        private readonly HttpClient httpClient;

        public SupplierClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public record Supplier(System.Int64 SupplierId, string Name);

        public async Task<Supplier> GetSupplierAsync(System.Int64 SupplierId)
        {
            return await httpClient.GetFromJsonAsync<Supplier>(
                $"{System.Environment.GetEnvironmentVariable("KONG_URL")}/supplier-api/supplier/{SupplierId}"
            );
        }
    }
}
