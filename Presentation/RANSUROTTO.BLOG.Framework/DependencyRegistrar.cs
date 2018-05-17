using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Autofac.Integration.Mvc;
using RANSUROTTO.BLOG.Core.Caching;
using RANSUROTTO.BLOG.Core.Caching.RedisCaching;
using RANSUROTTO.BLOG.Core.Configuration;
using RANSUROTTO.BLOG.Core.Context;
using RANSUROTTO.BLOG.Core.Data;
using RANSUROTTO.BLOG.Core.Fakes;
using RANSUROTTO.BLOG.Core.Helper;
using RANSUROTTO.BLOG.Core.Infrastructure;
using RANSUROTTO.BLOG.Core.Infrastructure.TypeFinder;
using RANSUROTTO.BLOG.Data.Context;
using RANSUROTTO.BLOG.Data.Provider;
using RANSUROTTO.BLOG.Data.Repository;
using RANSUROTTO.BLOG.Framework.Mvc.Routes;
using RANSUROTTO.BLOG.Framework.Themes;
using RANSUROTTO.BLOG.Framework.UI;
using RANSUROTTO.BLOG.Service.Helpers;
using RANSUROTTO.BLOG.Services.Authentication;
using RANSUROTTO.BLOG.Services.Common;
using RANSUROTTO.BLOG.Services.Configuration;
using RANSUROTTO.BLOG.Services.Customers;
using RANSUROTTO.BLOG.Services.Events;
using RANSUROTTO.BLOG.Services.Helpers;
using RANSUROTTO.BLOG.Services.Infrastructure;
using RANSUROTTO.BLOG.Services.Installation;
using RANSUROTTO.BLOG.Services.Localization;
using RANSUROTTO.BLOG.Services.Logging;
using RANSUROTTO.BLOG.Services.Messages;
using RANSUROTTO.BLOG.Services.Security;
using RANSUROTTO.BLOG.Services.Tasks;

namespace RANSUROTTO.BLOG.Framework
{
    /// <summary>
    /// 依赖注册
    /// </summary>
    public class DependencyRegistrar : IDependencyRegistrar
    {

