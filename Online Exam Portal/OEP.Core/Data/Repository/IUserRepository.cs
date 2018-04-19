using OEP.Core.DomainModels;
using OEP.Core.DomainModels.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OEP.Core.Data.Repository
{
    public interface IUserRepository
    {
        List<ApplicationUser> GetApplicationUsers();
        PaginatedList<ApplicationUser> GetApplicationUsers(int pageIndex, int pageSize, Expression<Func<ApplicationUser, object>> keySelector, Expression<Func<ApplicationUser, bool>> predicate, OrderBy orderBy, params Expression<Func<ApplicationUser, object>>[] includeProperties);
        ApplicationUser GetById(string id);
        string Update(ApplicationUser entity);
        List<RolesViewModel> GetRoles();
        string UpdateRole(string username, string rolename);
        void DeleteRoles(string username);
        string AddUser(ApplicationUser user);
        string UpdateStatus(ApplicationUser entity);
    }
}
