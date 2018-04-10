using OEP.Core.DomainModels.ExamModels;
using OEP.Core.DomainModels.SubCategoryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OEP.Resources.Admin
{
   public class ExamResource : BaseResource
    {
        public string Name { get; set; }
        public int Examtypeid { get; set; }
        public int SubcategoryId { get; set; }
        public ExamType ExamType { get; set; }
        public SubCategory SubCategory { get; set; }
        public int Passmark { get; set; }
    }
}
