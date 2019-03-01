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
        [Required(ErrorMessage = "Enter Category Name")]
        [Display(Name = "Category Name")]
        [StringLength(30)]
        public string CategoryName { get; set; }
        [Required(ErrorMessage = "Enter Description")]
        [Display(Name = "Description")]
        [StringLength(100)]
        public string Description { get; set; }
    }
}
