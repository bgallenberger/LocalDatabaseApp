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

                    var query = await Database.QueryAsync<Fact>("Select * From [Fact]");
                    int count = 0;
                    foreach (var fact in query)
                    {
                        count++;
                    }

                    if (count == 0)
                    {
                        Fact fact1 = new Fact();
                        fact1.Name = "Name";
                        fact1.TheFact = "My name is Brian Gallenberger";
                        await SaveFactAsync(fact1);

                        Fact fact2 = new Fact();
                        fact2.Name = "High School";
                        fact2.TheFact = "I went to Oconomowoc High School";
                        await SaveFactAsync(fact2);

                        Fact fact3 = new Fact();
                        fact3.Name = "College";
                        fact3.TheFact = "I am currently attending WCTC";
                        await SaveFactAsync(fact3);

                        Fact fact4 = new Fact();
                        fact4.Name = "Pets";
                        fact4.TheFact = "I have 3 cats named Jack, Gavin, and Tessa";
                        await SaveFactAsync(fact4);

                        Fact fact5 = new Fact();
                        fact5.Name = "Hobbys";
                        fact5.TheFact = "I like to play video games and watch movies and tv shows";
                        await SaveFactAsync(fact5);
                    }
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
