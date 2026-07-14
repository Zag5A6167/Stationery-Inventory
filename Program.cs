using Microsoft.EntityFrameworkCore;
using StationeryAPI;
using StationeryAPI.Models;
using Scalar.AspNetCore;
using StationeryAPI.Endpoints;

var builder = WebApplication.CreateBuilder(args);


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

app.MapProductEndpoints();

app.Run();