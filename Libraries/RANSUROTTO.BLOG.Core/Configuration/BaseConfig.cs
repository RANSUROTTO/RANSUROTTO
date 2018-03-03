using System.Configuration;
using System.Xml;

namespace RANSUROTTO.BLOG.Core.Configuration
{
    public abstract class BaseConfig<T> : IConfigurationSectionHandler
    {
        protected T Config;

        public abstract T Create(XmlNode section);

        public object Create(object parent, object configContext, XmlNode section)
        {
            return Create(section);
        }

    }
}
