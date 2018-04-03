using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OEP.Core.DomainModels.Identity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace OEP.Data.Migrations
{
  
    internal sealed class Configuration : DbMigrationsConfiguration<OEP.Data.OepDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(OEP.Data.OepDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            string[] items = new string[3];
            items[0] = "Admin";
            items[1] = "User";
            items[2] = "Faculty";

            foreach (var item in items)
            {
                if (!context.Roles.Any(r => r.Name == item))
                {
                    var store = new RoleStore<IdentityRole>(context);
                    var manager = new RoleManager<IdentityRole>(store);
                    var role = new IdentityRole { Name = item };

                    manager.Create(role);
                }
            }


            string[] itemsUSers = new string[3];
            itemsUSers[0] = "admin@test.com";
            itemsUSers[1] = "user@test.com";
            itemsUSers[2] = "faculty@test.com";

            foreach (var item in itemsUSers)
            {
                if(!context.Users.Any(u => u.UserName == item))
                {
                    var store = new UserStore<ApplicationUser>(context);
                    var manager = new UserManager<ApplicationUser>(store);
                    var user = new ApplicationUser { UserName = item };

                    manager.Create(user, "test@123");
                    if (item == "admin@test.com")
                    {
                        manager.AddToRole(user.Id, "Admin");
                    }
                    if (item == "user@test.com")
                    {
                        manager.AddToRole(user.Id, "User");
                    }
                    if (item == "faculty@test.com")
                    {
                        manager.AddToRole(user.Id, "Faculty");
                    }
                }
            }
           
        }
    }
}
