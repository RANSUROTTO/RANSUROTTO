using System;
using System.Web;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Collections.Generic;
using RANSUROTTO.BLOG.Core.Common;
using System.Security.Cryptography;
using RANSUROTTO.BLOG.Core.Domain.Seo.Enum;
using RANSUROTTO.BLOG.Core.Domain.Seo.Setting;

namespace RANSUROTTO.BLOG.Framework.UI
{
    public class PageHeadBuilder : IPageHeadBuilder
    {

        #region Fields

        private static readonly object s_lock = new object();
        private readonly SeoSettings _seoSettings;
        private readonly List<string> _titleParts;
        private readonly List<string> _metaDescriptionParts;
        private readonly List<string> _metaKeywordParts;
        private readonly Dictionary<ResourceLocation, List<ScriptReferenceMeta>> _scriptParts;
        private readonly Dictionary<ResourceLocation, List<CssReferenceMeta>> _cssParts;
        private readonly List<string> _canonicalUrlParts;
        private readonly List<string> _headCustomParts;
        private readonly List<string> _pageCssClassParts;
        private string _editPageUrl;
        private string _activeAdminMenuSystemName;

        #endregion

        #region Constructor

        public PageHeadBuilder(SeoSettings seoSettings)
        {
            this._seoSettings = seoSettings;
            this._titleParts = new List<string>();
            this._metaDescriptionParts = new List<string>();
            this._metaKeywordParts = new List<string>();
            this._scriptParts = new Dictionary<ResourceLocation, List<ScriptReferenceMeta>>();
            this._cssParts = new Dictionary<ResourceLocation, List<CssReferenceMeta>>();
            this._canonicalUrlParts = new List<string>();
            this._headCustomParts = new List<string>();
            this._pageCssClassParts = new List<string>();
        }

        #endregion

        #region Methods

        public virtual void AddTitleParts(string part)
        {
            if (string.IsNullOrEmpty(part))
                return;

            _titleParts.Add(part);
        }
        public virtual void AppendTitleParts(string part)
        {
            if (string.IsNullOrEmpty(part))
                return;

            _titleParts.Insert(0, part);
        }
        public virtual string GenerateTitle(bool addDefaultTitle)
        {
            string result;
            var specificTitle = string.Join(_seoSettings.PageTitleSeparator, _titleParts.AsEnumerable().Reverse().ToArray());
            if (!string.IsNullOrEmpty(specificTitle))
            {
                if (addDefaultTitle)
                {
                    switch (_seoSettings.PageTitleSeoAdjustment)
                    {
                        case PageTitleSeoAdjustment.PagenameAfterBlogname:
                            {
                                result = string.Join(_seoSettings.PageTitleSeparator, _seoSettings.DefaultTitle, specificTitle);
                            }
                            break;
                        case PageTitleSeoAdjustment.BlognameAfterPagename:
                        default:
                            {
                                result = string.Join(_seoSettings.PageTitleSeparator, specificTitle, _seoSettings.DefaultTitle);
                            }
                            break;

                    }
                }
                else
                {
                    result = specificTitle;
                }
            }
            else
            {
                result = _seoSettings.DefaultTitle;
            }
            return result;
        }

        public virtual void AddMetaDescriptionParts(string part)
        {
            if (string.IsNullOrEmpty(part))
                return;

            _metaDescriptionParts.Add(part);
        }
        public virtual void AppendMetaDescriptionParts(string part)
        {
            if (string.IsNullOrEmpty(part))
                return;

            _metaDescriptionParts.Insert(0, part);
        }
        public virtual string GenerateMetaDescription()
        {
            var metaDescription = string.Join(", ", _metaDescriptionParts.AsEnumerable().Reverse().ToArray());
            var result = !String.IsNullOrEmpty(metaDescription) ? metaDescription : _seoSettings.DefaultMetaDescription;
            return result;
        }

        public virtual void AddMetaKeywordParts(string part)
        {
            if (string.IsNullOrEmpty(part))
                return;

            _metaKeywordParts.Add(part);
        }
        public virtual void AppendMetaKeywordParts(string part)
        {
            if (string.IsNullOrEmpty(part))
                return;

            _metaKeywordParts.Insert(0, part);
        }
        public virtual string GenerateMetaKeywords()
        {
            var metaKeyword = string.Join(", ", _metaKeywordParts.AsEnumerable().Reverse().ToArray());
            var result = !String.IsNullOrEmpty(metaKeyword) ? metaKeyword : _seoSettings.DefaultMetaKeywords;
            return result;
        }

