﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using OEP.Core.Data;
using OEP.Core.DomainModels;

namespace OEP.Core.Services
{
    public interface IService<TEntity> : IService where TEntity : BaseEntity
    {
        List<TEntity> GetAll();
        PaginatedList<TEntity> GetAll(int pageIndex, int pageSize);
        PaginatedList<TEntity> GetAll(int pageIndex, int pageSize, Expression<Func<TEntity, object>> keySelector, OrderBy orderBy = OrderBy.Ascending);
        PaginatedList<TEntity> GetAll(int pageIndex, int pageSize, Expression<Func<TEntity, object>> keySelector, Expression<Func<TEntity, bool>> predicate, OrderBy orderBy, params Expression<Func<TEntity, object>>[] includeProperties);
     
        TEntity GetById(int id);
        void Add(TEntity entity);

        void UnitOfWorkSaveChanges();
        void Update(TEntity entity);
        void Delete(TEntity entity);
        Task<List<TEntity>> GetAllAsync();
        Task<PaginatedList<TEntity>> GetAllAsync(int pageIndex, int pageSize);
        Task<PaginatedList<TEntity>> GetAllAsync(int pageIndex, int pageSize, Expression<Func<TEntity, object>> keySelector, OrderBy orderBy = OrderBy.Ascending);
        Task<PaginatedList<TEntity>> GetAllAsync(int pageIndex, int pageSize, Expression<Func<TEntity, object>> keySelector, Expression<Func<TEntity, bool>> predicate, OrderBy orderBy, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<TEntity> GetByIdAsync(int id);
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);

        Task<List<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> predicate);

        List<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> GetSingleIncludingAsync(int id, params Expression<Func<TEntity, object>>[] includeProperties);
    }
}
