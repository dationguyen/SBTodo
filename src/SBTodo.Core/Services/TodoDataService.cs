using SQLite;

namespace SBTodo.Core.Services;

public class TodoDataService : ITodoDataService
{
    private readonly Lazy<Task<SQLiteAsyncConnection>> _dBConnectionLazy;

    public TodoDataService()
    {
        //Init db in a lazy way as constructors can't be asynchronous
        _dBConnectionLazy = new Lazy<Task<SQLiteAsyncConnection>>(async () =>
        {
            var db = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            await db.CreateTableAsync<TodoItem>();
            await db.EnableWriteAheadLoggingAsync();
            return db;
        });
    }


    public async Task<List<TodoItem>> GetItemsAsync()
    {
        SQLiteAsyncConnection db = await _dBConnectionLazy.Value;
        return await db.Table<TodoItem>().ToListAsync();
    }

    public async Task<int> SaveItemAsync(TodoItem item)
    {
        SQLiteAsyncConnection db = await _dBConnectionLazy.Value;

        if (item.Id != 0)
        {
            item.DateModified = DateTime.Now;
            return await db.UpdateAsync(item);
        }
        else
        {
            return await db.InsertAsync(item);
        }
    }

    public async Task<int> DeleteItemAsync(TodoItem item)
    {
        SQLiteAsyncConnection db = await _dBConnectionLazy.Value;
        return await db.DeleteAsync(item);
    }

    public async Task<List<TodoItem>> GetNotCompletedItemsAsync()
    {
        SQLiteAsyncConnection db = await _dBConnectionLazy.Value;
        return await db.QueryAsync<TodoItem>("SELECT * FROM [TodoItem] WHERE [Completed] = 0");
    }

    public async Task<List<TodoItem>> SearchItemsAsync(string key)
    {
        SQLiteAsyncConnection db = await _dBConnectionLazy.Value;
        return await db.QueryAsync<TodoItem>($"SELECT * FROM [TodoItem] WHERE [Todo] LIKE '%{key}%'");
    }

    public async Task<TodoItem> GetItemAsync(int id)
    {
        SQLiteAsyncConnection db = await _dBConnectionLazy.Value;
        return await db.Table<TodoItem>().Where(i => i.Id == id).FirstOrDefaultAsync();
    }
}
