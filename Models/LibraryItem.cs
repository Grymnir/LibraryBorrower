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
        [Required]
        public string Title { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public int? Pages { get; set; }
        public int? RunTimeMinutes { get; set; }
        [Required]
        public bool IsBorrowable { get; set; }
        public virtual string Borrower { get; set; }
        public virtual DateTime? BorrowDate { get; set; }
        [Required]
        public string Type { get; set; }
        [NotMapped]
        public ICollection<Category> MyCategory { get; set; }
    }
}
