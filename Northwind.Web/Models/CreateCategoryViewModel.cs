using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Web.Models
{
    public class CreateCategoryViewModel
    {
        [Required(ErrorMessage = "Enter Category Name")]
        [StringLength(15)]
        public string CategoryName { get; set; }
        [Required(ErrorMessage = "Enter Description")]
        [StringLength(100)]
        public string Description { get; set; }
    }
}
