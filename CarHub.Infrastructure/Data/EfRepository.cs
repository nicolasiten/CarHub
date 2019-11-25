using CarHub.Core.Entities;
using CarHub.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CarHub.Infrastructure.Data
{
    public class EfRepository<T> : IAsyncRepository<T> where T: BaseEntity, new()
    {
        protected readonly ApplicationDbContext dbContext;
        protected readonly IValidator<T> validator;

        public EfRepository(ApplicationDbContext dbContext, IValidator<T> validator)
        {
            this.dbContext = dbContext;
            this.validator = validator;
        }

        public virtual async Task<T> GetByIdAsync(object id)
        {
            return await dbContext.Set<T>().FindAsync(id);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await dbContext.Set<T>().ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>,
            IOrderedQueryable<T>> orderBy = null, string includeProperties = "")
        {
            IQueryable<T> query = dbContext.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (string includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }

            return await query.ToListAsync();
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            await validator.ValidateAndThrowAsync(entity);

            dbContext.Set<T>().Add(entity);
            await dbContext.SaveChangesAsync();

            return entity;
        }

        public virtual async Task UpdateAsync(T entity)
        {
            await validator.ValidateAndThrowAsync(entity);

            dbContext.Set<T>().Update(entity);
            await dbContext.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(object id)
        {
            T entity = await dbContext.Set<T>().FindAsync(id);

            if (entity != null)
            {
                dbContext.Set<T>().Remove(entity);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
