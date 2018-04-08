using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using AutoMapper;
using RANSUROTTO.BLOG.Core.Configuration;
using RANSUROTTO.BLOG.Core.Infrastructure.DependencyManagement;
using RANSUROTTO.BLOG.Core.Infrastructure.Mapper;
using RANSUROTTO.BLOG.Core.Infrastructure.TypeFinder;

namespace RANSUROTTO.BLOG.Core.Infrastructure
{
    /// <summary>
    /// 项目引擎
    /// </summary>
    public class SiteEngine : IEngine
    {

        #region Fields

        private ContainerManager _containerManager;

        #endregion

        #region Properties

        /// <summary>
        /// 集装箱管理
        /// </summary>
        public ContainerManager ContainerManager
        {
            get { return _containerManager; }
        }

        #endregion

        #region Methods

        public virtual void Initialize(WebConfig config)
        {
            //依赖注册
            RegisterDependencies(config);

            //映射配置注册
            RegisterMapperConfiguration(config);

            //运行启动任务
            if (!config.IgnoreStartupTasks)
            {
                RunStartupTasks();
            }
        }

        public virtual T Resolve<T>() where T : class
        {
            return ContainerManager.Resolve<T>();
        }

        public virtual object Resolve(Type type)
        {
            return ContainerManager.Resolve(type);
        }

        public virtual T[] ResolveAll<T>()
        {
            return ContainerManager.ResolveAll<T>();
        }

        #endregion

        #region Utilities

        /// <summary>
        /// 注册依赖
        /// </summary>
        /// <param name="config"></param>
        public virtual void RegisterDependencies(WebConfig config)
        {
            var builder = new ContainerBuilder();
            var container = builder.Build();
            _containerManager = new ContainerManager(container);

            var typeFinder = new WebAppTypeFinder();
            builder = new ContainerBuilder();
            builder.RegisterInstance(config).As<WebConfig>().SingleInstance();
            builder.RegisterInstance(this).As<IEngine>().SingleInstance();
            builder.RegisterInstance(typeFinder).As<ITypeFinder>().SingleInstance();
            builder.Update(container);

            //register dependencies provided by other assemblies
            builder = new ContainerBuilder();
            var drTypes = typeFinder.FindClassesOfType<IDependencyRegistrar>();
            var drInstances = drTypes.Select(drType => (IDependencyRegistrar)Activator.CreateInstance(drType)).ToList();
            //sort
            drInstances = drInstances.AsQueryable().OrderBy(t => t.Order).ToList();
            drInstances.ForEach(p => p.Register(builder, typeFinder, config));
            builder.Update(container);

            //set dependency resolver
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        /// <summary>
        /// 运行应用启动任务
        /// </summary>
        public virtual void RunStartupTasks()
        {
            //finder subclass
            var typeFinder = _containerManager.Resolve<ITypeFinder>();
            var startUpTaskTypes = typeFinder.FindClassesOfType<IStartupTask>();
            //builder examples
            var startUpTasks = startUpTaskTypes.Select(startUpTaskType => (IStartupTask)Activator.CreateInstance(startUpTaskType)).ToList();
            //sort
            startUpTasks = startUpTasks.AsQueryable().OrderBy(st => st.Order).ToList();
            //Execute
            startUpTasks.ForEach(p => p.Execute());
        }

        protected virtual void RegisterMapperConfiguration(WebConfig config)
        {
            //dependencies
            var typeFinder = new WebAppTypeFinder();

            //register mapper configurations provided by other assemblies
            var mcTypes = typeFinder.FindClassesOfType<IMapperConfiguration>();
            var mcInstances = new List<IMapperConfiguration>();
            foreach (var mcType in mcTypes)
                mcInstances.Add((IMapperConfiguration)Activator.CreateInstance(mcType));
            //sort
            mcInstances = mcInstances.AsQueryable().OrderBy(t => t.Order).ToList();
            //get configurations
            var configurationActions = new List<Action<IMapperConfigurationExpression>>();
            foreach (var mc in mcInstances)
                configurationActions.Add(mc.GetConfiguration());
            //register
            AutoMapperConfiguration.Init(configurationActions);
        }

        #endregion

    }
}
