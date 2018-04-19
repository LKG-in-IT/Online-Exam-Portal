using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using OEP.Core.Data;
using OEP.Core.DomainModels;
using OEP.Core.DomainModels.Identity;

namespace OEP.Core.Services
{
  public  interface IApplicationUserService
    {
        List<ApplicationUser> GetApplicationUsers();
        PaginatedList<ApplicationUser> GetApplicationUsers(int pageIndex, int pageSize, Expression<Func<ApplicationUser, object>> keySelector, Expression<Func<ApplicationUser, bool>> predicate, OrderBy orderBy, params Expression<Func<ApplicationUser, object>>[] includeProperties);
        ApplicationUser GetById(string UserName);
        string Update(ApplicationUser entity);
        List<RolesViewModel> GetRoles();
        string UpdateRole(string username, string rolename);
        void DeleteRoles(string username);
        string AddUser(ApplicationUser user);
    }
}
