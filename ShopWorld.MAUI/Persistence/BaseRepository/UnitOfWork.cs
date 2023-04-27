using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWorld.MAUI.Repository
{
    public class UnitOfWork:IUnitOfWork
    {
        private SQLiteAsyncConnection Database;
        private Dictionary<Type, object> Repositories;
        public UnitOfWork() {
            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        }

        public IGenericRepository<T> GetRepository<T>() where T : class, new()
        {
            if(this.Repositories == null)
            {
                this.Repositories = new Dictionary<Type, object>();
            }
            var type = typeof(T);
            if (!this.Repositories.ContainsKey(type))
            {
                Database.CreateTableAsync<T>();
                this.Repositories[type] = new GenericRepository<T>(Database);
            }
            return (GenericRepository<T>)this.Repositories[type];
        }
    }
}
