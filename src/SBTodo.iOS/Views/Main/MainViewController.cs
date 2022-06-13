using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Cirrious.FluentLayouts.Touch;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using SBTodo.Core.ViewModels;
using SBTodo.Core.ViewModels.Main;
using SBTodo.iOS.Styles;
using SBTodo.iOS.Views.TableViewSources;
using UIKit;

namespace SBTodo.iOS.Views.Main
{
    [MvxRootPresentation(WrapInNavigationController = true)]
    public class MainViewController : BaseViewController<MainViewModel>
    {
        private UILabel _lblShowCompleted;
        private UIButton _buttonAddTodo, _buttonSearch;
        private UITextField _txtTodoInput;
        private UITableView _tableViewTodoList;
        private UISwitch _switchShowCompleted;

        protected override void CreateView()
        {
            _buttonAddTodo = new UIButton
            {
                BackgroundColor = ColorPalette.Primary
            };
            _buttonAddTodo.SetTitle("Add Todo", UIControlState.Normal);
            _buttonAddTodo.SetTitle("<-- Todo", UIControlState.Disabled);
            _buttonAddTodo.SetTitleColor(ColorPalette.SecondaryText, UIControlState.Disabled);
            _buttonAddTodo.SetTitleColor(ColorPalette.PrimaryText, UIControlState.Normal);
            Add(_buttonAddTodo);

            _buttonSearch = new UIButton
            {
                BackgroundColor = ColorPalette.Primary
            };
            _buttonSearch.SetTitle("Search", UIControlState.Normal);
            _buttonSearch.SetTitleColor(ColorPalette.SecondaryText, UIControlState.Disabled);
            _buttonSearch.SetTitleColor(ColorPalette.PrimaryText, UIControlState.Normal);
            Add(_buttonSearch);

            _txtTodoInput = new UITextField
            {
                BorderStyle = UITextBorderStyle.Line
            };
            Add(_txtTodoInput);

            _tableViewTodoList = new UITableView();
            Add(_tableViewTodoList);

            _switchShowCompleted = new UISwitch();
            Add(_switchShowCompleted);

            _lblShowCompleted = new UILabel
            {
                Text = "Show Completed"
            };
            Add(_lblShowCompleted);

        }

        protected override void LayoutView()
        {
            View.AddConstraints(new FluentLayout[]
            {
                _buttonAddTodo.AtTopOf(View, 10f),
                _buttonAddTodo.AtRightOf(View, 10f),
                _buttonAddTodo.Width().EqualTo(100f),

                _txtTodoInput.AtTopOf(View, 10f),
                _txtTodoInput.AtLeftOf(View, 10f),
                _txtTodoInput.ToLeftOf(_buttonAddTodo, 10f),
                _txtTodoInput.WithSameHeight(_buttonAddTodo),

                _tableViewTodoList.Below(_buttonAddTodo, 10f),
                _tableViewTodoList.AtLeftOf(View, 10f),
                _tableViewTodoList.AtRightOf(View, 10f),
                _tableViewTodoList.Above(_switchShowCompleted, 10f),

                _switchShowCompleted.Below(_tableViewTodoList, 10f),
                _switchShowCompleted.AtRightOf(View, 10f),
                _switchShowCompleted.ToRightOf(_lblShowCompleted, 10f),
                _switchShowCompleted.AtBottomOf(View, 10f),

                _lblShowCompleted.Below(_tableViewTodoList, 10f),
                _lblShowCompleted.ToLeftOf(_switchShowCompleted, 10f),
                _lblShowCompleted.AtBottomOf(View, 10f),

                _buttonSearch.Below(_tableViewTodoList, 10f),
                _buttonSearch.AtLeftOf(View, 10f),
                _buttonSearch.AtBottomOf(View, 10f),
                _buttonSearch.Width().EqualTo(100f),
            });
        }

        protected override void BindView()
        {
            base.BindView();

            MvxFluentBindingDescriptionSet<IMvxIosView<MainViewModel>, MainViewModel> bindingSet = CreateBindingSet();

            bindingSet.Bind(_txtTodoInput).For(txt => txt.Text).To(vm => vm.ToDoInput);
            bindingSet.Bind(_buttonAddTodo).For(btn => btn.BindTouchUpInside()).To(vm => vm.AddTodoCommand);
            bindingSet.Bind(_buttonSearch).For(btn => btn.BindTouchUpInside()).To(vm => vm.SearchCommand);
            bindingSet.Bind(_switchShowCompleted).For(sw => sw.BindTouchUpInside()).To(vm => vm.ToggleShowCompletedCommand);
            bindingSet.Bind(_switchShowCompleted).For(sw => sw.On).To(vm => vm.ShowCompletedItem);

            var source = new TodoTableViewSource(_tableViewTodoList);
            bindingSet.Bind(source).For(s=>s.ItemsSource).To(vm => vm.TodoCollection);
            bindingSet.Bind(source).For(s=>s.SelectionChangedCommand).To(vm => vm.TodoItemClickedCommand);

            bindingSet.Apply();

            _tableViewTodoList.Source = source;
            _tableViewTodoList.RowHeight = 40f;
            _tableViewTodoList.ReloadData();
        }
    }
}
