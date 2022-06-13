using MvvmCross.Navigation;
using SBTodo.Core.Services;
using SBTodo.Core.ViewModels.Base;

namespace SBTodo.Core.ViewModels;

public partial class ItemDetailViewModel : BaseViewModel<TodoItem>
{
    private readonly ITodoDataService _todoDataService;
    
    public ItemDetailViewModel(ITodoDataService todoDataService)
    {
        _todoDataService = todoDataService;
    }

    [ObservableProperty]
    private TodoItem _todoItem;

    public override void Prepare(TodoItem parameter)
    {
        TodoItem = parameter;
    }

    public override void ViewDisappearing()
    {
        base.ViewDisappearing();
        _todoDataService.SaveItemAsync(TodoItem);
    }
}
