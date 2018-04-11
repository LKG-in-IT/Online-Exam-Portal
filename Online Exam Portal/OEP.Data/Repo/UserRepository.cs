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
        private readonly OepDbContext _OepDbContext=new OepDbContext();
        
        public List<ApplicationUser> GetApplicationUsers()
        {
            var userList= _OepDbContext.Users.ToList();
            return userList;
        }


        public ApplicationUser GetById(string id)
        {
            return _OepDbContext.Users.Where(i => i.Id == id).FirstOrDefault();
        }

        public PaginatedList<ApplicationUser> GetApplicationUsers(int pageIndex, int pageSize, Expression<Func<ApplicationUser, object>> keySelector, Expression<Func<ApplicationUser, bool>> predicate, OrderBy orderBy, params Expression<Func<ApplicationUser, object>>[] includeProperties)
        {
            var entities = FilterQuery(keySelector, predicate, orderBy, includeProperties);
            var total = entities.Count();// entities.Count() is different than pageSize
            entities = entities.Paginate(pageIndex, pageSize);
            return entities.ToPaginatedList(pageIndex, pageSize, total);


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

        //public void Insert(ApplicationUser entity)
        //{
        //    _OepDbContext.SetAsAdded(entity)
        //}

        public string Update(ApplicationUser entity)
        {
            ApplicationUser user = _OepDbContext.Users.Where(i => i.Id == entity.Id).FirstOrDefault();
            user.Name = entity.Name;
            user.Email = entity.Email;
            user.UserName = entity.UserName;
            user.PhoneNumber = entity.PhoneNumber;

          int r=  _OepDbContext.SaveChanges();
            if(r>0)
            {
                return "Success";
            }
            else
            {
                return "Error";
            }

        }

        //public void Delete(ApplicationUser entity)
        //{
        //    _context.SetAsDeleted(entity);
        //}

    }
}
