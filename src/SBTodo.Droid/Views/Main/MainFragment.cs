using MvvmCross.Platforms.Android.Presenters.Attributes;
using SBTodo.Core.ViewModels;
using SBTodo.Core.ViewModels.Main;
using SBTodo.Droid.Views.Base;

namespace SBTodo.Droid.Views.Main
{
    [MvxFragmentPresentation(typeof(MainContainerViewModel), Resource.Id.content_frame)]
    public class MainFragment : BaseFragment<MainViewModel>
    {
        protected override int FragmentLayoutId => Resource.Layout.fragment_main;
    }
}
