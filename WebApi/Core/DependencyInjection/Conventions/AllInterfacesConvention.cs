using System.Linq;
using BaselineTypeDiscovery;
using Lamar;
using Lamar.Scanning.Conventions;

namespace WebApi.Core.DependencyInjection.Conventions
{
    public class AllInterfacesConvention : IRegistrationConvention
    {
        public void ScanTypes(TypeSet types, ServiceRegistry services)
        {
            types
                .FindTypes(TypeClassification.Concretes | TypeClassification.Closed)
                .ToList()
                .ForEach(type =>
                {
                    // Register against all the interfaces implemented
                    // by this concrete class
                    type.GetInterfaces().ToList().ForEach(@interface => services.For(@interface).Use(type));
                });
        }
    }
}