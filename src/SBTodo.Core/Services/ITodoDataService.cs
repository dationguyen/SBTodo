using System.Diagnostics.CodeAnalysis;

namespace SBTodo.Core.Services;

public interface ITodoDataService
{
    public Task<List<TodoItem>> GetItemsAsync();

    public Task<int> SaveItemAsync(TodoItem item);

    public Task<int> DeleteItemAsync(TodoItem item);

    public Task<List<TodoItem>> GetNotCompletedItemsAsync();

    public Task<List<TodoItem>> SearchItemsAsync(string key);

    public Task<TodoItem> GetItemAsync(int id);
}
