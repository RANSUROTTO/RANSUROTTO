using Autofac;
using RANSUROTTO.BLOG.Core.Configuration;
using RANSUROTTO.BLOG.Core.Infrastructure;
using RANSUROTTO.BLOG.Core.Infrastructure.TypeFinder;
using RANSUROTTO.BLOG.Web.Factories;
using RANSUROTTO.BLOG.Web.Infrastructure.Installation;

namespace RANSUROTTO.BLOG.Web.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {

        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder, WebConfig config)
        {
            builder.RegisterType<InstallationLocalizationService>().As<IInstallationLocalizationService>().InstancePerLifetimeScope();

            //Factories
            builder.RegisterType<CommonModelFactory>().As<ICommonModelFactory>().InstancePerLifetimeScope();

        }

        public int Order => 1;

    }
}