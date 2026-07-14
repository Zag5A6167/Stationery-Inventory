using Microsoft.EntityFrameworkCore;
using StationeryAPI;
using StationeryAPI.Models;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// 1. เชื่อมต่อฐานข้อมูล SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=stationery.db"));

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();


app.MapGet("/products", async (AppDbContext db) =>
    await db.Products.ToListAsync());

app.MapPost("/products", async (AppDbContext db, Product product) =>
{
    db.Products.Add(product);
    await db.SaveChangesAsync();
    return Results.Created($"/products/{product.Id}", product);
});

app.Run();