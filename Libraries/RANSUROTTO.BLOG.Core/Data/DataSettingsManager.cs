﻿using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using RANSUROTTO.BLOG.Core.Helper;

namespace RANSUROTTO.BLOG.Core.Data
{

    /// <summary>
    /// 数据源设置管理
    /// </summary>
    public class DataSettingsManager
    {

        #region Field

        /// <summary>
        /// 键/值 字符串分隔符
        /// </summary>
        protected const char Separator = ':';

        /// <summary>
        /// 数据源设置 信息存储文件
        /// </summary>
        protected const string Filename = "db.config";

        #endregion

        #region Methods

        /// <summary>
        /// 将字符串解析为数据源设置实例
        /// </summary>
        /// <param name="settingString">数据源设置字符串</param>
        /// <returns>数据源设置实例</returns>
        protected virtual DataSettings ParseSettins(string settingString)
        {
            var shellSettings = new DataSettings();
            if (string.IsNullOrEmpty(settingString))
                return shellSettings;

            #region Old Method -> String Store
            /*
            var settings = new List<string>();
            using (var reader = new StringReader(settingString))
            {
                string str;
                while (!string.IsNullOrEmpty(str = reader.ReadLine()))
                {
                    settings.Add(str);
                }
            }

            foreach (var setting in settings)
            {
                var separatorIndex = setting.IndexOf(separator);
                if (separatorIndex == -1)
                    continue;

                string key = setting.Substring(0, separatorIndex).Trim();
                string value = setting.Substring(separatorIndex + 1).Trim();

                PropertyInfo property = typeof(DataSettings).GetProperty(key);
                if (property != null && property.CanWrite)
                {
                    property.SetValue(shellSettings, value);
                }
                else
                {
                    shellSettings.RawDataSettings.Add(key, value);
                }
            }*/
            #endregion

            var xElement = XDocument.Parse(settingString).Root;
            var items = xElement?.Elements("item").ToList();

            if (items == null)
                return shellSettings;

            shellSettings.DataProvider = items.FirstOrDefault(p => p.Attribute("key")?.Value == "DataProvider")?.Attribute("value")?.Value;
            shellSettings.DataConnectionString = items.FirstOrDefault(p => p.Attribute("key")?.Value == "DataConnectionString")?.Attribute("value")?.Value;

            return shellSettings;
        }

        /// <summary>
        /// 将数据源设置实例解析为字符串
        /// </summary>
        /// <param name="shellSettings">数据源设置实例</param>
        /// <returns>数据源设置字符串</returns>
        protected virtual string ComposeSettings(DataSettings shellSettings)
        {
            if (shellSettings == null)
                return string.Empty;

            var xmlDocument = new XDocument(
                new XElement(
                    "configuration",
                    new XElement("item", new XAttribute("key", "DataProvider"), new XAttribute("value", shellSettings.DataProvider ?? string.Empty)),
                    new XElement("item", new XAttribute("key", "DataConnectionString"), new XAttribute("value", shellSettings.DataConnectionString ?? string.Empty))
                    )
                );
            shellSettings.RawDataSettings?.Keys.ToList().ForEach(p =>
            {
                var xElement = new XElement("item", new XAttribute("key", p), new XAttribute("key", shellSettings.RawDataSettings[p]));
                xmlDocument.Root?.Add(xElement);
            });

            #region Old Method 
            /* var settingString = new StringBuilder();
             foreach (var propertiy in typeof(DataSettings).GetProperties())
             {
                 if (propertiy.PropertyType.IsCSharpBasicTypeOrOtherBasicType())
                 {
                     settingString.AppendFormat("{0}:{1}{2}"
                         , propertiy.Name
                         , propertiy.GetValue(shellSettings),
                         Environment.NewLine);
                 }
             }*/
            #endregion

            return xmlDocument.ToString();
        }

        /// <summary>
        /// 加载数据源设置实例
        /// </summary>
        public virtual DataSettings LoadSettings(string filePath = null)
        {
            if (string.IsNullOrEmpty(filePath))
                filePath = Path.Combine(CommonHelper.MapPath("~/App_Data/"), Filename);

            if (File.Exists(filePath))
            {
                string text = File.ReadAllText(filePath);
                return ParseSettins(text);
            }

            return new DataSettings();
        }

        /// <summary>
        /// 保存数据源设置实例信息到文件
        /// </summary>
        /// <param name="settings">数据源设置实例</param>
        /// <param name="filePath">文件存储目录</param>
        public virtual void SaveSettings(DataSettings settings, string filePath = null)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            if (string.IsNullOrEmpty(filePath))
                filePath = Path.Combine(CommonHelper.MapPath("~/App_Data/"), Filename);

            //如果不存在则创建
            if (!File.Exists(filePath))
            {
                using (File.Create(filePath))
                {

                }
            }

            File.WriteAllText(filePath, this.ComposeSettings(settings));
        }

        #endregion

    }

}
