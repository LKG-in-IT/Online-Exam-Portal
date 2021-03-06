﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OEP.Core.Data;
using OEP.Core.Data.Repository;
using OEP.Core.DomainModels;
using OEP.Core.DomainModels.Identity;
using OEP.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OEP.Data.Repo
{
    public class UserRepository : IUserRepository
    {
        private readonly OepDbContext _OepDbContext = new OepDbContext();


        public List<ApplicationUser> GetApplicationUsers()
        {
            var userList = _OepDbContext.Users.ToList();

            return userList;
        }


        public ApplicationUser GetById(string UserName)
        {

            var userListWithRole = _OepDbContext.Users.ToList();
            var userListWithoutRole = userListWithRole.Select(u =>
            new ApplicationUser
            {
                Name = u.Name,
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                Status = u.Status,
                Role = String.Join(",", _OepDbContext.Roles.Where(role => role.Users.Any(user => user.UserId == u.Id))
                                    .Select(r => r.Name))
            }).ToList().Where(i => i.UserName == UserName).FirstOrDefault();


            return userListWithoutRole;
        }

        public List<RolesViewModel> GetRoles()
        {
            var Rolelist = _OepDbContext.Roles.Select(s => new RolesViewModel { Id = s.Id, Name = s.Name }).ToList();
            return Rolelist;

        }

        public PaginatedList<ApplicationUser> GetApplicationUsers(int pageIndex, int pageSize, Expression<Func<ApplicationUser, object>> keySelector, Expression<Func<ApplicationUser, bool>> predicate, OrderBy orderBy, params Expression<Func<ApplicationUser, object>>[] includeProperties)
        {
            var entities = FilterQuery(keySelector, predicate, orderBy, includeProperties);
            var total = entities.Count();// entities.Count() is different than pageSize
            entities = entities.Paginate(pageIndex, pageSize);
            var userListWithoutRole = entities.ToPaginatedList(pageIndex, pageSize, total);
            var userListWithRole = userListWithoutRole.Select(u =>
            new ApplicationUser
            {
                Name = u.Name,
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                Status = u.Status,
                Role = String.Join(",", _OepDbContext.Roles.Where(role => role.Users.Any(user => user.UserId == u.Id))
                                    .Select(r => r.Name))
            }). ToList();

            if(userListWithoutRole!=null)
            {
                var usersNotInRole = userListWithRole.Where(m => !m.Role.Split(',').Contains("Admin"));
                return new PaginatedList<ApplicationUser>(usersNotInRole, pageIndex, pageSize, total);

            }




            return new PaginatedList<ApplicationUser>(userListWithRole, pageIndex, pageSize, total);


        }
        private IQueryable<ApplicationUser> FilterQuery(Expression<Func<ApplicationUser, object>> keySelector, Expression<Func<ApplicationUser, bool>> predicate, OrderBy orderBy,
          Expression<Func<ApplicationUser, object>>[] includeProperties)
        {
            var entities = IncludeProperties(includeProperties);
            entities = (predicate != null) ? entities.Where(predicate) : entities;
            entities = (orderBy == OrderBy.Ascending)
                ? entities.OrderBy(keySelector)
                : entities.OrderByDescending(keySelector);
            return entities;
        }
        private IQueryable<ApplicationUser> IncludeProperties(params Expression<Func<ApplicationUser, object>>[] includeProperties)
        {

            IQueryable<ApplicationUser> entities = _OepDbContext.Users;


            foreach (var includeProperty in includeProperties)
            {
                entities = entities.Include(includeProperty);
            }
            return entities;
        }



        public string Update(ApplicationUser entity)
        {
            try
            {
                ApplicationUser user = _OepDbContext.Users.Where(i => i.UserName == entity.UserName).FirstOrDefault();
                
                user.Name = entity.Name;
                user.Email = entity.Email;
                user.UserName = entity.UserName;
                user.PhoneNumber = entity.PhoneNumber;
         

                int r = _OepDbContext.SaveChanges();
                if (r > 0)
                {


                    return "Success";
                }
                else
                {


                    return "Error";
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public string UpdateStatus(ApplicationUser entity)
        {
            try
            {
                ApplicationUser user = _OepDbContext.Users.Where(i => i.UserName == entity.UserName).FirstOrDefault();


                if (user.Status==true)
                {
                    user.Status = false;
                }
              else
                {
                    user.Status = true;
                }

                int r = _OepDbContext.SaveChanges();
                if (r > 0)
                {


                    return "Success";
                }
                else
                {


                    return "Error";
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public string ResetPassword(string username)
        {
            var user = _OepDbContext.Users.Where(u => u.UserName.Equals(username, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

         

            var userStore = new UserStore<ApplicationUser>(_OepDbContext);
            var userManager = new UserManager<ApplicationUser>(userStore);

            String hashedNewPassword = userManager.PasswordHasher.HashPassword("Test@123");
            userStore.SetPasswordHashAsync(user, hashedNewPassword);
       

            return "Changed";



        }

        

        public string UpdateRole(string username, string rolename)
        {
            var user = _OepDbContext.Users.Where(u => u.UserName.Equals(username, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

            var allroles = _OepDbContext.Roles;

            var userStore = new UserStore<ApplicationUser>(_OepDbContext);
            var userManager = new UserManager<ApplicationUser>(userStore);

            
     
            userManager.AddToRole(user.Id, rolename);
         
            return "Changed";



        }



        public void DeleteRoles(string username)
        {
            var user = _OepDbContext.Users.Where(u => u.UserName.Equals(username, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();


            var allroles = _OepDbContext.Roles.Where(role => role.Users.Any(uu => uu.UserId == user.Id))
                                    .Select(r => r.Name).ToList();

            var userStore = new UserStore<ApplicationUser>(_OepDbContext);
            var userManager = new UserManager<ApplicationUser>(userStore);

            foreach (var item in allroles)
            {
                userManager.RemoveFromRole(user.Id, item);
            }
        }
        public string AddUser(ApplicationUser user)
        {
            var userStore = new UserStore<ApplicationUser>(_OepDbContext);

            var userManager = new UserManager<ApplicationUser>(userStore);
            var result = userManager.Create(user, "Test@123");
            if (result.Succeeded)
            {
                string[] selectedRoles = user.Role.Split(',');

                foreach (string item in selectedRoles)
                {
                    userManager.AddToRole(user.Id, item);
                }
              
                return "Success";
            }
            return "Error";
        }


    }
}
