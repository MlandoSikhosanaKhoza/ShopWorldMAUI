using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShopWorld.MAUI.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class , new()
    {
        private SQLiteAsyncConnection _database;
        public GenericRepository(SQLiteAsyncConnection database) { 
            _database= database;
        }
        public async Task<List<T>> GetAsync(Expression<Func<T, bool>> filter = null)
        {
            if (filter != null)
            {
                return await _database.Table<T>().Where(filter).ToListAsync();
            }
            return await _database.Table<T>().ToListAsync();
        }

        public Task<int> InsertAsync(T entity)
        {
            return _database.InsertAsync(entity);
        }

        public Task<int> UpdateAsync(T entity)
        {
            return _database.UpdateAsync(entity);
        }

        public Task<int> DeleteAsync(T entity)
        {
            return _database.DeleteAsync(entity);
        }

        public async Task<bool> DeleteAllAsync()
        {
            try
            {
                await _database.DeleteAllAsync<T>();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Task<int> CountAsync()
        {
            return _database.Table<T>().CountAsync();
        }
    }
}
