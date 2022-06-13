using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using SBTodo.Core.Models;
using SBTodo.Core.Services;

namespace SBTodo.Core.Test.ServiceTest;

[TestFixture]
public class TodoDataServiceTest
{
    private readonly ITodoDataService _dataService = new TodoDataService();
    //test data
    private readonly List<TodoItem> _mockData = new List<TodoItem>()
    {
        new TodoItem(){Todo = "Todo 1" , Completed = true},
        new TodoItem(){Todo = "Todo 2" , Completed = false},
        new TodoItem(){Todo = "Todo 3" , Completed = true},
        new TodoItem(){Todo = "Todo 4" , Completed = false}
    };

    [Test]
    public async Task SaveItems_AddNewDataAsync()
    {
        // Arrange
        List<TodoItem> mockResult = await CreateMockAsync();
        
        // Act
        await _dataService.SaveItemAsync(_mockData[0]);
        

        List<TodoItem> after = await _dataService.GetItemsAsync();

        // Assert
        Assert.AreEqual(after.Count - mockResult.Count, 1);
    }

    [Test]
    public async Task SaveItems_UpdateData_Async()
    {
        // Arrange
        List<TodoItem> mockResult = await CreateMockAsync();
        var updatedString = "Updated test";
        TodoItem mockItem = mockResult[0];
        mockItem.Todo = updatedString;

        // Act
        await _dataService.SaveItemAsync(mockItem);

        List<TodoItem> after = await _dataService.GetItemsAsync();
        
        // Assert
        Assert.AreEqual(after[0].Todo, updatedString);
    }
    
    [Test]
    public async Task DeleteItem_Valid_Async()
    {
        // Arrange
        List<TodoItem> mockResult = await CreateMockAsync();

        TodoItem mockItem = mockResult[0];

        // Act
        await _dataService.DeleteItemAsync(mockItem);

        List<TodoItem> after = await _dataService.GetItemsAsync();
        
        // Assert
        Assert.AreEqual(after.Exists((item) => item.Id == mockItem.Id), false);
    }

    [Test]
    public async Task DeleteItem_Invalid_Async()
    {
        // Arrange
        List<TodoItem> mockResult = await CreateMockAsync();

        TodoItem mockItem = _mockData[0];

        // Act
        await _dataService.DeleteItemAsync(mockItem);

        List<TodoItem> after = await _dataService.GetItemsAsync();
        
        // Assert
        Assert.AreEqual(after.Count == mockResult.Count, true);
    }

    [TestCase("Todo")]
    [TestCase("Todo 1")]
    [TestCase("1")]
    public async Task Search_Valid_Async(string key)
    {
        // Arrange
        List<TodoItem> mockResult = await CreateMockAsync();
        
        // Act
        List<TodoItem> result = await _dataService.SearchItemsAsync(key);
        
        // Assert
        Assert.AreEqual(result.Count > 0, true);
    }

    [TestCase("0")]
    [TestCase("   ")]
    [TestCase("nah")]
    public async Task Search_Invalid_Async(string key)
    {
        // Arrange
        List<TodoItem> mockResult = await CreateMockAsync();
        
        // Act
        List<TodoItem> result = await _dataService.SearchItemsAsync(key);
        
        // Assert
        Assert.AreEqual(result.Count > 0, false);
    }

    [Test]
    public async Task GetNotCompleted_Async()
    {
        // Arrange
        List<TodoItem> mockResult = await CreateMockAsync();
        
        // Act
        List<TodoItem> result = await _dataService.GetNotCompletedItemsAsync();
        
        // Assert
        Assert.AreEqual(result.Exists((item)=> item.Completed), false);
    }

    [Test]
    public async Task GetItemById_ValidId_Async()
    {
        // Arrange
        List<TodoItem> mockResult = await CreateMockAsync();
        
        // Act
        TodoItem result = await _dataService.GetItemAsync(mockResult[0].Id);
        
        // Assert
        Assert.NotNull(result);
    }

    private async Task<List<TodoItem>> CreateMockAsync()
    {
        List<TodoItem> before = await _dataService.GetItemsAsync();
        if (before.Count == 0)
        {
            // -Add Mock Data
            foreach (TodoItem item in _mockData)
            {
                await _dataService.SaveItemAsync(item);
            }
        }
        return await _dataService.GetItemsAsync();
    }
}
