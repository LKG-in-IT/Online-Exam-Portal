using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OEP.Core.Data.Repository;
using OEP.Core.DomainModels.Category;
using OEP.Core.Services;

namespace OEP.Services
{
    public class CategoryService: BaseService<Category>, ICategoryService
    {
        public CategoryService(IUnitOfWork unitOfWork) :base(unitOfWork)
        {

        }
    }
}
