using OEP.Core.DomainModels.SubCategoryModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OEP.Core.DomainModels.StudyMaterialsModel
{
  public  class StudyMaterial:CommonDetailsEntity
    {

        public string Name { get; set; }

        public int SubcategoryID { get; set; }

        [ForeignKey("SubcategoryID")]
        public virtual SubCategory SubCategory { get; set; }
        public string FilePath { get; set; }
    }
}