        public virtual void AddScriptParts(ResourceLocation location, string part, bool excludeFromBundle, bool isAsync)
        {
            if (!_scriptParts.ContainsKey(location))
                _scriptParts.Add(location, new List<ScriptReferenceMeta>());

            if (string.IsNullOrEmpty(part))
                return;

            _scriptParts[location].Add(new ScriptReferenceMeta
            {
                ExcludeFromBundle = excludeFromBundle,
                IsAsync = isAsync,
                Part = part
            });
        }
        public virtual void AppendScriptParts(ResourceLocation location, string part, bool excludeFromBundle, bool isAsync)
        {
            if (!_scriptParts.ContainsKey(location))
                _scriptParts.Add(location, new List<ScriptReferenceMeta>());

            if (string.IsNullOrEmpty(part))
                return;

            _scriptParts[location].Insert(0, new ScriptReferenceMeta
            {
                ExcludeFromBundle = excludeFromBundle,
                IsAsync = isAsync,
                Part = part
            });
        }
        public virtual string GenerateScripts(UrlHelper urlHelper, ResourceLocation location, bool? bundleFiles = null)
        {
            if (!_scriptParts.ContainsKey(location) || _scriptParts[location] == null)
                return "";

            if (!_scriptParts.Any())
                return "";

            if (!bundleFiles.HasValue)
            {
                bundleFiles = _seoSettings.EnableJsBundling && BundleTable.EnableOptimizations;
            }
            if (bundleFiles.Value)
            {
                var partsToBundle = _scriptParts[location]
                    .Where(x => !x.ExcludeFromBundle)
                    .Select(x => x.Part)
                    .Distinct()
                    .ToArray();
                var partsToDontBundle = _scriptParts[location]
                    .Where(x => x.ExcludeFromBundle)
                    .Select(x => new { x.Part, x.IsAsync })
                    .Distinct()
                    .ToArray();


                var result = new StringBuilder();

                if (partsToBundle.Length > 0)
                {
                    string bundleVirtualPath = GetBundleVirtualPath("~/bundles/scripts/", ".js", partsToBundle);

                    lock (s_lock)
                    {
                        var bundleFor = BundleTable.Bundles.GetBundleFor(bundleVirtualPath);
                        if (bundleFor == null)
                        {
                            var bundle = new ScriptBundle(bundleVirtualPath);

                            bundle.Orderer = new AsIsBundleOrderer();
                            bundle.EnableFileExtensionReplacements = false;
                            bundle.Include(partsToBundle);
                            BundleTable.Bundles.Add(bundle);
                        }
                    }

                    result.AppendLine(Scripts.Render(bundleVirtualPath).ToString());
                }

                foreach (var item in partsToDontBundle)
                {
                    result.AppendFormat("<script {2}src=\"{0}\" type=\"{1}\"></script>", urlHelper.Content(item.Part), MimeTypes.TextJavascript, item.IsAsync ? "async " : "");
                    result.Append(Environment.NewLine);
                }
                return result.ToString();
            }
            else
            {
                var result = new StringBuilder();
                foreach (var item in _scriptParts[location].Select(x => new { x.Part, x.IsAsync }).Distinct())
                {
                    result.AppendFormat("<script {2}src=\"{0}\" type=\"{1}\"></script>", urlHelper.Content(item.Part), MimeTypes.TextJavascript, item.IsAsync ? "async " : "");
                    result.Append(Environment.NewLine);
                }
                return result.ToString();
            }
        }

        public virtual void AddCssFileParts(ResourceLocation location, string part, bool excludeFromBundle = false)
        {
            if (!_cssParts.ContainsKey(location))
                _cssParts.Add(location, new List<CssReferenceMeta>());

            if (string.IsNullOrEmpty(part))
                return;

            _cssParts[location].Add(new CssReferenceMeta
            {
                ExcludeFromBundle = excludeFromBundle,
                Part = part
            });
        }
        public virtual void AppendCssFileParts(ResourceLocation location, string part, bool excludeFromBundle = false)
        {
            if (!_cssParts.ContainsKey(location))
                _cssParts.Add(location, new List<CssReferenceMeta>());

            if (string.IsNullOrEmpty(part))
                return;

            _cssParts[location].Insert(0, new CssReferenceMeta
            {
                ExcludeFromBundle = excludeFromBundle,
                Part = part
            });
        }
        public virtual string GenerateCssFiles(UrlHelper urlHelper, ResourceLocation location, bool? bundleFiles = null)
        {
            if (!_cssParts.ContainsKey(location) || _cssParts[location] == null)
                return "";

            if (!_cssParts.Any())
                return "";

            if (!bundleFiles.HasValue)
            {
                bundleFiles = _seoSettings.EnableCssBundling && BundleTable.EnableOptimizations;
            }
            if (bundleFiles.Value)
            {
                var partsToBundle = _cssParts[location]
                    .Where(x => !x.ExcludeFromBundle)
                    .Select(x => x.Part)
                    .Distinct()
                    .ToArray();
                var partsToDontBundle = _cssParts[location]
                    .Where(x => x.ExcludeFromBundle)
                    .Select(x => x.Part)
                    .Distinct()
                    .ToArray();


                var result = new StringBuilder();

                if (partsToBundle.Length > 0)
                {
                    string bundleVirtualPath = GetBundleVirtualPath("~/bundles/styles/", ".css", partsToBundle);

                    lock (s_lock)
                    {
                        var bundleFor = BundleTable.Bundles.GetBundleFor(bundleVirtualPath);
                        if (bundleFor == null)
                        {
                            var bundle = new StyleBundle(bundleVirtualPath);

                            bundle.Orderer = new AsIsBundleOrderer();
                            bundle.EnableFileExtensionReplacements = false;
                            foreach (var ptb in partsToBundle)
                            {
                                bundle.Include(ptb, GetCssTranform());
                            }
                            BundleTable.Bundles.Add(bundle);
                        }
                    }

                    result.AppendLine(Styles.Render(bundleVirtualPath).ToString());
                }

                foreach (var item in partsToDontBundle)
                {
                    result.AppendFormat("<link href=\"{0}\" rel=\"stylesheet\" type=\"{1}\" />", urlHelper.Content(item), MimeTypes.TextCss);
                    result.Append(Environment.NewLine);
                }

                return result.ToString();
            }
            else
            {
                var result = new StringBuilder();
                foreach (var path in _cssParts[location].Select(x => x.Part).Distinct())
                {
                    result.AppendFormat("<link href=\"{0}\" rel=\"stylesheet\" type=\"{1}\" />", urlHelper.Content(path), MimeTypes.TextCss);
                    result.AppendLine();
                }
                return result.ToString();
            }
        }

