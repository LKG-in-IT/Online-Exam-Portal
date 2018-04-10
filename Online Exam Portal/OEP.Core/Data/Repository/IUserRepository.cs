using OEP.Core.DomainModels.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OEP.Core.Data.Repository
{
    public interface IUserRepository
    {
      List<ApplicationUser> GetApplicationUsers();
    }
}
