using System.ComponentModel;
using RANSUROTTO.BLOG.Core.Context;
using RANSUROTTO.BLOG.Core.Infrastructure;
using RANSUROTTO.BLOG.Framework.Mvc;
using RANSUROTTO.BLOG.Service.Localization;

namespace RANSUROTTO.BLOG.Framework
{
    public class ResourceDisplayName : DisplayNameAttribute, IModelAttribute
    {

        private string _resourceValue = string.Empty;

        public string ResourceKey { get; set; }

        public string Name => "ResourceDisplayName";

        public ResourceDisplayName(string resourceKey) : base(resourceKey)
        {
            ResourceKey = resourceKey;
        }

        public override string DisplayName
        {
            get
            {
                var langId = EngineContext.Current.Resolve<IWorkContext>().WorkingLanguage.Id;
                _resourceValue = EngineContext.Current
                    .Resolve<ILocalizationService>()
                    .GetResource(ResourceKey, langId, true, ResourceKey);

                return _resourceValue;
            }
        }

    }
}
