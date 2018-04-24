using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OEP.Resources.Admin
{
   public  class PackageResource:BaseResource
    {

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Required]
        public string Details { get; set; }
        [Required]
      
        [DataType(DataType.Currency,ErrorMessage ="Must Be Numeric")]
        public Double Prize { get; set; }
        [Required]

        public string Duration { get; set; }
      

    }
}
