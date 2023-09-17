using System.Reflection;
using System.Runtime.CompilerServices;
using Energy.Repositories.Interfaces;
using Energy.Services.Interfaces;

namespace Energy.Blazor.Extensions;

public static class DependencyInjectionScanner
{
    public static void RegisterDependencies(this WebApplicationBuilder builder)
    {
        builder.Services.AddByNamespaces(ServiceLifetime.Scoped, "Energy.Services.Interfaces", "Energy.Services.Services", "Energy.Repositories.Interfaces", "Energy.Repositories.Repositories");
    }

    /// <summary>
    /// Recursively adds all classes in namespace. Default lifetime is transient. Single class can be overwritten with [LifeTime(ServiceLifetime.Singleton)]  
    /// </summary>
    /// <param name="namespaces">Namespace to (recursively) search for classes in</param>
    /// <returns>Configured service collection</returns>
    public static IServiceCollection AddByNamespaces(this IServiceCollection services, ServiceLifetime defaultLifetime = ServiceLifetime.Transient, params string[] namespaces)
    {
        List<ServiceDescriptor> serviceInformation = FindTypes(namespaces, defaultLifetime);

        foreach (ServiceDescriptor service in serviceInformation)
        {
            services.Add(service);
        }

        return services;
    }

    #region Internal logic
    private static List<ServiceDescriptor> FindTypes(string[] namespaces, ServiceLifetime defaultLifetime)
    {
        List<Type> serviceTypes = GetValidTypesInNamespace(namespaces);

        List<ServiceDescriptor> services = new();

        foreach (Type serviceType in serviceTypes)
        {
            Type implementationType = serviceType;

            if (serviceType.IsInterface)
            {
                implementationType = serviceTypes.FirstOrDefault(t => string.Equals($"I{t.Name}", serviceType.Name, StringComparison.CurrentCultureIgnoreCase));

                if (implementationType == null)
                    throw new Exception(
                    $"Could not find any implementation of interface {serviceType.Name}. To ignore interface add [IgnoreService] attribute");
            }
            else if (serviceTypes.Any(s => string.Equals($"I{serviceType.Name}", s.Name, StringComparison.CurrentCultureIgnoreCase)))
            {
                continue;
            }

            ServiceLifetime lifetime = defaultLifetime;

            if (implementationType.HasAttribute<LifeTimeAttribute>())
                lifetime = implementationType.GetCustomAttribute<LifeTimeAttribute>()!.Lifetime;

            services.Add(new ServiceDescriptor(serviceType, implementationType, lifetime));
        }

        services = Validate(services);

        return services;
    }

    private static bool HasAttribute<TAttribute>(this Type type) where TAttribute : Attribute
    {
        return type.GetCustomAttribute<TAttribute>() != null;
    }

    private static List<ServiceDescriptor> Validate(List<ServiceDescriptor> services)
    {
        foreach (ServiceDescriptor service in services)
        {
            if (service.ImplementationType == null)
                throw new ArgumentNullException(
                $"The interface {service.ServiceType.Name} does not have an implementation");
        }

        return services.Where(si => !si.ImplementationType.HasAttribute<IgnoreServiceAttribute>())
                       .ToList();
    }

    private static List<Type> GetValidTypesInNamespace(string[] namespaces)
    {
        return GetClassesAndInterfacesByNamespace(namespaces)
               .Where(t => !t.HasAttribute<CompilerGeneratedAttribute>())
               .ToList();
    }

    private static List<Type> GetClassesAndInterfacesByNamespace(string[] namespaces)
    {
        List<Assembly> assemblies = new()
        {
            typeof(IExampleService).Assembly,
            typeof(IExampleRepository).Assembly,
        };

        return assemblies.SelectMany(a => a.GetTypes())
                         .Where(x => x.Namespace != null && namespaces.Any(n => x.Namespace.StartsWith(n)))
                         .Where(t => t != null && t.Namespace != null)
                         .Where(t => t.IsInterface || t.IsClass)
                         .ToList();
    }
    #endregion
}
public class LifeTimeAttribute : Attribute
{
    internal ServiceLifetime Lifetime { get; set; }

    internal LifeTimeAttribute(ServiceLifetime lifetime)
    {
        Lifetime = lifetime;
    }
}
public class IgnoreServiceAttribute : Attribute { }