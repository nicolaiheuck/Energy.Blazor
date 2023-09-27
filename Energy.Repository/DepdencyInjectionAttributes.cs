using Microsoft.Extensions.DependencyInjection;

namespace Energy.Repositories
{
    public class LifeTimeAttribute : Attribute
    {
        public ServiceLifetime Lifetime { get; set; }

        internal LifeTimeAttribute(ServiceLifetime lifetime)
        {
            Lifetime = lifetime;
        }
    }
    public class IgnoreServiceAttribute : Attribute { }
}
