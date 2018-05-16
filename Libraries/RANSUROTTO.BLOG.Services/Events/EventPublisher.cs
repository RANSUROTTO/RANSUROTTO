using System;
using System.Linq;
using RANSUROTTO.BLOG.Core.Infrastructure;
using RANSUROTTO.BLOG.Core.Plugins;
using RANSUROTTO.BLOG.Services.Logging;

namespace RANSUROTTO.BLOG.Services.Events
{
    public class EventPublisher : IEventPublisher
    {

        private readonly ISubscriptionService _subscriptionService;

        public EventPublisher(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        public void Publish<T>(T eventMessage)
        {
            var subscriptions = _subscriptionService.GetSubscriptions<T>();
            subscriptions.ToList().ForEach(x => PublishToConsumer(x, eventMessage));
        }

        public virtual void PublishToConsumer<T>(IConsumer<T> consumer, T eventMessage)
        {
            /*var plugin = FindPlugin(consumer.GetType());
            if (plugin != null && !plugin.Installed)
                return;*/

            try
            {
                consumer.HandleEvent(eventMessage);
            }
            catch (Exception exc)
            {
                var logger = EngineContext.Current.Resolve<ILogger>();
                try
                {
                    //记录发布事件错误
                    logger.Error(exc.Message, exc);
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        protected virtual PluginDescriptor FindPlugin(Type providerType)
        {
            if (providerType == null)
                throw new ArgumentNullException(nameof(providerType));

            if (PluginManager.ReferencedPlugins == null)
                return null;

            foreach (var plugin in PluginManager.ReferencedPlugins)
            {
                if (plugin.ReferencedAssembly == null)
                    continue;

                if (plugin.ReferencedAssembly.FullName == providerType.Assembly.FullName)
                    return plugin;
            }

            return null;
        }

    }
}