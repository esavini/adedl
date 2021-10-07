using System.Windows;
using AdeDl.App.Services;
using AdeDl.App.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace AdeDl.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public App()
        {
            ServiceLocator.ConfigureServices(ConfigureServices);
        }

        private void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IFileManager, FileManager>();
            serviceCollection.AddSingleton<ILoginService, LoginService>();
            serviceCollection.AddTransient<IBrowserService, BrowserService>();
            serviceCollection.AddTransient<ICuService, CuService>();
            serviceCollection.AddTransient<IF24Service, F24Service>();
            
            serviceCollection.AddTransient<MainWindowViewModel>();
        }
    }
}