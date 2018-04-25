using System.ComponentModel.DataAnnotations;

namespace OEP.Resources.Admin
{
    public class EducationTypeResource:BaseResource
    {
        [Required]
        [Display(Name = "Name")]
      
        public string Name { get; set; }
    }
}
