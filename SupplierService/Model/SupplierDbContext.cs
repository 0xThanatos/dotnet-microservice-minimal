using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SupplierService.Model
{
    public class SupplierDbContext : DbContext
    {
        public SupplierDbContext()
        {

        }

        public SupplierDbContext(DbContextOptions<SupplierDbContext> options) : base(options)
        {
        }

        public DbSet<Supplier> Supplier { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = System.Environment.GetEnvironmentVariable("CONNECTION_STRINGS");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
