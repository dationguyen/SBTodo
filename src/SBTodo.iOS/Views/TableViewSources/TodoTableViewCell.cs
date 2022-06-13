using System;
using System.Drawing;
using Cirrious.FluentLayouts.Touch;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross.Platforms.Ios.Binding.Views;
using SBTodo.Core.Models;
using SBTodo.iOS.Styles;
using UIKit;

namespace SBTodo.iOS.Views.TableViewSources
{
    public class TodoTableViewCell : MvxTableViewCell
    {
        private UILabel _lblTodoMessage;
        private UISwitch _switchCompleted;
        private UIButton _btnDelete;

        public TodoTableViewCell(IntPtr handle) : base(handle)
        {
            CreateView();
            LayoutView();
            BindView();
        }

        public override void PrepareForReuse()
        {
            base.PrepareForReuse();
            _switchCompleted.SetState(false, false);
        }

        private void BindView()
        {
            this.DelayBind(() =>
            {
                MvxFluentBindingDescriptionSet<TodoTableViewCell, TodoItem> bindingSet = this.CreateBindingSet<TodoTableViewCell,TodoItem>();
                bindingSet.Bind(_lblTodoMessage).For(lbl => lbl.Text).To(vm => vm.Todo);
                bindingSet.Bind(_switchCompleted).For(sw => sw.On).To(vm => vm.Completed);
                bindingSet.Bind(_switchCompleted).For(sw => sw.BindTouchUpInside()).To(vm => vm.EditCommand);
                bindingSet.Bind(_btnDelete).For(sw => sw.BindTouchUpInside()).To(vm => vm.DeleteCommand);

                bindingSet.Apply();
            });
        }

        private void LayoutView()
        {
            ContentView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
            ContentView.AddConstraints(new FluentLayout[]
            {
                _lblTodoMessage.AtLeftOf(ContentView),
                _lblTodoMessage.AtTopOf(ContentView),
                _lblTodoMessage.AtBottomOf(ContentView),
                _lblTodoMessage.ToLeftOf(_switchCompleted),

                _switchCompleted.WithSameCenterY(ContentView),
                _switchCompleted.ToRightOf(_lblTodoMessage),
                _switchCompleted.ToLeftOf(_btnDelete, 10f),

                _btnDelete.AtTopOf(ContentView),
                _btnDelete.AtBottomOf(ContentView),
                _btnDelete.ToRightOf(_switchCompleted, 10f),
                _btnDelete.AtRightOf(ContentView)
            });
        }

        private void CreateView()
        {
            _lblTodoMessage = new UILabel();
            ContentView.Add(_lblTodoMessage);

            _switchCompleted = new UISwitch();
            ContentView.Add(_switchCompleted);

            _btnDelete = new UIButton();
            _btnDelete.SetTitle("Delete", UIControlState.Normal);
            _btnDelete.SetTitleColor(ColorPalette.SecondaryText, UIControlState.Disabled);
            _btnDelete.SetTitleColor(UIColor.Red, UIControlState.Normal);
            ContentView.Add(_btnDelete);
        }
    }
}
