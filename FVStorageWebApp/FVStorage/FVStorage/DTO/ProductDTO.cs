using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace FVStorage.Models;

public class ProductDTO
{
    [HiddenInput(DisplayValue = false)]
    public string SupplierCode { get; set; }
    
    [Required]
    public string Code;

    [Required]
    public string Name { get; set; }

    [Required]
    [DisplayName("Weight (kg)")]
    public string Weight { get; set; }
    [Required]
    [DisplayName("Price for 1 product")]
    [Range(0, 100)]
    public string Price { get; set; }
}