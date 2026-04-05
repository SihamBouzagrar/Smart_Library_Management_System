

using System.ComponentModel.DataAnnotations;

namespace Smart_Library_Management_System.Models;

public class Category
{
    public int CategoryID { get; set; }
    [Required(ErrorMessage = "veuillez saisir le nom du category")]
    [StringLength(250)]
    public String? Name { get; set; }
    
    // Navigation property
    public ICollection<Book> Books { get; set; } = new List<Book>();

   
}
