using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using NUnit.Framework;
using SBTodo.Core.Models;
using SBTodo.Core.Services;
using SBTodo.Core.ViewModels;
using SBTodo.Core.ViewModels.Main;
using SBTodo.Core.ViewModels.Search;

namespace SBTodo.Core.Test.ViewModelTest;

[TestFixture]
public class MainViewModelTest
{
    private Mock<ITodoDataService> _todoDataServiceMock;
    private Mock<IMvxNavigationService > _navigationServiceMock;

    #region AddTodo

    [TestCase("Todo 1 for smoke ball")]
    [TestCase("Add new ball")]
    [TestCase("There is no smoke without a fire")]
    public async Task AddNewValidTodo_CanExecuteCommandAsync(string todo)
    {
        // Arrange
        MainViewModel viewModel = await InitMainViewModelWithEmptyTodoListAsync();

        //Act
        viewModel.ToDoInput = todo;

        //Assert
        Assert.AreEqual(viewModel.AddTodoCommand.CanExecute(null), true);
    }

    [TestCase("Todo 1 for smoke ball")]
    [TestCase("Add new ball")]
    [TestCase("There is no smoke without a fire")]
    public async Task AddNewValidTodo_NewTodoAppearAsync(string todo)
    {
        // Arrange
        MainViewModel viewModel = await InitMainViewModelWithEmptyTodoListAsync();

        //Act
        viewModel.ToDoInput = todo;

        if (viewModel.AddTodoCommand.CanExecute(null))
        {
            await viewModel.AddTodoCommand.ExecuteAsync(null);
        }

        //Assert
        Assert.AreEqual(viewModel.TodoCollection.ToList().Exists((item) => item.Todo == todo), true);
    }
    
    [TestCase("")]
    [TestCase("       ")]
    [TestCase(null)]
    public async Task AddNewInvalidTodo_CanNotExecuteCommandAsync(string todo)
    {
        // Arrange
        MainViewModel viewModel = await InitMainViewModelWithEmptyTodoListAsync();

        //Act
        viewModel.ToDoInput = todo;

        //Assert
        Assert.AreEqual(viewModel.AddTodoCommand.CanExecute(null), false);
    }

    #endregion

    #region EditTodo

    [TestCase]
    public async Task EditTodo_DoAddNewRecordAsync()
    {
        // Arrange
        MainViewModel viewModel = await InitMainViewModelWithDataTodoListAsync();

        var itemToEdit = new TodoItem() { Id = 4, Todo = "Todo 4", Completed = false };

        //Act
        await viewModel.EditTodoCommand.ExecuteAsync(itemToEdit);

        //Assert
        _todoDataServiceMock.Verify(x=> x.SaveItemAsync(itemToEdit),Times.Once);
    }

    #endregion

    #region RemoveTodo

    [Test]
    public async Task DeleteTodoCommand_DoDeleteItemAsync()
    {
        // Arrange
        MainViewModel viewModel = await InitMainViewModelWithDataTodoListAsync();

        var itemToRemove = new TodoItem() { Id = 4, Todo = "Todo 4", Completed = false };

        //Act
        await viewModel.DeleteTodoCommand.ExecuteAsync(itemToRemove);

        //Assert
        _todoDataServiceMock.Verify(x=> x.DeleteItemAsync(itemToRemove),Times.Once);
    }

    #endregion

    #region ItemClicked

