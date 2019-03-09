using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Web.Models
{
    public class EditCategoryViewModel
    {
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Enter Description")]
        [Display(Description = "Category of name")]
        [StringLength(15)]
        public string CategoryName { get; set; }
        [Required(ErrorMessage = "Enter Description")]
        [Display(Description = "Description")]
        [StringLength(100)]
        public string Description { get; set; }
    }
}
