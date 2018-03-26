using System;
using RANSUROTTO.BLOG.Core.Infrastructure;
using RANSUROTTO.BLOG.Service.Logging;

namespace RANSUROTTO.BLOG.Service.Events
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
            throw new NotImplementedException();
        }

        public virtual void PublishToConsumer<T>(IConsumer<T> consumer, T eventMessage)
        {
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

    }
}