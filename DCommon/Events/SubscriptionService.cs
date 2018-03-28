using CommonServiceLocator;
using System.Collections.Generic;
using System.Linq;

namespace DCommon.Events
{
    public class SubscriptionService : ISubscriptionService
    {
        public IList<IConsumer<T>> GetSubscriptions<T>()
        {
            var consumers = ServiceLocator.Current.GetAllInstances<IConsumer<T>>();
            var items = consumers.ToList();
            return items;
        }
    }
}
