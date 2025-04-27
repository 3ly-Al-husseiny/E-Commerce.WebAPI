using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    // The UnitOfWork is "Wasit" Between Any Repository and the Database.
    public interface IUnitOfWork
    {
       // SaveChangesAsync() --> SaveChanges
       Task<int> SaveChangesAsync();

        // Generate Repository
        IGenericRepository<TEntity,Tkey> GetRepository<TEntity,Tkey>() where TEntity : BaseEntity<Tkey>;
    }
}
