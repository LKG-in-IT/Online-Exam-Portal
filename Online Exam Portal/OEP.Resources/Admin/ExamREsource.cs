using OEP.Core.DomainModels.ExamModels;
using OEP.Core.DomainModels.SubCategoryModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OEP.Resources.Admin
{
   public class ExamResource : BaseResource
    {
        [Required]
        [Display(Name = "Name")]
     
        public string Name { get; set; }
        public int Examtypeid { get; set; }
        public int SubcategoryId { get; set; }
        public ExamType ExamType { get; set; }
        public SubCategory SubCategory { get; set; }
        [Required]
        [Display(Name = "Passmark")]
   
        public int Passmark { get; set; }

        public int Duration { get; set; }

        public bool AllowReAttempts { get; set; }
    }
}
