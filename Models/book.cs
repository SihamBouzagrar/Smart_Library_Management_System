
using System.ComponentModel.DataAnnotations;

namespace Smart_Library_Management_System.Models;

public class Book
{
    public int BookID { get; set; }
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be beween 3 and 100 caracteres")]
    [Display(Name = "Title")]
    public String? Title { get; set; }
    [Required(ErrorMessage = "veuillez saisir le nom de l'auteur ")]
    [StringLength(250)]
    public String? Author { get; set; }
    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "Le prix doit être positif.")]
    [DataType(DataType.Currency)]
    [Display(Name = "Price")]
    public Double Price { get; set; }
    [Required]
    [Range(0, 1000, ErrorMessage = "Le stock doit être entre 0 et 1000.")]
    [Display(Name = "Stock Quantity")]
    public int StockQuantity { get; set; }
    [Required(ErrorMessage = "La catégorie est obligatoire.")]
    [Range(1, int.MaxValue, ErrorMessage = "Please select a category.")]
    public int CategoryId { get; set; }
    public Category? Category { get; set; } // navigation
    public ICollection<BorrowItem> BorrowItems { get; set; } = new List<BorrowItem>();


}
