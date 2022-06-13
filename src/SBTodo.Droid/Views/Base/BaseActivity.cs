using Android.OS;
using MvvmCross.Platforms.Android.Views;
using MvvmCross.ViewModels;

namespace SBTodo.Droid.Views.Base
{
    public abstract class BaseActivity<TViewModel> : MvxActivity<TViewModel>
        where TViewModel : class, IMvxViewModel
    {
        protected abstract int ActivityLayoutId { get; }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(ActivityLayoutId);
        }
    }
}
