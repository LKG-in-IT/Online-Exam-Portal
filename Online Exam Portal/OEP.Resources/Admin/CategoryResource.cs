using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OEP.Resources.Admin
{
 public   class CategoryResource:BaseResource
    {
        [Required]
        [Display(Name = "Name")]
      
        public string Name { get; set; }        

    }
}
