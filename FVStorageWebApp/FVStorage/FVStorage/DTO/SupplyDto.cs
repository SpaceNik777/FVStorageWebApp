using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FVStorage.Models;

public class SupplyDto {

    [HiddenInput(DisplayValue = false)]
    public string ProductCode { get; set; }
    
    [Required]
    [Range(0, 1000000)]
    public string Id;

    [Required]
    public DateTime Date { get; set; }

    [Required]
    [DisplayName("Amount")]
    public int Amount { get; set; }
}