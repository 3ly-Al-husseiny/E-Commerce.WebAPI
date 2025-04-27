using Domain.Contracts;
using Domain.Models;
using Persistence.Data.Contexts;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly E_CommerceDbContext _context;
        //private readonly Dictionary<string, object> _repositories = new Dictionary<string,object>;
        private readonly ConcurrentDictionary<string, object> _repositories = new ConcurrentDictionary<string, object>();
        public UnitOfWork(E_CommerceDbContext context)
        {
            _context = context;
        }

        public IGenericRepository<TEntity, Tkey> GetRepository<TEntity, Tkey>() where TEntity : BaseEntity<Tkey>
        {
            #region Using Dictionary
            //var type = typeof(TEntity).Name; // Product as string
            //if (!_repositories.ContainsKey(type))
            //{
            //    var repository = new GenericRepository<TEntity, Tkey>(_context);
            //    _repositories.Add(type, repository);
            //}
            //return (IGenericRepository<TEntity, Tkey>)_repositories[type]; 
            #endregion

            #region Using Concurrent Dictionary
            return (IGenericRepository<TEntity,Tkey>)_repositories.GetOrAdd(typeof(TEntity).Name, new GenericRepository<TEntity, Tkey>(_context));
            #endregion
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
