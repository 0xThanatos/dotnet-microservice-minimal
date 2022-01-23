using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using SupplierService.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Polly;
using System;

var builder = WebApplication.CreateBuilder(args);

//Setup enviroment variables
var connectionString = System.Environment.GetEnvironmentVariable("CONNECTION_STRINGS");

//Add Repository Pattern
builder.Services.AddScoped<IDataRepository, DataRepository>();
builder.Services.AddDbContext<SupplierDbContext>(x => x.UseSqlServer(connectionString));

//Add Swagger Support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Add Seeder
builder.Services.AddTransient<DataSeeder>();

//Add Service Client
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
app.MapGet("/supplier/{id}", ([FromServices] IDataRepository db, System.Int64 id) =>
{
    return db.GetSupplierById(id);
});

app.MapGet("/suppliers", ([FromServices] IDataRepository db) =>
{
    return db.GetSuppliers();
}
);

app.MapPut("/supplier/{id}", ([FromServices] IDataRepository db, Supplier supplier) =>
{
    return db.PutSupplier(supplier);
});

app.MapPost("/supplier", ([FromServices] IDataRepository db, Supplier supplier) =>
{
    return db.AddSupplier(supplier);
});

app.MapDelete("/supplier/{id}", ([FromServices] IDataRepository db, System.Int64 id) =>
{
    return db.DeleteSupplierById(id);
});

//Health check route
app.MapHealthChecks("healthz");

app.Run();
