using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShopWorld.MAUI.Repository
{
    public interface IGenericRepository<T> where T : class,new()
    {
        Task<List<T>> GetAsync(Expression<Func<T, bool>> filter = null);
        Task<int> InsertAsync(T entity);
        Task<int> UpdateAsync(T entity);
        Task<int> DeleteAsync(T entity);
        Task<bool> DeleteAllAsync();
        Task<int> CountAsync();
    }
}
