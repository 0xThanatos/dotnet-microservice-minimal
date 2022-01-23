using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using ProductService.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using ProductService.ServiceClient;
using Polly;
using System;

var builder = WebApplication.CreateBuilder(args);

//Setup enviroment variables
var connectionString = System.Environment.GetEnvironmentVariable("CONNECTION_STRINGS");

//Add Repository Pattern
builder.Services.AddScoped<IDataRepository, DataRepository>();
builder.Services.AddDbContext<ProductDbContext>(x => x.UseSqlServer(connectionString));

//Add Swagger Support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Add Seeder
builder.Services.AddTransient<DataSeeder>();

//Add Service Client
builder.Services.AddHttpClient<SupplierClient>()
    .AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(10, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))))
    .AddTransientHttpErrorPolicy(builder => builder.CircuitBreakerAsync(3, TimeSpan.FromSeconds(10)));
builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseSwaggerUI();

//Seed Data
if (args.Length == 1 && args[0].ToLower() == "-seed")
    SeedData(app);

static void SeedData(IHost app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

    using var scope = scopedFactory.CreateScope();
    var service = scope.ServiceProvider.GetService<DataSeeder>();
    service.Seed();
}

//Exception page for developement
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

//Swagger routes
app.UseSwagger(x => x.SerializeAsV2 = true);

//App routes
app.MapGet("/product/{id}", ([FromServices] IDataRepository db, System.Int64 id) =>
{
    return db.GetProductById(id);
});

app.MapGet("/products", ([FromServices] IDataRepository db) =>
    {
        return db.GetProducts();
    }
);

app.MapPut("/product/{id}", ([FromServices] IDataRepository db, Product product) =>
{
    return db.PutProduct(product);
});

app.MapPost("/product", ([FromServices] IDataRepository db, Product product) =>
{
    return db.AddProduct(product);
});

app.MapDelete("/product/{id}", ([FromServices] IDataRepository db, System.Int64 id) =>
{
    return db.DeleteProductById(id);
});

//Health check route
app.MapHealthChecks("healthz");

app.Run();
