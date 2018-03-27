using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OEP.Core.Data.Repository;
using OEP.Core.DomainModels.EducationModels;
using OEP.Core.Services;

namespace OEP.Services
{
    public class YearService:BaseService<YearDetails>,IYearService
    {
        public YearService(IUnitOfWork unitOfWork):base(unitOfWork)
        {
            
        }
    }
}
