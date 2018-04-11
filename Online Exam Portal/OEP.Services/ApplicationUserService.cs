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

        public ApplicationUser GetById(string id)
        {
           return _userRepository.GetById(id);
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
       
    }
}
