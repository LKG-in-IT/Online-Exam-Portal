using OEP.Core.Data.Repository;
using OEP.Core.DomainModels.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OEP.Data.Repo
{
    public class UserRepository : IUserRepository
    {
        private readonly OepDbContext _OepDbContext=new OepDbContext();
        
        public List<ApplicationUser> GetApplicationUsers()
        {
            var userList= _OepDbContext.Users.ToList();
            return userList;
        }
    }
}
