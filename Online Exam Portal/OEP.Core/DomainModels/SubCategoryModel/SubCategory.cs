using OEP.Core.DomainModels.CategoryModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OEP.Core.DomainModels.SubCategoryModel
{
 public   class SubCategory: CommonDetailsEntity
    {
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
        public string Name { get; set; }
    }
}
