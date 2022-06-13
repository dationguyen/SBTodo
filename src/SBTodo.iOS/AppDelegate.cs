using Foundation;
using MvvmCross.Platforms.Ios.Core;
using SBTodo.Core;

namespace SBTodo.iOS
{
    [Register(nameof(AppDelegate))]
    public class AppDelegate : MvxApplicationDelegate<Setup, App>
    {
    }
}