    [Test]
    public async Task TodoItemClickedCommand_DoNavigateToItemDetailAsync()
    {
        // Arrange
        MainViewModel viewModel = await InitMainViewModelWithDataTodoListAsync();

        var item = new TodoItem() { Id = 4, Todo = "Todo 4", Completed = false };

        //Act
        await viewModel.TodoItemClickedCommand.ExecuteAsync(item);

        //Assert
        _navigationServiceMock.Verify(x => x.Navigate<ItemDetailViewModel, TodoItem>(item, It.IsAny<IMvxBundle>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region Toggle Show/hide Completed Item

    [TestCase]
    public async Task ToggleShowHideCompletedTodo_HideCompletedAsync()
    {
        // Arrange
        MainViewModel viewModel = await InitMainViewModelWithDataTodoListAsync();

        //Act
        viewModel.ShowCompletedItem = false;
        await viewModel.ToggleShowCompletedCommand.ExecuteAsync(null);

        //Assert
        _todoDataServiceMock.Verify(x=> x.GetNotCompletedItemsAsync(),Times.Once);
    }

    [TestCase()]
    public async Task ToggleShowHideCompletedTodo_ShowCompletedAsync()
    {
        // Arrange
        MainViewModel viewModel = await InitMainViewModelWithDataTodoListAsync();

        //Act
        viewModel.ShowCompletedItem = true;
        await viewModel.ToggleShowCompletedCommand.ExecuteAsync(null);

        //Assert
        _todoDataServiceMock.Verify(x=> x.GetNotCompletedItemsAsync(),Times.Never);
    }

    #endregion

    #region Search

    [Test]
    public async Task Search_DidNavigateToSearchPageAsync()
    {
        // Arrange
        MainViewModel viewModel = await InitMainViewModelWithDataTodoListAsync();

        //Act
        await viewModel.SearchCommand.ExecuteAsync(null);

        //Assert
        _navigationServiceMock.Verify(x => x.Navigate<SearchViewModel>(It.IsAny<IMvxBundle>(), It.IsAny<CancellationToken>()), Times.Once);

    }

    #endregion

    #region Child Item execute Delete

    [Test]
    public async Task DeleteTodoCommand_DoDeleteItemFromChildAsync()
    {
        // Arrange
        MainViewModel viewModel = await InitMainViewModelWithDataTodoListAsync();

        TodoItem itemToRemove = viewModel.TodoCollection[0];

        //Act
        itemToRemove.DeleteCommand.Execute(null);

        //Assert
        _todoDataServiceMock.Verify(x=> x.DeleteItemAsync(itemToRemove),Times.Once);
    }
    #endregion

    #region Child Item execute edit

    [Test]
    public async Task EditTodoCommand_DoEditItemFromChildAsync()
    {
        // Arrange
        MainViewModel viewModel = await InitMainViewModelWithDataTodoListAsync();

        TodoItem itemToEdit = viewModel.TodoCollection[0];

        //Act
        itemToEdit.EditCommand.Execute(null);

        //Assert
        _todoDataServiceMock.Verify(x=> x.SaveItemAsync(itemToEdit),Times.Once);
    }
    #endregion

    #region Mock Setup

    public async Task<MainViewModel> InitMainViewModelWithEmptyTodoListAsync()
    {
        _todoDataServiceMock = new Mock<ITodoDataService>();

        // Mock GetItemsAsync => new List<TodoItem>() 
        _todoDataServiceMock.Setup(x => x.GetItemsAsync()).Returns(Task.FromResult(new List<TodoItem>()));

        _navigationServiceMock = new Mock<IMvxNavigationService>();

        var viewModel = new MainViewModel(_todoDataServiceMock.Object, _navigationServiceMock.Object);
        await viewModel.Initialize();
        viewModel.ViewAppearing();

        return viewModel;
    }

    public async Task<MainViewModel> InitMainViewModelWithDataTodoListAsync()
    {
        _todoDataServiceMock = new Mock<ITodoDataService>();
        // Mock  GetItemsAsync => List of todo
        _todoDataServiceMock.Setup(x => x.GetItemsAsync()).Returns(Task.FromResult(new List<TodoItem>()
        {
            new TodoItem(){Id = 1, Todo = "Todo 1" , Completed = true},
            new TodoItem(){Id = 2, Todo = "Todo 2" , Completed = false},
            new TodoItem(){Id = 3, Todo = "Todo 3" , Completed = true},
            new TodoItem(){Id = 4, Todo = "Todo 4" , Completed = false}
        }));

        // Mock  GetNotCompletedItemsAsync => List of todo
        _todoDataServiceMock.Setup(x => x.GetNotCompletedItemsAsync()).Returns(Task.FromResult(new List<TodoItem>()
        {
            new TodoItem(){Id = 2, Todo = "Todo 2" , Completed = false},
            new TodoItem(){Id = 4, Todo = "Todo 4" , Completed = false}
        }));

        _navigationServiceMock = new Mock<IMvxNavigationService>();

        var viewModel = new MainViewModel(_todoDataServiceMock.Object, _navigationServiceMock.Object);
        await viewModel.Initialize();
        viewModel.ViewAppearing();

        return viewModel;
    }

    #endregion


}