        public virtual void AddCanonicalUrlParts(string part)
        {
            if (string.IsNullOrEmpty(part))
                return;

            _canonicalUrlParts.Add(part);
        }
        public virtual void AppendCanonicalUrlParts(string part)
        {
            if (string.IsNullOrEmpty(part))
                return;

            _canonicalUrlParts.Insert(0, part);
        }
        public virtual string GenerateCanonicalUrls()
        {
            var result = new StringBuilder();
            foreach (var canonicalUrl in _canonicalUrlParts)
            {
                result.AppendFormat("<link rel=\"canonical\" href=\"{0}\" />", canonicalUrl);
                result.Append(Environment.NewLine);
            }
            return result.ToString();
        }

        public virtual void AddHeadCustomParts(string part)
        {
            if (string.IsNullOrEmpty(part))
                return;

            _headCustomParts.Add(part);
        }
        public virtual void AppendHeadCustomParts(string part)
        {
            if (string.IsNullOrEmpty(part))
                return;

            _headCustomParts.Insert(0, part);
        }
        public virtual string GenerateHeadCustom()
        {
            var distinctParts = _headCustomParts.Distinct().ToList();
            if (!distinctParts.Any())
                return "";

            var result = new StringBuilder();
            foreach (var path in distinctParts)
            {
                result.Append(path);
                result.Append(Environment.NewLine);
            }
            return result.ToString();
        }

        public virtual void AddPageCssClassParts(string part)
        {
            if (string.IsNullOrEmpty(part))
                return;

            _pageCssClassParts.Add(part);
        }
        public virtual void AppendPageCssClassParts(string part)
        {
            if (string.IsNullOrEmpty(part))
                return;

            _pageCssClassParts.Insert(0, part);
        }
        public virtual string GeneratePageCssClasses()
        {
            string result = string.Join(" ", _pageCssClassParts.AsEnumerable().Reverse().ToArray());
            return result;
        }

        public virtual void AddEditPageUrl(string url)
        {
            _editPageUrl = url;
        }
        public virtual string GetEditPageUrl()
        {
            return _editPageUrl;
        }

        public virtual void SetActiveMenuItemSystemName(string systemName)
        {
            _activeAdminMenuSystemName = systemName;
        }
        public virtual string GetActiveMenuItemSystemName()
        {
            return _activeAdminMenuSystemName;
        }

        #endregion

        #region Utilities

        protected virtual string GetBundleVirtualPath(string prefix, string extension, string[] parts)
        {
            if (parts == null || parts.Length == 0)
                throw new ArgumentException("parts");

            var hash = "";
            using (SHA256 sha = new SHA256Managed())
            {
                var hashInput = "";
                foreach (var part in parts)
                {
                    hashInput += part;
                    hashInput += ",";
                }

                byte[] input = sha.ComputeHash(Encoding.Unicode.GetBytes(hashInput));
                hash = HttpServerUtility.UrlTokenEncode(input);
            }

            var sb = new StringBuilder(prefix);
            sb.Append(hash);

            return sb.ToString();
        }

        protected virtual IItemTransform GetCssTranform()
        {
            return new CssRewriteUrlTransform();
        }

        #endregion

        #region Nested classes

        private class ScriptReferenceMeta
        {
            public bool ExcludeFromBundle { get; set; }

            public bool IsAsync { get; set; }

            public string Part { get; set; }
        }

        private class CssReferenceMeta
        {
            public bool ExcludeFromBundle { get; set; }

            public string Part { get; set; }
        }

        #endregion

    }
}
