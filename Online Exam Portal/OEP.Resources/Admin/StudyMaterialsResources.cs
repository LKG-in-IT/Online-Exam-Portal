using OEP.Core.DomainModels.SubCategoryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OEP.Resources.Admin
{
   public  class StudyMaterialsResources:BaseResource
    {
        public string Name { get; set; }
        public int SubcategoryID { get; set; }
        public SubCategory subCategory { get; set; }
        public string Filepath { get; set; }
    }
}
