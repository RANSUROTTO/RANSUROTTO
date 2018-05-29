using System.IO;
using RANSUROTTO.BLOG.Core.Context;
using RANSUROTTO.BLOG.Core.Helper;
using RANSUROTTO.BLOG.Core.Infrastructure;

namespace RANSUROTTO.BLOG.Framework.UI
{
    public static class TinyMceHelper
    {

        public static string GetThinyMceLnaguage()
        {
            var workContext = EngineContext.Current.Resolve<IWorkContext>();

            var languageCulture = workContext.WorkingLanguage.LanguageCulture;

            var langFile = string.Format("{0}.js", languageCulture);
            var path = CommonHelper.MapPath("~/Content/tinymce/langs/");
            var fileExists = File.Exists(string.Format("{0}{1}", path, langFile));

            if (!fileExists)
            {
                languageCulture = languageCulture.Replace('-', '_');
                langFile = string.Format("{0}.js", languageCulture);
                fileExists = File.Exists(string.Format("{0}{1}", path, langFile));
            }

            if (!fileExists)
            {
                languageCulture = languageCulture.Split('_', '-')[0];
                langFile = string.Format("{0}.js", languageCulture);
                fileExists = File.Exists(string.Format("{0}{1}", path, langFile));
            }

            return fileExists ? languageCulture : string.Empty;
        }

    }
}
