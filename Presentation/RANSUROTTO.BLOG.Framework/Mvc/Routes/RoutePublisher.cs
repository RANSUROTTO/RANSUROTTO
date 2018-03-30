using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;
using RANSUROTTO.BLOG.Core.Infrastructure.TypeFinder;
using RANSUROTTO.BLOG.Core.Plugins;

namespace RANSUROTTO.BLOG.Framework.Mvc.Routes
{
    /// <summary>
    /// 路由发布器
    /// </summary>
    public class RoutePublisher : IRoutePublisher
    {

        protected readonly ITypeFinder TypeFinder;

        public RoutePublisher(ITypeFinder typeFinder)
        {
            this.TypeFinder = typeFinder;
        }
        public virtual void RegisterRoutes(RouteCollection routes)
        {
            var routeProviderTypes = TypeFinder.FindClassesOfType<IRouteProvider>();
            var routeProviders = new List<IRouteProvider>();

            foreach (var providerType in routeProviderTypes)
            {
                var plugin = FindPlugin(providerType);
                if (plugin != null && !plugin.Installed)
                    continue;

                var provider = Activator.CreateInstance(providerType) as IRouteProvider;
                routeProviders.Add(provider);
            }

            routeProviders = routeProviders.OrderByDescending(rp => rp.Priority).ToList();
            routeProviders.ForEach(rp => rp.RegisterRoutes(routes));
        }

        protected virtual PluginDescriptor FindPlugin(Type providerType)
        {
            if (providerType == null)
                throw new ArgumentNullException(nameof(providerType));

            foreach (var plugin in PluginManager.ReferencedPlugins)
            {
                if (plugin.ReferencedAssembly == null)
                    continue;

                if (plugin.ReferencedAssembly.FullName == providerType.Assembly.FullName)
                    return plugin;
            }

            return null;
        }

    }
}
