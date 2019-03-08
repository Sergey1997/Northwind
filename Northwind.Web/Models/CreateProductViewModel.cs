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
        [StringLength(70)]
        public string ProductName { get; set; }
        [Required(ErrorMessage = "Enter Supplier")]
        public int? SupplierId { get; set; }
        [Required(ErrorMessage = "Enter Category")]
        public int? CategoryId { get; set; }
    }
}
