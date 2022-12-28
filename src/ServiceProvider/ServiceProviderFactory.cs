using System;
using Microsoft.Extensions.DependencyInjection;
using Unity.Lifetime;

namespace Unity.Microsoft.DependencyInjection
{
    public class ServiceProviderFactory : IServiceProviderFactory<IUnityContainer>,
                                          IServiceProviderFactory<IServiceCollection>
    {
        #region Fields

        private readonly IUnityContainer _container;
        private readonly ServiceProviderOptions _options;

        #endregion


        #region Constructors

        public ServiceProviderFactory(IUnityContainer container, ServiceProviderOptions options)
        {
            _container = container ?? new UnityContainer();
            _options = options;
            ((UnityContainer)_container).AddExtension(new MdiExtension());

            _container.RegisterInstance<IServiceProviderFactory<IUnityContainer>>(this, new ContainerControlledLifetimeManager());
            _container.RegisterInstance<IServiceProviderFactory<IServiceCollection>>(this, new ExternallyControlledLifetimeManager());
        }

        #endregion


        #region IServiceProviderFactory<IUnityContainer>

        public IServiceProvider CreateServiceProvider(IUnityContainer container)
        {
            return new ServiceProvider(container);
        }

        IUnityContainer IServiceProviderFactory<IUnityContainer>.CreateBuilder(IServiceCollection services)
        {
            return CreateServiceProviderContainer(services);
        }

        #endregion


        #region IServiceProviderFactory<IServiceCollection>

        public IServiceProvider CreateServiceProvider(IServiceCollection containerBuilder)
        {
            return new ServiceProvider(CreateServiceProviderContainer(containerBuilder));
        }

        IServiceCollection IServiceProviderFactory<IServiceCollection>.CreateBuilder(IServiceCollection services)
        {
            return services;
        }

        #endregion


        #region Implementation

        private IUnityContainer CreateServiceProviderContainer(IServiceCollection services)
        {
            var container = _container.CreateChildContainer();
            new ServiceProviderFactory(container, _options);    //This line is crucial, even if returned value is not used

            return ((UnityContainer)container).AddExtension(new MdiExtension())
                                              .AddServices(services, _options.TypesWithPreferedUnityImplementations);
        }

        #endregion
    }
}
