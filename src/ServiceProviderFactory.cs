using System;
using Microsoft.Extensions.DependencyInjection;
using Unity.Lifetime;

namespace Unity.Microsoft.DependencyInjection
{
    public class ServiceProviderFactory : IServiceProviderFactory<IUnityContainer>,
                                          IServiceProviderFactory<IServiceCollection>
    {
        private readonly IUnityContainer _container;
        private readonly ServiceProviderOptions _options;

        public ServiceProviderFactory(IUnityContainer container, ServiceProviderOptions options)
        {
            _container = container ?? new UnityContainer();
            _options = options;
            _container.RegisterInstance<IServiceProviderFactory<IUnityContainer>>(this, new ContainerControlledLifetimeManager());
            _container.RegisterInstance<IServiceProviderFactory<IServiceCollection>>(this, new ExternallyControlledLifetimeManager());
        }

        public IServiceProvider CreateServiceProvider(IUnityContainer container)
        {
            return new ServiceProvider(container);
        }

        public IServiceProvider CreateServiceProvider(IServiceCollection containerBuilder)
        {
            return new ServiceProvider(CreateServiceProviderContainer(containerBuilder));
        }

        IUnityContainer IServiceProviderFactory<IUnityContainer>.CreateBuilder(IServiceCollection services)
        {
            return CreateServiceProviderContainer(services);
        }

        IServiceCollection IServiceProviderFactory<IServiceCollection>.CreateBuilder(IServiceCollection services)
        {
            return services;
        }


        private IUnityContainer CreateServiceProviderContainer(IServiceCollection services)
        {
            new ServiceProviderFactory(_container, _options);

            return _container.AddExtension(new MdiExtension())
                            .AddServices(services, _options);
        }
    }
}
