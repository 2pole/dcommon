using System;
using System.Linq;
using Common.Logging;

namespace DCommon.Events
{
    public class EventPublisher : IEventPublisher
    {
        private readonly ILog _logger = LogManager.GetLogger<EventPublisher>();
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

        private void PublishToConsumer<T>(IConsumer<T> x, T eventMessage)
        {
            try
            {
                x.HandleEvent(eventMessage);
            }
            catch (Exception exc)
            {
                //log error
                //we put in to nested try-catch to prevent possible cyclic (if some error occurs)
                try
                {
                    _logger.Error(exc.Message, exc);
                }
                catch (Exception)
                {
                    //do nothing
                }
            }
            finally
            {
                //TODO actually we should not dispose it
                var instance = x as IDisposable;
                if (instance != null)
                {
                    instance.Dispose();
                }
            }
        }
    }
}
