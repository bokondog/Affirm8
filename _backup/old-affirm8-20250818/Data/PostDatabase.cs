using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Affirm8.Models;

namespace Affirm8.Data
{
    public class PostDatabase
    {
        SQLiteAsyncConnection Database;
        public PostDatabase()
        {
        }
        async Task Init()
        {
            if (Database is not null)
                return;

            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            var result = await Database.CreateTableAsync<Post>();
        }
        // Get ALL
        public async Task<List<Post>> GetItemsAsync()
        {
            await Init();
            return await Database.Table<Post>().ToListAsync();
        }
        // GET by ID
        public async Task<Post> GetItemAsync(int id)
        {
            await Init();
            return await Database.Table<Post>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }
        // INSERT/UPDATE
        public async Task<int> SaveItemAsync(Post item)
        {
            await Init();
            if (item.ID != 0)
            {
                return await Database.UpdateAsync(item);
            }
            else
            {
                return await Database.InsertAsync(item);
            }
        }
        // DELETE
        public async Task<int> DeleteItemAsync(Post item)
        {
            await Init();
            return await Database.DeleteAsync(item);
        }
    }
}
