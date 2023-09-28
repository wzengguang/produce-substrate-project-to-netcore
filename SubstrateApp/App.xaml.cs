namespace SubstrateApp
{
    using System.Windows;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            AppServiceProvider.Configure(new ServiceCollection());
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            AppServiceProvider.Dispose();
        }
    }
}
