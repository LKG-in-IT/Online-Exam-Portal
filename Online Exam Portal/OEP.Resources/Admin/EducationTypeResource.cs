using System.ComponentModel.DataAnnotations;

namespace OEP.Resources.Admin
{
    public class EducationTypeResource:BaseResource
    {
        [Required]
        [Display(Name = "Name")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string Name { get; set; }
    }
}
