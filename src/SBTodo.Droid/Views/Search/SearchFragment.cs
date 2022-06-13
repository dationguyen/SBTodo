using Android.OS;
using Android.Views;
using AndroidX.AppCompat.Widget;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using SBTodo.Core.ViewModels.Main;
using SBTodo.Core.ViewModels.Search;
using SBTodo.Droid.Views.Base;

namespace SBTodo.Droid.Views.ItemDetail
{
    [MvxFragmentPresentation(typeof(MainContainerViewModel), Resource.Id.content_frame, addToBackStack:true)]
    public class SearchFragment : BaseFragmentWithBackButton<SearchViewModel>
    {
        protected override int FragmentLayoutId => Resource.Layout.fragment_search;
    }
}
