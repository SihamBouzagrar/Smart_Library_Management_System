using System.ComponentModel.DataAnnotations;

namespace Smart_Library_Management_System.Models;

public class BorrowItem
{
    public int BorrowItemID { get; set; }

    [Required]
    public int BorrowID { get; set; }
    public Borrow? Borrow { get; set; }

    [Required]
    public int BookID { get; set; } // ⚠ ici c’est le bon FK
    public Book? Book { get; set; }

    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
}