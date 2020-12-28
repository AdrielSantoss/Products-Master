using API.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repositories
{
    public class Repository : IRepository
    {

        private readonly DatabaseContext _context;
        public Repository(DatabaseContext contextDb)
        {
            _context = contextDb;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }
        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}

