using Android.OS;
using Android.Views;
using AndroidX.AppCompat.Widget;
using MvvmCross.ViewModels;

namespace SBTodo.Droid.Views.Base;

public abstract class BaseFragmentWithBackButton<TViewModel> : BaseFragment<TViewModel>
    where TViewModel : class, IMvxViewModel
{
    private Toolbar _toolbar;

    public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
    {
        View view = base.OnCreateView(inflater, container, savedInstanceState);
        
        _toolbar = Activity?.FindViewById<Toolbar>(Resource.Id.toolbar);
       
        return view;
    }
    
    public override void OnStart()
    {
        base.OnStart();
        if (_toolbar != null)
        {
            // add back button (hacked way)
            _toolbar.SetNavigationIcon(Resource.Drawable.abc_ic_ab_back_material);
            _toolbar.NavigationClick += Toolbar_NavigationOnClick;
        }
    }

    public override void OnStop()
    {
        base.OnStop();
        if (_toolbar != null)
        {
            _toolbar.NavigationIcon = null;
            _toolbar.NavigationClick -= Toolbar_NavigationOnClick;
        }
    }

    private void Toolbar_NavigationOnClick(object sender, System.EventArgs e)
    {
        Activity?.OnBackPressed();
    }
}
