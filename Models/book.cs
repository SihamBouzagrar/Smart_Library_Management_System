
using System.ComponentModel.DataAnnotations;

namespace Smart_Library_Management_System.Models;

public class Book
{
    public int BookID { get; set; }
    [Required(ErrorMessage = "veuillez saisir le titre ")]
    [StringLength(250)]
    public String? Title { get; set; }
    [Required(ErrorMessage = "veuillez saisir le nom de l'auteur ")]
    [StringLength(250)]
    public String? Author { get; set; }
    public double Price { get; set; }
    public double StockQuantity { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; } // navigation


}
