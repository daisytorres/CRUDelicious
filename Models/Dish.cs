#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
namespace CRUDelicious.Models;

public class Dish
{
    [Key]
    public int DishId {get;set;}

    [Required]
    public string Chef {get;set;}
    [Required]
    public string DishName {get;set;}
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Calories must be greater than 0.")]
    public int Calories {get;set;}
    [Required]
    public int  Tastiness {get;set;}
    [Required]
    public string Description {get;set;}
    
    public DateTime CreatedAt {get;set;} = DateTime.Now;
    public DateTime UpdatedAt {get;set;} = DateTime.Now;
}
