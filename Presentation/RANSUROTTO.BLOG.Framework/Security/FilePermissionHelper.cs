using System.Collections.Generic;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using RANSUROTTO.BLOG.Core.Helper;

namespace RANSUROTTO.BLOG.Framework.Security
{
    /// <summary>
    /// 文件权限帮助类
    /// </summary>
    public static class FilePermissionHelper
    {

        /// <summary>
        /// 检查指定目标的权限
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="checkRead">检查是否可读</param>
        /// <param name="checkWrite">检查是否可写</param>
        /// <param name="checkModify">检查是否可移动</param>
        /// <param name="checkDelete">检查是否可删除</param>
        /// <returns>结果</returns>
        public static bool CheckPermissions(string path, bool checkRead, bool checkWrite, bool checkModify, bool checkDelete)
        {
            bool flag = false;
            bool flag2 = false;
            bool flag3 = false;
            bool flag4 = false;
            bool flag5 = false;
            bool flag6 = false;
            bool flag7 = false;
            bool flag8 = false;
            WindowsIdentity current = WindowsIdentity.GetCurrent();
            AuthorizationRuleCollection rules;
            try
            {
                rules = Directory.GetAccessControl(path).GetAccessRules(true, true, typeof(SecurityIdentifier));
            }
            catch
            {
                return true;
            }
            try
            {
                foreach (FileSystemAccessRule rule in rules)
                {
                    if (!current.User.Equals(rule.IdentityReference))
                    {
                        continue;
                    }
                    if (AccessControlType.Deny.Equals(rule.AccessControlType))
                    {
                        if ((FileSystemRights.Delete & rule.FileSystemRights) == FileSystemRights.Delete)
                            flag4 = true;
                        if ((FileSystemRights.Modify & rule.FileSystemRights) == FileSystemRights.Modify)
                            flag3 = true;

                        if ((FileSystemRights.Read & rule.FileSystemRights) == FileSystemRights.Read)
                            flag = true;

                        if ((FileSystemRights.Write & rule.FileSystemRights) == FileSystemRights.Write)
                            flag2 = true;

                        continue;
                    }
                    if (AccessControlType.Allow.Equals(rule.AccessControlType))
                    {
                        if ((FileSystemRights.Delete & rule.FileSystemRights) == FileSystemRights.Delete)
                        {
                            flag8 = true;
                        }
                        if ((FileSystemRights.Modify & rule.FileSystemRights) == FileSystemRights.Modify)
                        {
                            flag7 = true;
                        }
                        if ((FileSystemRights.Read & rule.FileSystemRights) == FileSystemRights.Read)
                        {
                            flag5 = true;
                        }
                        if ((FileSystemRights.Write & rule.FileSystemRights) == FileSystemRights.Write)
                        {
                            flag6 = true;
                        }
                    }
                }
                foreach (IdentityReference reference in current.Groups)
                {
                    foreach (FileSystemAccessRule rule2 in rules)
                    {
                        if (!reference.Equals(rule2.IdentityReference))
                        {
                            continue;
                        }
                        if (AccessControlType.Deny.Equals(rule2.AccessControlType))
                        {
                            if ((FileSystemRights.Delete & rule2.FileSystemRights) == FileSystemRights.Delete)
                                flag4 = true;
                            if ((FileSystemRights.Modify & rule2.FileSystemRights) == FileSystemRights.Modify)
                                flag3 = true;
                            if ((FileSystemRights.Read & rule2.FileSystemRights) == FileSystemRights.Read)
                                flag = true;
                            if ((FileSystemRights.Write & rule2.FileSystemRights) == FileSystemRights.Write)
                                flag2 = true;
                            continue;
                        }
                        if (AccessControlType.Allow.Equals(rule2.AccessControlType))
                        {
                            if ((FileSystemRights.Delete & rule2.FileSystemRights) == FileSystemRights.Delete)
                                flag8 = true;
                            if ((FileSystemRights.Modify & rule2.FileSystemRights) == FileSystemRights.Modify)
                                flag7 = true;
                            if ((FileSystemRights.Read & rule2.FileSystemRights) == FileSystemRights.Read)
                                flag5 = true;
                            if ((FileSystemRights.Write & rule2.FileSystemRights) == FileSystemRights.Write)
                                flag6 = true;
                        }
                    }
                }
                bool flag9 = !flag4 && flag8;
                bool flag10 = !flag3 && flag7;
                bool flag11 = !flag && flag5;
                bool flag12 = !flag2 && flag6;
                bool flag13 = true;
                if (checkRead)
                {
                    //flag13 = flag13 && flag11;
                    flag13 = flag11;
                }
                if (checkWrite)
                {
                    flag13 = flag13 && flag12;
                }
                if (checkModify)
                {
                    flag13 = flag13 && flag10;
                }
                if (checkDelete)
                {
                    flag13 = flag13 && flag9;
                }
                return flag13;
            }
            catch (IOException)
            {
            }
            return false;
        }

        /// <summary>
        /// 获取需要写入权限的目录（物理路径）列表
        /// </summary>
        /// <returns>结果</returns>
        public static IEnumerable<string> GetDirectoriesWrite()
        {
            string rootDir = CommonHelper.MapPath("~/");
            var dirsToCheck = new List<string>();
            //dirsToCheck.Add(rootDir);
            dirsToCheck.Add(Path.Combine(rootDir, "App_Data"));
            dirsToCheck.Add(Path.Combine(rootDir, "Administration\\db_backups"));
            dirsToCheck.Add(Path.Combine(rootDir, "bin"));
            dirsToCheck.Add(Path.Combine(rootDir, "content"));
            dirsToCheck.Add(Path.Combine(rootDir, "content\\images"));
            dirsToCheck.Add(Path.Combine(rootDir, "content\\images\\thumbs"));
            dirsToCheck.Add(Path.Combine(rootDir, "content\\images\\uploaded"));
            dirsToCheck.Add(Path.Combine(rootDir, "content\\files\\exportimport"));
            dirsToCheck.Add(Path.Combine(rootDir, "plugins"));
            dirsToCheck.Add(Path.Combine(rootDir, "plugins\\bin"));
            return dirsToCheck;
        }

        /// <summary>
        /// 获取需要写入权限的文件列表（物理路径）
        /// </summary>
        /// <returns>结果</returns>
        public static IEnumerable<string> GetFilesWrite()
        {
            string rootDir = CommonHelper.MapPath("~/");
            var filesToCheck = new List<string>();
            filesToCheck.Add(Path.Combine(rootDir, "Global.asax"));
            filesToCheck.Add(Path.Combine(rootDir, "web.config"));
            filesToCheck.Add(Path.Combine(rootDir, "App_Data\\InstalledPlugins.txt"));
            filesToCheck.Add(Path.Combine(rootDir, "App_Data\\Settings.txt"));
            return filesToCheck;
        }

    }
}
