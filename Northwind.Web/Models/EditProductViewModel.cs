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
        [Display(Name = "Product Name")]
        [StringLength(40)]
        public string ProductName { get; set; }
        public int? SupplierId { get; set; }
        public int? CategoryId { get; set; }
        [Required(ErrorMessage = "Enter Discontinued")]
        [Display(Name = "Discontinued")]
        public bool Discontinued { get; set; }
    }
}
