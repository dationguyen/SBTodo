using System.Collections.ObjectModel;
using MvvmCross.Navigation;
using SBTodo.Core.Services;
using SBTodo.Core.ViewModels.Base;
using SBTodo.Core.ViewModels.Search;

namespace SBTodo.Core.ViewModels.Main;

public partial class MainViewModel : BaseViewModel
{
    private readonly ITodoDataService _todoDataService;
    private readonly IMvxNavigationService _navigationService;
    
    public MainViewModel(ITodoDataService todoDataService, IMvxNavigationService navigationService)
    {
        _todoDataService = todoDataService;
        _navigationService = navigationService;
    }

    #region ObservableProperties -- Setup Properties

    [ObservableProperty]
    private ObservableCollection<TodoItem> _todoCollection;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AddTodoCommand))]
    private string _toDoInput;

    [ObservableProperty]
    private bool _showCompletedItem = true;
    
    #endregion


    #region ICommands - All Commands of this Viewmodel

    /// <summary>
    /// Toggle show completed item
    /// </summary>
    /// <returns></returns>
    [RelayCommand] 
    private async Task ToggleShowCompletedAsync()
    {
         await ShowHideCompletedTodoItemAsync();
    }

    /// <summary>
    /// Add new Todo_item
    /// </summary>
    /// <returns></returns>
    [RelayCommand(CanExecute = nameof(CanExecuteAddTodo))] 
    private async Task AddTodoAsync()
    {
        // Create new TodoItem
        var item = TodoItem.Create(ToDoInput, DeleteTodoCommand, EditTodoCommand);

        // Add to db
        await _todoDataService.SaveItemAsync(item);

        // Manually add the item to local field instead of reInit from db
        TodoCollection.Add(item);

        // Clean up input field
        ToDoInput = string.Empty;
    }
    private bool CanExecuteAddTodo()
    {
        // If input is null or empty or white space the AddTodoCommand will be blocked
        return !string.IsNullOrWhiteSpace(ToDoInput);
    }
    
    /// <summary>
    /// delete todo_item
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    [RelayCommand] 
    private async Task DeleteTodoAsync(TodoItem item)
    {
        // Delete from db
        await _todoDataService.DeleteItemAsync(item);

        // Manually delete the item on local field instead of reInit from db
        TodoCollection.Remove(item);
    }

    /// <summary>
    /// edit todo_item
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    [RelayCommand] 
    private async Task EditTodoAsync(TodoItem item)
    {
        // Update item to db
        await _todoDataService.SaveItemAsync(item);

        // Refresh the view to Show/hide completed item after update
        await ShowHideCompletedTodoItemAsync();
    }

    /// <summary>
    /// Item clicked
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    [RelayCommand]
    private async Task TodoItemClickedAsync(TodoItem item)
    {
        await _navigationService.Navigate<ItemDetailViewModel, TodoItem>(item);
    }

    /// <summary>
    /// Search button clicked
    /// </summary>
    /// <returns></returns>
    [RelayCommand]
    private async Task SearchAsync()
    {
        await _navigationService.Navigate<SearchViewModel>();
    }

    #endregion 

    /// <summary>
    /// Fill data to the view on first load
    /// Or Update data when navigate back
    /// </summary>
    public override void ViewAppearing()
    {
        base.ViewAppearing();
        
        var t = Task.Run(async () =>
        {
            await ShowHideCompletedTodoItemAsync();
        });
        t.Wait();
    }

    /// <summary>
    /// Get all todo_item from data services and load it to TodoCollection
    /// </summary>
    /// <returns></returns>
    private async Task ShowAllToDoItemsAsync()
    {
        // read from db
        TodoCollection = new ObservableCollection<TodoItem>(await _todoDataService.GetItemsAsync());

        SetupTodoCommand();
    }

    /// <summary>
    /// Get all todo_item that are not completed from data services and load it to TodoCollection
    /// </summary>
    /// <returns></returns>
    private async Task HideCompletedItemAsync()
    {
        // Read from db
        TodoCollection = new ObservableCollection<TodoItem>(await _todoDataService.GetNotCompletedItemsAsync());

        SetupTodoCommand();
    }

    /// <summary>
    /// Map command of the item to this ViewModel
    /// </summary>
    private void SetupTodoCommand()
    {
        // Sync item commands
        foreach (TodoItem item in TodoCollection)
        {
            item.DeleteCommand = new AsyncRelayCommand(()=> DeleteTodoCommand.ExecuteAsync(item));
            item.EditCommand = new AsyncRelayCommand(()=> EditTodoCommand.ExecuteAsync(item));
        }
    }

    /// <summary>
    /// check ShowCompletedItem property and display the item 
    /// </summary>
    /// <returns></returns>
    private async Task ShowHideCompletedTodoItemAsync()
    {
        if (_showCompletedItem)
        {
            await ShowAllToDoItemsAsync();
        }
        else
        {
            await HideCompletedItemAsync();
        }
    }
}
