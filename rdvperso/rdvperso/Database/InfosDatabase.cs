using rdvperso.Models;
using rdvperso.Services;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rdvperso.Database
{
    public static class TaskExtensions
    {
        // NOTE: Async void is intentional here. This provides a way
        // to call an async method from the constructor while
        // communicating intent to fire and forget, and allow
        // handling of exceptions
        public static async void SafeFireAndForget(this Task task,
            bool returnToCallingContext,
            Action<Exception> onException = null)
        {
            try
            {
                await task.ConfigureAwait(returnToCallingContext);
            }

            // if the provided action is not null, catch and
            // pass the thrown exception
            catch (Exception ex) when (onException != null)
            {
                onException(ex);
            }
        }
    }
    public class InfosDatabase
    {

        static readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        });

        static SQLiteAsyncConnection Database => lazyInitializer.Value;



        static bool initialized = false;


        public InfosDatabase()
        {
            InitializeAsync().SafeFireAndForget(false);
        }

        async Task InitializeAsync()
        {
            if (!initialized)
            {
                if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(Infos).Name))
                {
                    await Database.CreateTablesAsync(CreateFlags.None, typeof(Infos)).ConfigureAwait(false);
                    initialized = true;
                }
            }
        }


        public Task<List<Infos>> GetItemsAsync()
        {
            return Database.Table<Infos>().ToListAsync();
        }

        public Task<List<Infos>> GetItemsNotDoneAsync()
        {
            // SQL queries are also possible
            return Database.QueryAsync<Infos>("SELECT * FROM [TodoItem] WHERE [Done] = 0");
        }

        public Task<Infos> GetItemAsync(int id)
        {
            return Database.Table<Infos>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(Infos item)
        {
            if (item.ID != 0)
            {
                return Database.UpdateAsync(item);
            }
            else
            {
                return Database.InsertAsync(item);
            }
        }

        public Task<int> DeleteItemAsync(Infos item)
        {
            return Database.DeleteAsync(item);
        }
    }
}
