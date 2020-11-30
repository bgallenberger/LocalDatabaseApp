using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using SQLite;

namespace LocalDatabaseApp
{
    public class FactDatabase
    {
        static readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        });

        static SQLiteAsyncConnection Database => lazyInitializer.Value;
        static bool initialized = false;

        public FactDatabase()
        {
            InitializeAsync().SafeFireAndForget(false);
        }

        async Task InitializeAsync()
        {
            if (!initialized)
            {
                if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(Fact).Name))
                {
                    await Database.CreateTablesAsync(CreateFlags.None, typeof(Fact)).ConfigureAwait(false);
                }
                initialized = true;
            }
        }

        public Task<List<Fact>> GetFactsAsync()
        {
            return Database.Table<Fact>().ToListAsync();
        }

        public Task<Fact> GetFactAsync(int id)
        {
            return Database.Table<Fact>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveFactAsync(Fact item)
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

        public Task<int> DeleteFactAsync(Fact item)
        {
            return Database.DeleteAsync(item);
        }
    }
}
