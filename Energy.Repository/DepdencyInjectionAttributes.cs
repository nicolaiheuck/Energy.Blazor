using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
