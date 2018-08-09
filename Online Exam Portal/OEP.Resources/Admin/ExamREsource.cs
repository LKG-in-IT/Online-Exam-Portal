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
        public ExamResource()
        {
            ExamType = new ExamType();
            SubCategory = new SubCategory();
        }
        [Required]
        [Display(Name = "Name")]
     
        public string Name { get; set; }
        public string Description { get; set; }

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

    public class ExamStartResource : BaseResource
    {
        public ExamStartResource()
        {
            ExamResource = new ExamResource();
            QuestionsResource = new List<QuestionsResource>();
        }
        public ExamResource ExamResource { get; set; }
        public List<QuestionsResource> QuestionsResource { get; set; }
    }
}
