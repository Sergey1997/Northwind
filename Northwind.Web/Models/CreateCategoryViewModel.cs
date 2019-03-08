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
        [StringLength(50)]
        public string CategoryName { get; set; }
        [Required(ErrorMessage = "Enter Description")]
        [StringLength(150)]
        public string Description { get; set; }
    }
}
