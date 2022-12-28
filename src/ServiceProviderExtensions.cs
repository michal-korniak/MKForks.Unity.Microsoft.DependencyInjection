using Microsoft.Extensions.DependencyInjection;
using System;

namespace Unity.Microsoft.DependencyInjection
{
    public static class ServiceProviderExtensions
    {

        /// <summary>
        /// Creates a <see cref="ServiceProvider"/> containing services from the provided <see cref="IServiceCollection"/>
        /// optionally enabling scope validation.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> containing service descriptors.</param>
        /// <param name="validateScopes">
        /// <c>true</c> to perform check verifying that scoped services never gets resolved from root provider; otherwise <c>false</c>.
        /// </param>
        /// <returns>The <see cref="ServiceProvider"/>.</returns>
        public static IServiceProvider BuildServiceProvider(this IServiceCollection services, bool validateScopes = false, Action<ServiceProviderOptions> options = null)
        {
            var optionsObject = ServiceProviderOptions.Create(options);
            return new ServiceProvider(new UnityContainer().AddExtension(new MdiExtension())
                                                           .AddServices(services, optionsObject.TypesWithPreferedUnityImplementations));
        }

        /// <summary>
        /// Creates a <see cref="ServiceProvider"/> containing services from the provided <see cref="IServiceCollection"/>
        /// optionally enabling scope validation.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> containing service descriptors.</param>
        /// <param name="container">Parent container</param>
        /// <returns>Service provider</returns>
        public static IServiceProvider BuildServiceProvider(this IServiceCollection services, IUnityContainer container, Action<ServiceProviderOptions> options = null)
        {
            var optionsObject = ServiceProviderOptions.Create(options);
            return new ServiceProvider(container.AddExtension(new MdiExtension())
                                                .AddServices(services, optionsObject.TypesWithPreferedUnityImplementations));
        }

        /// <summary>
        /// Creates a <see cref="ServiceProvider"/> containing services from the provided <see cref="IServiceCollection"/>
        /// optionally enabling scope validation.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> containing service descriptors.</param>
        /// <param name="container">Parent container</param>
        /// <returns>Service provider</returns>
        public static IServiceProvider BuildServiceProvider(this IUnityContainer container, IServiceCollection services, Action<ServiceProviderOptions> options = null)
        {
            var optionsObject = ServiceProviderOptions.Create(options);
            return new ServiceProvider(container.AddExtension(new MdiExtension())
                                                .AddServices(services, optionsObject.TypesWithPreferedUnityImplementations));
        }
    }
}
