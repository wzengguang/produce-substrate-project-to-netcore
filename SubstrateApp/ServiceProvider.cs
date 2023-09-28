
namespace SubstrateApp
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using SubstrateApp.ViewModel;

    public class AppServiceProvider
    {
        static private ServiceProvider serviceProvider = null;

        static public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<MainWindowViewModel>();
            serviceCollection.AddSingleton<ProduceAssemblyViewModel>();

            serviceProvider = serviceCollection.BuildServiceProvider();
        }

        public static T GetService<T>()
        {
            return serviceProvider.GetService<T>();
        }

        public static void Dispose()
        {
            serviceProvider.Dispose();
        }
    }
}