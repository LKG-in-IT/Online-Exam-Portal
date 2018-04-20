using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OEP.Resources.Admin
{
    public class YearResource:BaseResource
    {
        [Required]
        [Display(Name = "Year")]
        [Range(1900,2050, ErrorMessage = "Please enter a valid year")]
        public string Year { get; set; }
    }
}