        /// <summary>
        /// 注册服务与接口
        /// </summary>
        public void Register(ContainerBuilder builder, ITypeFinder typeFinder, WebConfig config)
        {
            //Http
            builder.Register(c =>
                        HttpContext.Current != null ?
                            (new HttpContextWrapper(HttpContext.Current) as HttpContextBase) :
                            (new FakeHttpContext("~/") as HttpContextBase))
                .As<HttpContextBase>()
                .InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Request)
                .As<HttpRequestBase>()
                .InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Response)
                .As<HttpResponseBase>()
                .InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Server)
                .As<HttpServerUtilityBase>()
                .InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Session)
                .As<HttpSessionStateBase>()
                .InstancePerLifetimeScope();

            //Web helper
            builder.RegisterType<WebHelper>().As<IWebHelper>().InstancePerLifetimeScope();

            //UserAgent helper
            builder.RegisterType<UserAgentHelper>().As<IUserAgentHelper>().InstancePerLifetimeScope();

            //Controller
            builder.RegisterControllers(typeFinder.GetAssemblies().ToArray());

            //Data layer
            var dataSettingsManager = new DataSettingsManager();
            var dataProviderSettings = dataSettingsManager.LoadSettings();
            builder.Register(c => dataSettingsManager.LoadSettings()).As<DataSettings>();
            builder.Register(x => new EFDataProviderManager(x.Resolve<DataSettings>())).As<BaseDataProviderManager>().InstancePerDependency();

            builder.Register(x => x.Resolve<BaseDataProviderManager>().LoadDataProvider()).As<IDataProvider>().InstancePerDependency();

            //Data
            if (dataProviderSettings != null && dataProviderSettings.IsValid())
            {
                var efDataProviderManager = new EFDataProviderManager(dataSettingsManager.LoadSettings());
                var dataProvider = efDataProviderManager.LoadDataProvider();
                dataProvider.InitConnectionFactory();

                builder.Register<IDbContext>(c => new EntityContext(dataProviderSettings.DataConnectionString)).InstancePerLifetimeScope();
            }
            else
            {
                builder.Register<IDbContext>(c => new EntityContext(dataSettingsManager.LoadSettings().DataConnectionString)).InstancePerLifetimeScope();
            }

            builder.RegisterGeneric(typeof(EfRepository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();

            //Cache
            if (config.RedisCachingEnable)
            {
                builder.RegisterType<RedisConnectionWrapper>().As<IRedisConnectionWrapper>().SingleInstance();
                builder.RegisterType<RedisCacheManager>().As<ICacheManager>().Named<ICacheManager>("ransurotto_cache_static").InstancePerLifetimeScope();
            }
            else
            {
                builder.RegisterType<MemoryCacheManager>().As<ICacheManager>().Named<ICacheManager>("ransurotto_cache_static").SingleInstance();
            }
            builder.RegisterType<PerRequestCacheManager>().As<ICacheManager>().Named<ICacheManager>("ransurotto_cache_per_request").InstancePerLifetimeScope();

            //Machine Name Provider
            if (config.RunOnAzureWebApps)
            {
                builder.RegisterType<AzureWebAppsMachineNameProvider>().As<IMachineNameProvider>().SingleInstance();
            }
            else
            {
                builder.RegisterType<DefaultMachineNameProvider>().As<IMachineNameProvider>().SingleInstance();
            }

            //Work context
            builder.RegisterType<WebWorkContext>().As<IWorkContext>().InstancePerLifetimeScope();

            //Settings
            builder.RegisterType<SettingService>().As<ISettingService>()
                .WithParameter(ResolvedParameter.ForNamed<ICacheManager>("ransurotto_cache_static"))
                .InstancePerLifetimeScope();
            builder.RegisterSource(new SettingsSource());

            //Services

            builder.RegisterType<LocalizationService>().As<ILocalizationService>()
                .WithParameter(ResolvedParameter.ForNamed<ICacheManager>("ransurotto_cache_static"))
                .InstancePerLifetimeScope();
            builder.RegisterType<LanguageService>().As<ILanguageService>()
                .WithParameter(ResolvedParameter.ForNamed<ICacheManager>("ransurotto_cache_static"))
                .InstancePerLifetimeScope();

            builder.RegisterType<CustomerService>().As<ICustomerService>().InstancePerLifetimeScope();
            builder.RegisterType<CustomerRegistrationService>().As<ICustomerRegistrationService>().InstancePerLifetimeScope();
            builder.RegisterType<CustomerReportService>().As<ICustomerReportService>().InstancePerLifetimeScope();

            builder.RegisterType<GenericAttributeService>().As<IGenericAttributeService>().InstancePerLifetimeScope();
            builder.RegisterType<MaintenanceService>().As<IMaintenanceService>().InstancePerLifetimeScope();

            builder.RegisterType<PageHeadBuilder>().As<IPageHeadBuilder>().InstancePerLifetimeScope();
            builder.RegisterType<DateTimeHelper>().As<IDateTimeHelper>().InstancePerLifetimeScope();

            builder.RegisterType<ScheduleTaskService>().As<IScheduleTaskService>().InstancePerLifetimeScope();
            builder.RegisterType<EncryptionService>().As<IEncryptionService>().InstancePerLifetimeScope();
            builder.RegisterType<FormsAuthenticationService>().As<IAuthenticationService>().InstancePerLifetimeScope();
            builder.RegisterType<Logger>().As<ILogger>().InstancePerLifetimeScope();

            builder.RegisterType<EmailAccountService>().As<IEmailAccountService>().InstancePerLifetimeScope();
            builder.RegisterType<EmailSender>().As<IEmailSender>().InstancePerLifetimeScope();

            builder.RegisterType<CustomerActivityService>().As<ICustomerActivityService>()
                .WithParameter(ResolvedParameter.ForNamed<ICacheManager>("ransurotto_cache_static"))
                .InstancePerLifetimeScope();

            builder.RegisterType<PermissionService>().As<IPermissionService>()
                .WithParameter(ResolvedParameter.ForNamed<ICacheManager>("ransurotto_cache_static"))
                .InstancePerLifetimeScope();

            //Install services
            bool databaseInstalled = DataSettingsHelper.DatabaseIsInstalled();
            if (!databaseInstalled)
            {
                builder.RegisterType<CodeFirstInstallationService>().As<IInstallationService>().InstancePerLifetimeScope();
            }

            //Theme services
            builder.RegisterType<ThemeProvider>().As<IThemeProvider>().InstancePerLifetimeScope();
            builder.RegisterType<ThemeContext>().As<IThemeContext>().InstancePerLifetimeScope();

            //Route
            builder.RegisterType<RoutePublisher>().As<IRoutePublisher>().SingleInstance();

            //Event
            var consumers = typeFinder.FindClassesOfType(typeof(IConsumer<>)).ToList();
            foreach (var consumer in consumers)
            {
                builder.RegisterType(consumer)
                    .As(consumer.FindInterfaces((type, criteria) =>
                    {
                        var isMatch = type.IsGenericType && ((Type)criteria).IsAssignableFrom(type.GetGenericTypeDefinition());
                        return isMatch;
                    }, typeof(IConsumer<>)))
                    .InstancePerLifetimeScope();
            }
            builder.RegisterType<EventPublisher>().As<IEventPublisher>().SingleInstance();
            builder.RegisterType<SubscriptionService>().As<ISubscriptionService>().SingleInstance();

        }

        public int Order => 0;

    }

    public class SettingsSource : IRegistrationSource
    {
        static readonly MethodInfo BuildMethod = typeof(SettingsSource).GetMethod(
            "BuildRegistration",
            BindingFlags.Static | BindingFlags.NonPublic);

        public IEnumerable<IComponentRegistration> RegistrationsFor(
            Autofac.Core.Service service,
            Func<Autofac.Core.Service, IEnumerable<IComponentRegistration>> registrations)
        {
            var ts = service as TypedService;
            if (ts != null && typeof(ISettings).IsAssignableFrom(ts.ServiceType))
            {
                var buildMethod = BuildMethod.MakeGenericMethod(ts.ServiceType);
                yield return (IComponentRegistration)buildMethod.Invoke(null, null);
            }
        }

        static IComponentRegistration BuildRegistration<TSettings>() where TSettings : ISettings, new()
        {
            return RegistrationBuilder
                .ForDelegate((c, p) =>
                {
                    //uncomment the code below if you want load settings per store only when you have two stores installed.
                    //var currentStoreId = c.Resolve<IStoreService>().GetAllStores().Count > 1
                    //    c.Resolve<IStoreContext>().CurrentStore.Id : 0;

                    //although it's better to connect to your database and execute the following SQL:
                    //DELETE FROM [Setting] WHERE [StoreId] > 0
                    return c.Resolve<ISettingService>().LoadSetting<TSettings>();
                })
                .InstancePerLifetimeScope()
                .CreateRegistration();
        }

        public bool IsAdapterForIndividualComponents { get { return false; } }
    }

}
