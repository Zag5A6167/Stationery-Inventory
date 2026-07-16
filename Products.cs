using System.ComponentModel.DataAnnotations;
namespace StationeryAPI.Models;


public class Product
{
    [Key]
    public int Id { get; set; }
    [Required(ErrorMessage = "Product name is required")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters")]
    public string Name { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    [Range(0, 10000, ErrorMessage = "Stock must be between 0 and 10,000")]
    public int Stock { get; set; }
    [Range(0.01, 1000000, ErrorMessage = "Price must be greater than 0")]
 
    public decimal Price { get; set; }
}