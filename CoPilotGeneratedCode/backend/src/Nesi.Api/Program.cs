using Microsoft.EntityFrameworkCore;
using Nesi.Application;
using Nesi.Application.Services;
using Nesi.Domain.Interfaces;
using Nesi.Infrastructure.Data;
using Nesi.Infrastructure.Extensions;
using Nesi.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Add DbContext
builder.Services.AddDbContext<NesiDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Register IApplicationDbContext
builder.Services.AddScoped<Nesi.Application.Common.IApplicationDbContext>(provider => 
    provider.GetRequiredService<NesiDbContext>());

// Register Application layer services (MediatR, AutoMapper, FluentValidation)
builder.Services.AddApplication();

// Register Infrastructure services
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Register repositories
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IQuoteRepository, QuoteRepository>();
builder.Services.AddScoped<IWorkOrderRepository, WorkOrderRepository>();
builder.Services.AddScoped<IVendorRepository, VendorRepository>();
builder.Services.AddScoped<IPurchaseOrderRepository, PurchaseOrderRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Register services
var storagePath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
var baseUrl = builder.Configuration["FileStorage:BaseUrl"] ?? "http://localhost:5000/uploads";
builder.Services.AddSingleton<IFileStorageService>(new LocalFileStorageService(storagePath, baseUrl));

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Migrate and seed database
await app.MigrateDatabaseAsync();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
