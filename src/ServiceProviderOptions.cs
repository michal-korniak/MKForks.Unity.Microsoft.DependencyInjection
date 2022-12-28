using System;
using System.Collections.Generic;

namespace Unity.Microsoft.DependencyInjection
{
    public class ServiceProviderOptions
    {
        internal List<Type> TypesWithPreferedUnityImplementations { get; } = new List<Type>();

        public void PreferUnityImplementation(Type type)
        {
            TypesWithPreferedUnityImplementations.Add(type);
        }

        private ServiceProviderOptions()
        {

        }

        public static ServiceProviderOptions Create(Action<ServiceProviderOptions> options)
        {
            var optionsObject = new ServiceProviderOptions();
            if (options != null)
            {
                options.Invoke(optionsObject);
            }
            return optionsObject;
        }
    }
}