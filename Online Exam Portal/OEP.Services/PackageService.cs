using OEP.Core.Data.Repository;
using OEP.Core.DomainModels.PackageModel;
using OEP.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OEP.Services
{
public     class PackageService:BaseService<Package>,IPackageService
    {
        public PackageService(IUnitOfWork unitOfWork):base(unitOfWork)
        {

        }
    }
}
