using Cirrious.FluentLayouts.Touch;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using SBTodo.Core.ViewModels;
using SBTodo.Core.ViewModels.Main;
using SBTodo.Core.ViewModels.Search;
using SBTodo.iOS.Styles;
using SBTodo.iOS.Views.TableViewSources;
using UIKit;

namespace SBTodo.iOS.Views.ItemDetail
{
    [MvxChildPresentation]
    public class SearchViewController : BaseViewController<SearchViewModel>
    {
        private UIButton _buttonSearch;
        private UITextField _txtSearchInput;
        private UITableView _tableViewTodoList;

        protected override void CreateView()
        {
            _txtSearchInput = new UITextField
            {
                BorderStyle = UITextBorderStyle.Line
            };
            Add(_txtSearchInput);

            _buttonSearch = new UIButton
            {
                BackgroundColor = ColorPalette.Primary
            };
            _buttonSearch.SetTitle("Search", UIControlState.Normal);
            _buttonSearch.SetTitleColor(ColorPalette.SecondaryText, UIControlState.Disabled);
            _buttonSearch.SetTitleColor(ColorPalette.PrimaryText, UIControlState.Normal);
            Add(_buttonSearch);

            _tableViewTodoList = new UITableView();
            Add(_tableViewTodoList);
        }

        protected override void LayoutView()
        {
            View.AddConstraints(new FluentLayout[]
            {
                _buttonSearch.AtTopOf(View, 10f),
                _buttonSearch.AtRightOf(View, 10f),
                _buttonSearch.Width().EqualTo(100f),

                _txtSearchInput.AtTopOf(View, 10f),
                _txtSearchInput.AtLeftOf(View, 10f),
                _txtSearchInput.ToLeftOf(_buttonSearch, 10f),
                _txtSearchInput.WithSameHeight(_buttonSearch),

                _tableViewTodoList.Below(_buttonSearch, 10f),
                _tableViewTodoList.AtLeftOf(View, 10f),
                _tableViewTodoList.AtRightOf(View, 10f),
                _tableViewTodoList.AtBottomOf(View, 10f),
            });
        }

        protected override void BindView()
        {
            base.BindView();

            MvxFluentBindingDescriptionSet<IMvxIosView<SearchViewModel>, SearchViewModel> bindingSet = CreateBindingSet();

            bindingSet.Bind(_txtSearchInput).For(txt => txt.Text).To(vm => vm.SearchInput);
            bindingSet.Bind(_buttonSearch).For(btn => btn.BindTouchUpInside()).To(vm => vm.SearchCommand);

            var source = new TodoTableViewSource(_tableViewTodoList);
            bindingSet.Bind(source).For(s=>s.ItemsSource).To(vm => vm.TodoCollection);
            bindingSet.Bind(source).For(s=>s.SelectionChangedCommand).To(vm => vm.TodoItemClickedCommand);

            bindingSet.Apply();

            _tableViewTodoList.Source = source;
            _tableViewTodoList.RowHeight = 40f;
            _tableViewTodoList.ReloadData();

            bindingSet.Apply();
        }
    }
}
