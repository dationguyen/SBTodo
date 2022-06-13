using Microsoft.Extensions.Logging;
using MvvmCross.Platforms.Android.Core;
using SBTodo.Core;
using Serilog;
using Serilog.Extensions.Logging;

namespace SBTodo.Droid
{
    public class Setup : MvxAndroidSetup<App>
    {
        protected override ILoggerProvider CreateLogProvider() => new SerilogLoggerProvider();

        protected override ILoggerFactory CreateLogFactory()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.AndroidLog()
                .CreateLogger();

            return new SerilogLoggerFactory();
        }
    }
}
