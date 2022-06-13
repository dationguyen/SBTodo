using Android.App;
using Android.OS;
using Android.Views;
using SBTodo.Core.ViewModels.Main;
using SBTodo.Droid.Views.Base;

namespace SBTodo.Droid.Views.Main
{
    [Activity(
        Theme = "@style/AppTheme",
        WindowSoftInputMode = SoftInput.AdjustResize | SoftInput.StateHidden)]
    public class MainContainerActivity : BaseActivity<MainContainerViewModel>
    {
        protected override int ActivityLayoutId => Resource.Layout.activity_main_container;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            // SetSupportActionBar(FindViewById<Toolbar>(Resource.Id.toolbar));
        }
    }
}
