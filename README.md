[![NuGet](https://img.shields.io/nuget/v/MKForks.Unity.Microsoft.DependencyInjection.svg)](https://www.nuget.org/packages/MKForks.Unity.Microsoft.DependencyInjection)

#  MKForks.Unity.Microsoft.DependencyInjection

Unity extension to integrate with [Microsoft.Extensions.DependencyInjection](https://github.com/aspnet/DependencyInjection)  compliant systems

## Getting Started

- Reference the ` MKForks.Unity.Microsoft.DependencyInjection` package from NuGet.

```shell
Install-Package  MKForks.Unity.Microsoft.DependencyInjection
```

## Registration:

- In the `builder.Host` add `UseUnityServiceProvider(...)` method

```C#
var builder = WebApplication.CreateBuilder(args);
builder.Host.UseUnityServiceProvider();
```

- In case Unity container configured via application configuration or by convention this container could be used to initialize service provider.

```C#
var builder = WebApplication.CreateBuilder(args);
builder.Host.UseUnityServiceProvider(container);
```

### Resolving Controllers from Unity

By default ASP resolves controllers using built in activator. To enable resolution of controllers from Unity you need to add following line to MVC configuration:

```C#
public void ConfigureServices(IServiceCollection services)
{
    ...
    services.AddMvc()
        .AddControllersAsServices()  //<-- Add this line
        .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
    ...
}
```

## Examples

For example of using Unity with NET 6 Web Api application follow [this link](https://github.com/michal-korniak/MKForks.Unity.Microsoft.DependencyInjection.Example)

## Fork

This project is a fork of https://github.com/unitycontainer/microsoft-dependency-injection. It adds additional options that wasn't available in original library. List of changes

* Possiblity to choose prefered implementation (from default container or Unity). Example below:

```C#
...
unityContainer.RegisterType(typeof(ILogger<>), typeof(FakeLogger<>), new TransientLifetimeManager());
unityContainer.RegisterType(typeof(IService), typeof(Service), new TransientLifetimeManager());
builder.Host.UseUnityServiceProvider(unityContainer, options =>
{
    //By default implementations from Unity are overriden by ASP.NET implementation,
    //we can change it using PreferUnityImplementation method
    options.PreferUnityImplementation(typeof(ILogger<>));
    options.PreferUnityImplementation(typeof(ILogger));
});
...

```

