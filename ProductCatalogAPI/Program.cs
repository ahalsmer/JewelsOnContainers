using Microsoft.EntityFrameworkCore;
using ProductCatalogAPI.Data;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<CatalogContext>(
    options => options.UseSqlServer(configuration["ConnectionString"])
    );

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
// Ask for access of the scope to everything the microservice is using.
var scope = app.Services.CreateScope();
// Ask for access to all of the service providers within that scope.
var serviceProviders = scope.ServiceProvider;
// The main provider you are concerned with is the CatalogContext, so require that one in particular
var context = serviceProviders.GetRequiredService<CatalogContext>();
// Now, call the CatalogSeed method to populate the tables before running the application.
CatalogSeed.Seed(context);


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
