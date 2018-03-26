using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using RANSUROTTO.BLOG.Core.Infrastructure;

namespace RANSUROTTO.BLOG.Core.Plugins
{

    /// <summary>
    /// 插件描述信息
    /// </summary>
    public class PluginDescriptor : IDescriptor, IComparable<PluginDescriptor>
    {

        #region Constructor



        #endregion

        #region Properties

        /// <summary>
        /// 获取或设置插件分组
        /// </summary>
        public virtual string Group { get; set; }

        /// <summary>
        /// 获取或设置插件系统名称
        /// </summary>
        public string SystemName { get; set; }

        /// <summary>
        /// 获取或设置插件友好名称
        /// </summary>
        public string FriendlyName { get; set; }

        /// <summary>
        /// 获取或设置插件版本
        /// </summary>
        public virtual string Version { get; set; }

        /// <summary>
        /// 获取或设置插件支持的应用程序版本
        /// </summary>
        public virtual IList<string> SupportedVersions { get; set; }

        /// <summary>
        /// 获取或设置插件作者
        /// </summary>
        public virtual string Author { get; set; }

        /// <summary>
        /// 获取或设置显示顺序
        /// </summary>
        public virtual int DisplayOrder { get; set; }

        /// <summary>
        /// 获取或设置程序集文件名称
        /// </summary>
        public virtual string AssemblyFileName { get; set; }

        /// <summary>
        /// 获取或设置描述信息
        /// </summary>
        public virtual string Description { get; set; }


        /// <summary>
        /// 获取或设置是否已安装插件的标识值
        /// </summary>
        public virtual bool Installed { get; set; }

        /// <summary>
        /// 获取或设置插件类型
        /// </summary>
        public virtual Type PluginType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual FileInfo OriginalAssemblyFile { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual Assembly ReferencedAssembly { get; internal set; }

        #endregion

        /// <summary>
        /// 获取插件实例对象
        /// </summary>
        public IPlugin Instance()
        {
            return Instance<IPlugin>();
        }

        public virtual T Instance<T>() where T : class, IPlugin
        {
            if (!EngineContext.Current.ContainerManager.TryResolve(PluginType, null, out var instance))
            {
                instance = EngineContext.Current.ContainerManager.ResolveUnregistered(PluginType);
            }
            var typedInstance = instance as T;
            if (typedInstance != null)
                typedInstance.PluginDescriptor = this;
            return typedInstance;
        }

        public int CompareTo(PluginDescriptor other)
        {
            if (DisplayOrder != other.DisplayOrder)
                return DisplayOrder.CompareTo(other.DisplayOrder);

            return String.Compare(FriendlyName, other.FriendlyName, StringComparison.Ordinal);
        }

        public override string ToString()
        {
            return FriendlyName;
        }

        public override int GetHashCode()
        {
            return SystemName.GetHashCode();
        }

        public override bool Equals(object value)
        {
            return SystemName?.Equals((value as PluginDescriptor)?.SystemName) ?? false;
        }

    }
}
