using OEP.Core.DomainModels.CategoryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OEP.Resources.Admin
{
   public  class SubCategoryResource:BaseResource
    {
        public int CategoryID { get; set; }
        public string Name { get; set; }
        public CategoryResource Category { get; set; }

    }
}
