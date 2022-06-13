using MvvmCross.IoC;
using MainViewModel = SBTodo.Core.ViewModels.Main.MainViewModel;

namespace SBTodo.Core;

public class App:MvxApplication
{
    public override void Initialize()
    {
        CreatableTypes()
            .EndingWith("Service")
            .AsInterfaces()
            .RegisterAsLazySingleton();

        RegisterAppStart<MainViewModel>();
    }
}

