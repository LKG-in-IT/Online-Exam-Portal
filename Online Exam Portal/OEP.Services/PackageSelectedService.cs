using OEP.Core.Data.Repository;
using OEP.Core.DomainModels.PackageSelectedModels;
using OEP.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OEP.Services
{
  public  class PackageSelectedService:BaseService<PackageSelected>, IPackageSelectedService
    {
        public PackageSelectedService(IUnitOfWork unitOfWork):base(unitOfWork)
        {

        }
    }
}
