using API.Data;
using API.Models;
using Microsoft.EntityFrameworkCore;
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
        public void Update<T>(T entity) where T: class
        {
            _context.Update(entity);
        }
        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }
        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
        public async Task<User[]> GetAllUsers()
        {
            IQueryable<User> query = _context.Users;

            query = query.AsNoTracking().OrderBy(u => u.Id);
            return await query.ToArrayAsync();
        }
        public async Task<User> GetUserById(int id)
        {
            IQueryable<User> query = _context.Users;

            query = query.AsNoTracking().Where(u => u.Id == id);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<User> SingleOrDefaultByEmailAsync(string email)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
        }
    }
}

