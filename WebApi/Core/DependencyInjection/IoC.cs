using System;
using Lamar;

namespace WebApi.Core.DependencyInjection
{
    // Mixture from Matt Honeycutt und https://dotnetcoretutorials.com/2018/05/06/servicelocator-shim-for-net-core/
    public class IoC
    {
        private static IServiceProvider _serviceProvider;
        private readonly IServiceProvider _currentServiceProvider;

        public IoC(IServiceProvider currentServiceProvider)
        {
            _currentServiceProvider = (Container)currentServiceProvider;
            Container = (Container)currentServiceProvider;
        }

        public IoC(ServiceRegistry currentServiceProvider)
        {
            Container = new Container(currentServiceProvider);
            _currentServiceProvider = Container;
        }

        public static IContainer Container { get; set; }

        public static IoC Current => new IoC(_serviceProvider);

        public static void SetLocatorProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public object GetInstance(Type serviceType)
        {
            return _currentServiceProvider.GetService(serviceType);
        }

        public TService GetInstance<TService>()
        {
            //return _currentServiceProvider.GetService<TService>();
            return (TService)GetInstance(typeof(TService));
        }
    }
}