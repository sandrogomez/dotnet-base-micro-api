using Base.Application.Interfaces;
using Base.Infrastructure.Services;
using Base.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSingleton<IProductService, InMemoryProductService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/products", (IProductService service) =>
{
    return Results.Ok(service.GetAll());
});

app.MapGet("/products/{id}", (IProductService service, Guid id) =>
{
    var product = service.GetById(id);
    return product == null ? Results.NotFound() : Results.Ok(product);
});

app.MapPost("/products", (IProductService service, Product product) =>
{
    service.Create(product);
    return Results.Created($"/products/{product.Id}", product);
});

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
