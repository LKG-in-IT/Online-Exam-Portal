using System.Data.Entity;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OEP.Core.DomainModels.Identity;
using OEP.Data.IdentityConfig;

namespace OEP.Data
{
    public class ApplicationDbInitializer:CreateDatabaseIfNotExists<OepDbContext>
    {
        protected override void Seed(OepDbContext context)
        {
            InitializeIdentityForEf(context);
            base.Seed(context);
        }

        //Create User=Admin@Admin.com with password=Admin@123456 in the Admin role        
        public void InitializeIdentityForEf(OepDbContext db)
        {
            
            
            
        }
    }
}