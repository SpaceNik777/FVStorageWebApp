using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FVStorage.Models;

public class SupplierDto {
    
    [Required]
    public string Code;

    [Required]
    [DisplayName("Name")]
    public string Name { get; set; }
}