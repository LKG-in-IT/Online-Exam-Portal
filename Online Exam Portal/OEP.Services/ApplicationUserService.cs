using OEP.Core.Data.Repository;
using OEP.Core.DomainModels.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OEP.Core.Services;
using System.Linq.Expressions;
using OEP.Core.Data;
using OEP.Core.DomainModels;

namespace OEP.Services
{
    public class ApplicationUserService : IApplicationUserService
    {
        private readonly IUserRepository _userRepository;

        public ApplicationUserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public ApplicationUser GetById(string UserName)
        {
           return _userRepository.GetById(UserName);
        }
        public  List<ApplicationUser> GetApplicationUsers()
        {
            return  _userRepository.GetApplicationUsers();
        }
        public PaginatedList<ApplicationUser> GetApplicationUsers(int pageIndex, int pageSize, Expression<Func<ApplicationUser, object>> keySelector, Expression<Func<ApplicationUser, bool>> predicate, OrderBy orderBy, params Expression<Func<ApplicationUser, object>>[] includeProperties)
        {


            return _userRepository.GetApplicationUsers(pageIndex, pageSize, keySelector, predicate, orderBy, includeProperties);
        }
        public string Update(ApplicationUser entity)
        {
            string result = _userRepository.Update(entity);

            return result;
        }
      public List<RolesViewModel> GetRoles()
        {
            return _userRepository.GetRoles();
        }
        public string UpdateRole(string username, string rolename)
        {
            return _userRepository.UpdateRole(username,rolename);
        }
        public void DeleteRoles(string username)
        {
          _userRepository.DeleteRoles(username);

        }
        public string AddUser(ApplicationUser user)
        {
            return _userRepository.AddUser(user);
             
        }
        public string UpdateStatus(ApplicationUser entity)
        {
            return _userRepository.UpdateStatus(entity);
        }
    }
}
