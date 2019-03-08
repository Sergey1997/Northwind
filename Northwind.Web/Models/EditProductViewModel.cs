using Microsoft.AspNetCore.Mvc.Rendering;
using Northwind.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Web.Models
{
    public class EditProductViewModel
    {
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Enter Product Name")]
        [Display(Name = "Name of product")]
        [StringLength(40)]
        public string ProductName { get; set; }
        [Required(ErrorMessage = "Enter Supplier Name")]
        [Display(Name = "Name of supplier")]
        public int? SupplierId { get; set; }
        [Required(ErrorMessage = "Enter Category Name")]
        [Display(Name = "Name of category")]
        public int? CategoryId { get; set; }
        [Required(ErrorMessage = "Enter Discontinued status")]
        [Display(Name = "Discontinued")]
        public bool Discontinued { get; set; }
    }
}
