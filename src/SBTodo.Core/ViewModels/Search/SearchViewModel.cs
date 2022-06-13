using System.Collections.ObjectModel;
using MvvmCross.Navigation;
using SBTodo.Core.Services;
using SBTodo.Core.ViewModels.Base;

namespace SBTodo.Core.ViewModels.Search;

public partial class SearchViewModel : BaseViewModel
{
    private readonly ITodoDataService _todoDataService;
    private readonly IMvxNavigationService _navigationService;
    
    public SearchViewModel(ITodoDataService todoDataService, IMvxNavigationService navigationService)
    {
        _todoDataService = todoDataService;
        _navigationService = navigationService;
    }

    #region ObservableProperties -- Setup Properties

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SearchCommand))]
    private string _searchInput;

    [ObservableProperty]
    private ObservableCollection<TodoItem> _todoCollection;

    [ObservableProperty]
    private bool _noTodoFound = false;

    #endregion

    #region ICommands - All Commands of this Viewmodel

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
    /// Search for Todo_item that match the SearchInput
    /// </summary>
    /// <returns></returns>
    [RelayCommand(CanExecute = nameof(CanExecuteSearch))] 
    private async Task SearchAsync()
    {
        List<TodoItem> result = await _todoDataService.SearchItemsAsync(SearchInput);
        NoTodoFound = (result == null || result.Count == 0);
        TodoCollection = new ObservableCollection<TodoItem>(result);
        SetupTodoCommand();
    }

    private bool CanExecuteSearch()
    {
        // If input is null or empty or white space the AddTodoCommand will be blocked
        return !string.IsNullOrWhiteSpace(SearchInput);
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

        // Manually delete the item on local field instead of reInit from search result
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
    }

    #endregion

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
}
