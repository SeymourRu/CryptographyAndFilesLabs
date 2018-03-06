using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using Autofac.Core.Activators.Reflection;

namespace CoreDefinitions.Helpers
{
    public class ImplementationSearcher
    {
        public static IEnumerable<Type> GetImplementingTypes<T>(ILifetimeScope scope)
        {
            return scope.ComponentRegistry
                .RegistrationsFor(new TypedService(typeof(T)))
                .Select(x => x.Activator)
                .OfType<ReflectionActivator>()
                .Select(x => x.LimitType).OrderBy(x=>x.Name);
        }
    }
}
