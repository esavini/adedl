using System;
using Microsoft.Extensions.DependencyInjection;

namespace AdeDl.App.Services
{
    public static class ServiceLocator
    {
        private static IServiceProvider ServiceProvider { get; set; }

        public static IServiceProvider Services => ServiceProvider ??= ServiceCollection.BuildServiceProvider()
            .CreateScope().ServiceProvider;

        private static IServiceCollection ServiceCollection { get; } = new ServiceCollection();

        public static void ConfigureServices(Action<IServiceCollection> serviceCollection) => serviceCollection.Invoke(ServiceCollection);
    }
}