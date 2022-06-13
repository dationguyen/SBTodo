using Cirrious.FluentLayouts.Touch;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using SBTodo.Core.ViewModels;
using SBTodo.Core.ViewModels.Main;
using SBTodo.iOS.Styles;
using SBTodo.iOS.Views.TableViewSources;
using UIKit;

namespace SBTodo.iOS.Views.ItemDetail
{
    [MvxChildPresentation]
    public class ItemDetailViewController : BaseViewController<ItemDetailViewModel>
    {
        private UITextField _txtTodo;
        private UISwitch _switchCompleted;
        private UILabel _lblDateCreated;
        private UILabel _lblDateModified;

        private UILabel _lblTxtDateCreated;
        private UILabel _lblTxtDateModified;
        private UILabel _lblTxtCompleted;

        protected override void CreateView()
        {
            _txtTodo = new UITextField
            {
                BorderStyle = UITextBorderStyle.Line
            };
            Add(_txtTodo);

            _switchCompleted = new UISwitch();
            Add(_switchCompleted);

            _lblDateCreated = new UILabel();
            Add(_lblDateCreated);

            _lblDateModified = new UILabel();
            Add(_lblDateModified);

            _lblTxtDateCreated = new UILabel { Text = "Date Created:" };
            Add(_lblTxtDateCreated);

            _lblTxtDateModified = new UILabel { Text = "Date Modified:" };
            Add(_lblTxtDateModified);

            _lblTxtCompleted = new UILabel { Text = "Completed:" };
            Add(_lblTxtCompleted);
        }

        protected override void LayoutView()
        {
            View.AddConstraints(new FluentLayout[]
            {
                _txtTodo.AtTopOf(View, 10f),
                _txtTodo.AtLeftOf(View, 10f),
                _txtTodo.AtRightOf(View, 10f),

                _lblTxtCompleted.Below(_txtTodo, 10f),
                _lblTxtCompleted.AtLeftOf(View, 10f),
                _lblTxtCompleted.ToLeftOf(_switchCompleted, 10f),

                _switchCompleted.Below(_txtTodo, 10f),
                _switchCompleted.ToRightOf(_lblTxtCompleted, 10f),

                _lblTxtDateCreated.Below(_switchCompleted, 10f),
                _lblTxtDateCreated.AtLeftOf(View, 10f),
                _lblTxtDateCreated.ToLeftOf(_lblDateCreated, 10f),

                _lblDateCreated.Below(_switchCompleted, 10f),
                _lblDateCreated.ToRightOf(_lblTxtDateCreated, 10f),

                _lblTxtDateModified.Below(_lblTxtDateCreated, 10f),
                _lblTxtDateModified.AtLeftOf(View, 10f),
                _lblTxtDateModified.ToLeftOf(_lblDateCreated, 10f),

                _lblDateModified.Below(_lblTxtDateCreated, 10f),
                _lblDateModified.ToRightOf(_lblTxtDateModified, 10f),
            });
        }

        protected override void BindView()
        {
            base.BindView();

            MvxFluentBindingDescriptionSet<IMvxIosView<ItemDetailViewModel>, ItemDetailViewModel> bindingSet = CreateBindingSet();

            bindingSet.Bind(_txtTodo).For(txt => txt.Text).To(vm => vm.TodoItem.Todo);
            bindingSet.Bind(_switchCompleted).For(sw => sw.On).To(vm => vm.TodoItem.Completed);
            bindingSet.Bind(_lblDateCreated).For(txt => txt.Text).To(vm => vm.TodoItem.DateCreated);
            bindingSet.Bind(_lblDateModified).For(txt => txt.Text).To(vm => vm.TodoItem.DateModified);

            bindingSet.Apply();

        }
    }
}
