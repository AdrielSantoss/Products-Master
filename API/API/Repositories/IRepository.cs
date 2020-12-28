using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repositories
{
    public interface IRepository
    {

        void Add<T>(T entity) where T : class;
        Task<bool> SaveChangesAsync();
    }
}
