using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryBorrower.Models
{
    public class Category
    {
        public int ID { get; set; }
        [Required]
        
        public string CategoryName { get; set; }
        public ICollection<LibraryItem> items { get; set; }
    }
}
