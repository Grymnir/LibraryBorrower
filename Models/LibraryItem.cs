using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryBorrower.Models
{
    public class LibraryItem
    {
        public int ID { get; set; }
        public Category categoryID { get; set; }
        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$", ErrorMessage = "Only Text Please! No numbers!")]
        [Required]
        public string Title { get; set; }
        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$", ErrorMessage = "Only text please!")]
        [Required]
        public string Author { get; set; }
        [Required]
        [Range(1, 1000)]
        public int? Pages { get; set; }
        [Range(1, 1000)]
        public int? RunTimeMinutes { get; set; }
        [Required]
        public bool IsBorrowable { get; set; }
        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$", ErrorMessage = "Borrower cant be number! Only text!")]
        public string Borrower { get; set; }
        [DataType(DataType.Date)]
        public virtual DateTime? BorrowDate { get; set; }
        [Required]
        public string Type { get; set; }
        [NotMapped]
        public ICollection<Category> MyCategory { get; set; }

    }
}
