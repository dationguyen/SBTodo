using MvvmCross.Platforms.Android.Presenters.Attributes;
using SBTodo.Core.ViewModels;
using SBTodo.Core.ViewModels.Main;
using SBTodo.Droid.Views.Base;

namespace SBTodo.Droid.Views.ItemDetail
{
    [MvxFragmentPresentation(typeof(MainContainerViewModel), Resource.Id.content_frame, addToBackStack:true)]
    public class ItemDetailFragment : BaseFragmentWithBackButton<ItemDetailViewModel>
    {
        protected override int FragmentLayoutId => Resource.Layout.fragment_todo_detail;
    }
}
