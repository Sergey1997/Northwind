using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Web.Models
{
    public class CreateProductViewModel
    {
        [Display(Name = "Product Name")]
        [Required(ErrorMessage ="Input product name")]
        [RegularExpression(@"^\S*$", ErrorMessage = "No white space allowed")]
        [StringLength(40)]
        public string ProductName { get; set; }
        [Display(Name = "Supplier name")]
        [Required(ErrorMessage = "Enter Supplier")]
        public int? SupplierId { get; set; }
        [Display(Name = "Category name")]
        [Required(ErrorMessage = "Enter Category")]
        public int? CategoryId { get; set; }
    }
}
