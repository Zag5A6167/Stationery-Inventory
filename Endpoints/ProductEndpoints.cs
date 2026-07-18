namespace StationeryAPI.Endpoints;
using Microsoft.EntityFrameworkCore;
using StationeryAPI.Models;

public static class ProductEndpoints
{
    public static void MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        
app.MapGet("/products", async (AppDbContext db) =>
    await db.Products.ToListAsync());



app.MapPost("/products", async (AppDbContext db, HttpRequest request) =>
{
    var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
Console.WriteLine(">>> โฟลเดอร์ที่บันทึกคือ: " + folderPath);
    var form = await request.ReadFormAsync();
    var file = form.Files.GetFile("image"); 
    
    var product = new Product
    {
        Name = form["name"].ToString(),
        Stock = int.Parse(form["stock"]!),
        Price = decimal.Parse(form["price"]!)
    };

    if (file != null)
    {
       
        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
        
        using (var stream = new FileStream(path, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }
        product.ImageUrl = fileName; 
    }

    db.Products.Add(product);
    await db.SaveChangesAsync();
    return Results.Created($"/products/{product.Id}", product);
})
.DisableAntiforgery();

// app.MapPost("/products", async (AppDbContext db, Product product) =>
// {
//     db.Products.Add(product);
//     await db.SaveChangesAsync();
//     return Results.Created($"/products/{product.Id}", product);
// });


app.MapPut("/products/{id}", async (int id, Product inputProduct, AppDbContext db) =>
{
    var product = await db.Products.FindAsync(id);
    if (product is null) return Results.NotFound();

    product.Name = inputProduct.Name;
    product.Stock = inputProduct.Stock;
    product.Price = inputProduct.Price;

    await db.SaveChangesAsync();
    return Results.NoContent();
});


app.MapDelete("/products/{id}", async (int id, AppDbContext db) =>
{
    var product = await db.Products.FindAsync(id);
    if (product is null) return Results.NotFound();

    if (!string.IsNullOrEmpty(product.ImageUrl))
    {
        var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", product.ImageUrl);
        Console.WriteLine(">>> เข้ามาที่ฟังก์ชัน Delete แล้ว! id คือ: " + id); // เพิ่มบรรทัดนี้เพื่อ Test
        if (File.Exists(imagePath))
        {
            File.Delete(imagePath);
            Console.WriteLine(">>> deleted file !");
        }
    }

    db.Products.Remove(product);
    await db.SaveChangesAsync();
    return Results.NoContent();
});
    }
}