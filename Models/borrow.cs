using System.ComponentModel.DataAnnotations;

namespace Smart_Library_Management_System.Models;

public class Borrow
{
    public int BorrowID { get; set; }

    [Required(ErrorMessage = "Borrower name is required")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters")]
    [Display(Name = "Borrower Name")]
    public string? BorrowName { get; set; }

    [Required]
    [DataType(DataType.DateTime)]
    public DateTime BorrowDate { get; set; } = DateTime.Now;

    public DateTime? ReturnDate { get; set; }

    

    public ICollection<BorrowItem> BorrowItems { get; set; } = new List<BorrowItem>();
}