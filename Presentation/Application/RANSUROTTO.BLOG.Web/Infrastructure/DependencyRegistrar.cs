using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using RANSUROTTO.BLOG.Core.Configuration;
using RANSUROTTO.BLOG.Core.Infrastructure;
using RANSUROTTO.BLOG.Core.Infrastructure.TypeFinder;
using RANSUROTTO.BLOG.Web.Infrastructure.Installation;

namespace RANSUROTTO.BLOG.Web.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {

        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder, WebConfig config)
        {
            builder.RegisterType<InstallationLocalizationService>().As<IInstallationLocalizationService>().InstancePerLifetimeScope();

        }

        public int Order => 1;

    }
}