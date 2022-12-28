[![Build status](https://ci.appveyor.com/api/projects/status/sevk2yb2jokf8ltr/branch/master?svg=true)](https://ci.appveyor.com/project/IoC-Unity/microsoft-dependency-injection/branch/master)
[![License](https://img.shields.io/badge/license-apache%202.0-60C060.svg)](https://github.com/IoC-Unity/microsoft-dependency-injection/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/dt/Unity.Microsoft.DependencyInjection.svg)](https://www.nuget.org/packages/Unity.Microsoft.DependencyInjection)
[![NuGet](https://img.shields.io/nuget/v/Unity.Microsoft.DependencyInjection.svg)](https://www.nuget.org/packages/Unity.Microsoft.DependencyInjection)

# Unity.Microsoft.DependencyInjection

Unity extension to integrate with [Microsoft.Extensions.DependencyInjection](https://github.com/aspnet/DependencyInjection)  compliant systems

## Getting Started

- Reference the `Unity.Microsoft.DependencyInjection` package from NuGet.

```shell
Install-Package Unity.Microsoft.DependencyInjection
```

## Registration:

- In the `WebHostBuilder` add `UseUnityServiceProvider(...)` method

```C#
public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .UseUnityServiceProvider()   <---- Add this line
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
        });
```

- In case Unity container configured via application configuration or by convention this container could be used to initialize service provider.

```C#
public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .UseUnityServiceProvider(_container)   //<---- Add this line
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
        });
```

- Add optional method to your `Startup` class

```C#
public void ConfigureContainer(IUnityContainer container)
{
  // Could be used to register more types
  container.RegisterType<IMyService, MyService>();
}
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

For example of using Unity with Core 3.1 Web application follow [this link](https://github.com/unitycontainer/examples/tree/master/src/web/ASP.Net.Unity.Example)

## Fork

This project is a fork of https://github.com/unitycontainer/microsoft-dependency-injection. It adds additional options that wasn't available in original library. List of changes

* Possiblity to choose prefered implementation (from default container or Unity). Example bellow:

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

