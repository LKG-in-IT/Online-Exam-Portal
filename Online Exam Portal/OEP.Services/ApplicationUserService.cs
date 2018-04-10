using OEP.Core.Data.Repository;
using OEP.Core.DomainModels.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OEP.Core.Services;

namespace OEP.Services
{
    public class ApplicationUserService : IApplicationUserService
    {
        private readonly IUserRepository _userRepository;

        public ApplicationUserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public  List<ApplicationUser> GetApplicationUsers()
        {
            return  _userRepository.GetApplicationUsers();
        }
    }
}
