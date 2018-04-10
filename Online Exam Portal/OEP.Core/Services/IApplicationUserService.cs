using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OEP.Core.DomainModels.Identity;

namespace OEP.Core.Services
{
  public  interface IApplicationUserService
    {
        List<ApplicationUser> GetApplicationUsers();
    }
}
