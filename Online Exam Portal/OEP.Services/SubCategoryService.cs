using OEP.Core.Data.Repository;
using OEP.Core.DomainModels.SubCategoryModel;
using OEP.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OEP.Services
{
    public class SubCategoryService:BaseService<SubCategory>,ISubCategoryService
    {
        public SubCategoryService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
