using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Compilation;
using Newtonsoft.Json;
using RANSUROTTO.BLOG.Core.ComponentModel;
using RANSUROTTO.BLOG.Core.Configuration;
using RANSUROTTO.BLOG.Core.Helper;

namespace RANSUROTTO.BLOG.Core.Plugins
{
    public class PluginManager
    {

        #region Fields

        private static readonly ReaderWriterLockSlim Locker = new ReaderWriterLockSlim();
        private static DirectoryInfo _shadowCopyFolder;
        private static readonly List<string> BaseAppLibraries;

        #endregion

        #region Constructor

        static PluginManager()
        {
            //获取 /bin 目录下的所有类库(*.dll)文件
            BaseAppLibraries = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory)
                .GetFiles("*.dll", SearchOption.TopDirectoryOnly).Select(fi => fi.Name).ToList();
            //从站点目录中获取所有类库文件
            if (!AppDomain.CurrentDomain.BaseDirectory.Equals(Environment.CurrentDirectory, StringComparison.InvariantCultureIgnoreCase))
                BaseAppLibraries.AddRange(new DirectoryInfo(Environment.CurrentDirectory).GetFiles("*.dll", SearchOption.TopDirectoryOnly).Select(fi => fi.Name));
            //获取引用目录中所有类库文件
            var refsPathName = new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, RefsPathName));
            if (refsPathName.Exists)
                BaseAppLibraries.AddRange(refsPathName.GetFiles("*.dll", SearchOption.TopDirectoryOnly).Select(fi => fi.Name));
        }

        #endregion

        #region Properties

        /// <summary>
        /// 获取包含以前版本中安装的插件系统(名称)的文件路径
        /// </summary>
        public static string ObsoleteInstalledPluginsFilePath => "~/App_Data/InstalledPlugins.txt";

        /// <summary>
        /// 获取包含已安装的插件系统(名称)的文件路径
        /// </summary>
        public static string InstalledPluginsFilePath => "~/App_Data/installedPlugins.json";

        /// <summary>
        /// 获取插件文件夹目录
        /// </summary>
        public static string PluginsPath => "~/Plugins";

        /// <summary>
        /// 获取插件文件目录名
        /// </summary>
        public static string PluginsPathName => "Plugins";

        /// <summary>
        /// 获取插件复制文件夹的路径
        /// </summary>
        public static string ShadowCopyPath => "~/Plugins/bin";

        /// <summary>
        /// 获取插件引用程序集文件夹的路径
        /// </summary>
        public static string RefsPathName => "refs";

        /// <summary>
        /// 获取插件描述文件的名称
        /// </summary>
        public static string PluginDescriptionFileName => "plugin.json";

        /// <summary>
        /// 返回已被复制的所有引用插件程序集的集合
        /// </summary>
        public static IEnumerable<PluginDescriptor> ReferencedPlugins { get; set; }

        /// <summary>
        /// 返回与当前版本不兼容的所有插件的集合
        /// </summary>
        public static IEnumerable<string> IncompatiblePlugins { get; set; }

        #endregion

        #region Methods

        public static void Initialize(WebConfig config)
        {
            using (new WriteLockDisposable(Locker))
            {
                //获取插件文件夹对象和复制文件夹对象
                var pluginFolder = new DirectoryInfo(CommonHelper.MapPath(PluginsPath));
                _shadowCopyFolder = new DirectoryInfo(CommonHelper.MapPath(ShadowCopyPath));

                var referencedPlugins = new List<PluginDescriptor>();
                var incompatiblePlugins = new List<string>();

                try
                {
                    var installedPluginSystemNames = GetInstalledPluginNames(CommonHelper.MapPath(InstalledPluginsFilePath));

                    Debug.WriteLine("Creating shadow copy folder and querying for DLLs");
                    //创建插件和拷贝文件夹
                    Directory.CreateDirectory(pluginFolder.FullName);
                    Directory.CreateDirectory(_shadowCopyFolder.FullName);

                    //获取bin目录文件列表
                    var binFiles = _shadowCopyFolder.GetFiles("*", SearchOption.AllDirectories);
                    if (config.ClearPluginShadowDirectoryOnStartup)
                    {
                        foreach (var f in binFiles)
                        {
                            if (f.Name.Equals("placeholder.txt", StringComparison.InvariantCultureIgnoreCase))
                                continue;

                            Debug.WriteLine("Deleting " + f.Name);
                            try
                            {
                                //ignore index.htm
                                var fileName = Path.GetFileName(f.FullName);
                                if (fileName.Equals("index.htm", StringComparison.InvariantCultureIgnoreCase))
                                    continue;

                                File.Delete(f.FullName);
                            }
                            catch (Exception exc)
                            {
                                Debug.WriteLine("Error deleting file " + f.Name + ". Exception: " + exc);
                            }
                        }
                    }

                    //加载描述文件
                    foreach (var dfd in GetDescriptionFilesAndDescriptors(pluginFolder))
                    {
                        var descriptionFile = dfd.Key;
                        var pluginDescriptor = dfd.Value;

                        //确保插件版本有效
                        if (!pluginDescriptor.SupportedVersions.Contains(RansurottoVersion.CurrentVersion, StringComparer.InvariantCultureIgnoreCase))
                        {
                            incompatiblePlugins.Add(pluginDescriptor.SystemName);
                            continue;
                        }

                        if (string.IsNullOrWhiteSpace(pluginDescriptor.SystemName))
                            throw new Exception($"此插件 '{descriptionFile.FullName}'没有系统名，尝试分配一个唯一的名称并且重新编译的插件。");
                        if (referencedPlugins.Contains(pluginDescriptor))
                            throw new Exception($"此插件'{pluginDescriptor.SystemName}' 系统名称已经定义(已使用)。");

                        //设置'已安装'属性
                        pluginDescriptor.Installed = installedPluginSystemNames
                                                         .FirstOrDefault(x => x.Equals(pluginDescriptor.SystemName, StringComparison.InvariantCultureIgnoreCase)) != null;

                        try
                        {
                            if (descriptionFile.Directory == null)
                                throw new Exception($"无法解析 '{descriptionFile.Name}' 描述文件目录");

                            var pluginFiles = descriptionFile.Directory.GetFiles("*.dll", SearchOption.AllDirectories)
                                .Where(x => !binFiles.Select(q => q.FullName).Contains(x.FullName))
                                .Where(x => IsPackagePluginFolder(x.Directory))
                                .ToList();

                            var mainPluginFile = pluginFiles
                                .FirstOrDefault(x => x.Name.Equals(pluginDescriptor.AssemblyFileName, StringComparison.InvariantCultureIgnoreCase));

                            if (mainPluginFile == null)
                            {
                                incompatiblePlugins.Add(pluginDescriptor.SystemName);
                                continue;
                            }

                            pluginDescriptor.OriginalAssemblyFile = mainPluginFile;

                            pluginDescriptor.ReferencedAssembly = PerformFileDeploy(mainPluginFile);

                            foreach (var plugin in pluginFiles
                                .Where(x => !x.Name.Equals(mainPluginFile.Name, StringComparison.InvariantCultureIgnoreCase))
                                .Where(x => !IsAlreadyLoaded(x)))
                                PerformFileDeploy(plugin);

                            foreach (var t in pluginDescriptor.ReferencedAssembly.GetTypes())
                                if (typeof(IPlugin).IsAssignableFrom(t))
                                    if (!t.IsInterface)
                                        if (t.IsClass && !t.IsAbstract)
                                        {
                                            pluginDescriptor.PluginType = t;
                                            break;
                                        }

                            referencedPlugins.Add(pluginDescriptor);

                        }
                        catch (ReflectionTypeLoadException ex)
                        {
                            //add a plugin name. this way we can easily identify a problematic plugin
                            var msg = string.Format("Plugin '{0}'. ", pluginDescriptor.FriendlyName);
                            foreach (var e in ex.LoaderExceptions)
                                msg += e.Message + Environment.NewLine;

                            var fail = new Exception(msg, ex);
                            throw fail;
                        }
                        catch (Exception ex)
                        {
                            //add a plugin name. this way we can easily identify a problematic plugin
                            var msg = string.Format("Plugin '{0}'. {1}", pluginDescriptor.FriendlyName, ex.Message);

                            var fail = new Exception(msg, ex);
                            throw fail;
                        }
                    }
                }
                catch (Exception ex)
                {
                    var msg = string.Empty;
                    for (var e = ex; e != null; e = e.InnerException)
                        msg += e.Message + Environment.NewLine;

                    var fail = new Exception(msg, ex);
                    throw fail;
                }

                ReferencedPlugins = referencedPlugins;
                IncompatiblePlugins = incompatiblePlugins;
            }
        }

        /// <summary>
        /// Mark plugin as installed
        /// </summary>
        /// <param name="systemName">Plugin system name</param>
        public static void MarkPluginAsInstalled(string systemName)
        {
            if (string.IsNullOrEmpty(systemName))
                throw new ArgumentNullException(nameof(systemName));

            var filePath = CommonHelper.MapPath(InstalledPluginsFilePath);

            //create file if not exists
            if (!File.Exists(filePath))
            {
                //we use 'using' to close the file after it's created
                using (File.Create(filePath)) { }
            }

            //get installed plugin names
            var installedPluginSystemNames = GetInstalledPluginNames(filePath);

            //add plugin system name to the list if doesn't already exist
            var alreadyMarkedAsInstalled = installedPluginSystemNames.Any(pluginName => pluginName.Equals(systemName, StringComparison.InvariantCultureIgnoreCase));
            if (!alreadyMarkedAsInstalled)
                installedPluginSystemNames.Add(systemName);

            //save installed plugin names to the file
            SaveInstalledPluginNames(installedPluginSystemNames, filePath);
        }

        /// <summary>
        /// Mark plugin as uninstalled
        /// </summary>
        /// <param name="systemName">Plugin system name</param>
        public static void MarkPluginAsUninstalled(string systemName)
        {
            if (string.IsNullOrEmpty(systemName))
                throw new ArgumentNullException(nameof(systemName));

            var filePath = CommonHelper.MapPath(InstalledPluginsFilePath);

            //create file if not exists
            if (!File.Exists(filePath))
            {
                //we use 'using' to close the file after it's created
                using (File.Create(filePath)) { }
            }

            //get installed plugin names
            var installedPluginSystemNames = GetInstalledPluginNames(filePath);

            //remove plugin system name from the list if exists
            var alreadyMarkedAsInstalled = installedPluginSystemNames.Any(pluginName => pluginName.Equals(systemName, StringComparison.InvariantCultureIgnoreCase));
            if (alreadyMarkedAsInstalled)
                installedPluginSystemNames.Remove(systemName);

            //save installed plugin names to the file
            SaveInstalledPluginNames(installedPluginSystemNames, filePath);
        }

        /// <summary>
        /// Mark plugin as uninstalled
        /// </summary>
        public static void MarkAllPluginsAsUninstalled()
        {
            var filePath = CommonHelper.MapPath(InstalledPluginsFilePath);
            if (File.Exists(filePath))
                File.Delete(filePath);
        }

        #endregion

        #region Utilities

        /// <summary>
        /// 获取插件注释文件
        /// </summary>
        /// <param name="pluginFolder">插件文件夹对象</param>
        /// <returns>文件和解析描述文件</returns>
        private static IEnumerable<KeyValuePair<FileInfo, PluginDescriptor>> GetDescriptionFilesAndDescriptors(DirectoryInfo pluginFolder)
        {
            if (pluginFolder == null)
                throw new ArgumentNullException(nameof(pluginFolder));

            var result = new List<KeyValuePair<FileInfo, PluginDescriptor>>();

            foreach (var descriptionFile in pluginFolder.GetFiles(PluginDescriptionFileName, SearchOption.AllDirectories))
            {
                if (!IsPackagePluginFolder(descriptionFile.Directory))
                    continue;

                //parse file
                var pluginDescriptor = GetPluginDescriptorFromFile(descriptionFile.FullName);

                //populate list
                result.Add(new KeyValuePair<FileInfo, PluginDescriptor>(descriptionFile, pluginDescriptor));
            }

            result.Sort((firstPair, nextPair) => firstPair.Value.DisplayOrder.CompareTo(nextPair.Value.DisplayOrder));
            return result;
        }

        /// <summary>
        /// 获取已安装插件的系统名称列表
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        private static IList<string> GetInstalledPluginNames(string filePath)
        {
            //检查文件是否存在
            if (!File.Exists(filePath))
            {
                //获取保存旧版本已安装插件的系统名称的文件路径
                filePath = CommonHelper.MapPath(ObsoleteInstalledPluginsFilePath);
                if (!File.Exists(filePath))
                    return new List<string>();

                //读取插件系统名称
                var pluginSystemNames = new List<string>();
                using (var reader = new StringReader(File.ReadAllText(filePath)))
                {
                    string pluginName;
                    while ((pluginName = reader.ReadLine()) != null)
                    {
                        if (!string.IsNullOrWhiteSpace(pluginName))
                            pluginSystemNames.Add(pluginName.Trim());
                    }
                }

                //将已安装插件的系统名称保存到新文件
                SaveInstalledPluginNames(pluginSystemNames, CommonHelper.MapPath(InstalledPluginsFilePath));

                //删除旧文件
                File.Delete(filePath);

                return pluginSystemNames;
            }
            var text = File.ReadAllText(filePath);
            if (string.IsNullOrEmpty(text))
                return new List<string>();

            return JsonConvert.DeserializeObject<IList<string>>(text);
        }

        /// <summary>
        /// 保存已安装插件的系统名称到文件
        /// </summary>
        /// <param name="pluginSystemNames">已安装插件的系统名称列表</param>
        /// <param name="filePath">文件路径</param>
        private static void SaveInstalledPluginNames(IList<string> pluginSystemNames, string filePath)
        {
            var text = JsonConvert.SerializeObject(pluginSystemNames, Formatting.Indented);
            File.WriteAllText(filePath, text);
        }

        /// <summary>
        /// 确认文件夹是否包含为包的bin插件文件夹
        /// </summary>
        private static bool IsPackagePluginFolder(DirectoryInfo folder)
        {
            if (folder?.Parent == null) return false;
            if (!folder.Parent.Name.Equals(PluginsPathName, StringComparison.InvariantCultureIgnoreCase)) return false;

            return true;
        }

        /// <summary>
        /// 从插件描述文件中获取插件描述对象
        /// </summary>
        /// <param name="filePath">描述文件的路径</param>
        /// <returns>插件描述对象</returns>
        public static PluginDescriptor GetPluginDescriptorFromFile(string filePath)
        {
            var text = File.ReadAllText(filePath);

            return GetPluginDescriptorFromText(text);
        }

        /// <summary>
        /// 从描述文本中获取插件描述对象
        /// </summary>
        /// <param name="text">描述文本</param>
        /// <returns>插件描述对象</returns>
        public static PluginDescriptor GetPluginDescriptorFromText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return new PluginDescriptor();

            //从JSON文件中获取插件描述对象
            var descriptor = JsonConvert.DeserializeObject<PluginDescriptor>(text);

            return descriptor;
        }

        private static Assembly PerformFileDeploy(FileInfo plug)
        {
            if (plug.Directory == null || plug.Directory.Parent == null)
                throw new InvalidOperationException("无" + plug.Name + " 文件插件目录中存在的目录的文件夹层次结构以外的文件夹");

            FileInfo shadowCopiedPlug;

            if (CommonHelper.GetTrustLevel() != AspNetHostingPermissionLevel.Unrestricted)
            {
                //插件所有文件都需要拷贝到 ~/Plugins/bin/ 目录
                var shadowCopyPlugFolder = Directory.CreateDirectory(_shadowCopyFolder.FullName);
                shadowCopiedPlug = InitializeMediumTrust(plug, shadowCopyPlugFolder);
            }
            else
            {
                var directory = AppDomain.CurrentDomain.DynamicDirectory;
                Debug.WriteLine(plug.FullName + " to " + directory);

                shadowCopiedPlug = InitializeFullTrust(plug, new DirectoryInfo(directory));
            }

            //现在我们可以注册插件定义了
            var shadowCopiedAssembly = Assembly.Load(AssemblyName.GetAssemblyName(shadowCopiedPlug.FullName));

            //向构建管理器添加引用
            Debug.WriteLine("Adding to BuildManager: '{0}'", shadowCopiedAssembly.FullName);
            BuildManager.AddReferencedAssembly(shadowCopiedAssembly);

            return shadowCopiedAssembly;
        }

        private static FileInfo InitializeFullTrust(FileInfo plug, DirectoryInfo shadowCopyPlugFolder)
        {
            var shadowCopiedPlug = new FileInfo(Path.Combine(shadowCopyPlugFolder.FullName, plug.Name));
            try
            {
                File.Copy(plug.FullName, shadowCopiedPlug.FullName, true);
            }
            catch (IOException)
            {
                Debug.WriteLine(shadowCopiedPlug.FullName + " is locked, attempting to rename");
                //this occurs when the files are locked,
                //for some reason devenv locks plugin files some times and for another crazy reason you are allowed to rename them
                //which releases the lock, so that it what we are doing here, once it's renamed, we can re-shadow copy
                try
                {
                    var oldFile = shadowCopiedPlug.FullName + Guid.NewGuid().ToString("N") + ".old";
                    File.Move(shadowCopiedPlug.FullName, oldFile);
                }
                catch (IOException exc)
                {
                    throw new IOException(shadowCopiedPlug.FullName + " rename failed, cannot initialize plugin", exc);
                }
                //ok, we've made it this far, now retry the shadow copy
                File.Copy(plug.FullName, shadowCopiedPlug.FullName, true);
            }
            return shadowCopiedPlug;
        }

        private static FileInfo InitializeMediumTrust(FileInfo plug, DirectoryInfo shadowCopyPlugFolder)
        {
            var shouldCopy = true;
            var shadowCopiedPlug = new FileInfo(Path.Combine(shadowCopyPlugFolder.FullName, plug.Name));

            //check if a shadow copied file already exists and if it does, check if it's updated, if not don't copy
            if (shadowCopiedPlug.Exists)
            {
                //it's better to use LastWriteTimeUTC, but not all file systems have this property
                //maybe it is better to compare file hash?
                var areFilesIdentical = shadowCopiedPlug.CreationTimeUtc.Ticks >= plug.CreationTimeUtc.Ticks;
                if (areFilesIdentical)
                {
                    Debug.WriteLine("Not copying; files appear identical: '{0}'", shadowCopiedPlug.Name);
                    shouldCopy = false;
                }
                else
                {
                    //delete an existing file

                    //More info: http://www.nopcommerce.com/boards/t/11511/access-error-nopplugindiscountrulesbillingcountrydll.aspx?p=4#60838
                    Debug.WriteLine("New plugin found; Deleting the old file: '{0}'", shadowCopiedPlug.Name);
                    File.Delete(shadowCopiedPlug.FullName);
                }
            }

            if (shouldCopy)
            {
                try
                {
                    File.Copy(plug.FullName, shadowCopiedPlug.FullName, true);
                }
                catch (IOException)
                {
                    Debug.WriteLine(shadowCopiedPlug.FullName + " is locked, attempting to rename");
                    //this occurs when the files are locked,
                    //for some reason devenv locks plugin files some times and for another crazy reason you are allowed to rename them
                    //which releases the lock, so that it what we are doing here, once it's renamed, we can re-shadow copy
                    try
                    {
                        var oldFile = shadowCopiedPlug.FullName + Guid.NewGuid().ToString("N") + ".old";
                        File.Move(shadowCopiedPlug.FullName, oldFile);
                    }
                    catch (IOException exc)
                    {
                        throw new IOException(shadowCopiedPlug.FullName + " rename failed, cannot initialize plugin", exc);
                    }
                    //ok, we've made it this far, now retry the shadow copy
                    File.Copy(plug.FullName, shadowCopiedPlug.FullName, true);
                }
            }

            return shadowCopiedPlug;
        }

        private static bool IsAlreadyLoaded(FileInfo fileInfo)
        {
            //compare full assembly name
            //var fileAssemblyName = AssemblyName.GetAssemblyName(fileInfo.FullName);
            //foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            //{
            //    if (a.FullName.Equals(fileAssemblyName.FullName, StringComparison.InvariantCultureIgnoreCase))
            //        return true;
            //}
            //return false;

            //do not compare the full assembly name, just filename
            try
            {
                string fileNameWithoutExt = Path.GetFileNameWithoutExtension(fileInfo.FullName);
                if (fileNameWithoutExt == null)
                    throw new Exception(string.Format("Cannot get file extension for {0}", fileInfo.Name));
                foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
                {
                    string assemblyName = a.FullName.Split(new[] { ',' }).FirstOrDefault();
                    if (fileNameWithoutExt.Equals(assemblyName, StringComparison.InvariantCultureIgnoreCase))
                        return true;
                }
            }
            catch (Exception exc)
            {
                Debug.WriteLine("Cannot validate whether an assembly is already loaded. " + exc);
            }
            return false;
        }

        #endregion

    }
}
