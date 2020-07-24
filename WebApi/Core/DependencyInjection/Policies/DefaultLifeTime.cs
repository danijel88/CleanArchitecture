using System;
using System.Linq;
using Lamar;
using Lamar.IoC.Enumerables;
using Lamar.IoC.Instances;
using LamarCodeGeneration.Util;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Core.DependencyInjection.Policies
{
    public class DefaultLifetime : IInstancePolicy, IFamilyPolicy
    {
        public void Apply(Instance instance)
        {
            if (instance.ImplementationType.Namespace != null && (instance.ImplementationType.Namespace.Contains("CleanArchitecture")))
            {
                instance.Lifetime = ServiceLifetime.Scoped;
            }
        }

        public ServiceFamily Build(Type type, ServiceGraph serviceGraph)
        {
            if (type.IsArray)
            {
                var instanceType = typeof(ArrayInstance<>).MakeGenericType(type.GetElementType());
                var instance = Activator.CreateInstance(instanceType, type).As<Instance>();
                return new ServiceFamily(type, new IDecoratorPolicy[0], instance);
            }

            if (type.IsEnumerable())
            {
                var elementType = type.GetGenericArguments().First();

                var instanceType = typeof(ListInstance<>).MakeGenericType(elementType);
                var ctor = instanceType.GetConstructors().Single();
                var instance = ctor.Invoke(new object[] { type }).As<Instance>();

                return new ServiceFamily(type, new IDecoratorPolicy[0], instance);
            }

            return null;
        }
    }
}