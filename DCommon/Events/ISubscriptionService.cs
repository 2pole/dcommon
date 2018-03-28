using System.Collections.Generic;

namespace DCommon.Events
{
    public interface ISubscriptionService
    {
        IList<IConsumer<T>> GetSubscriptions<T>();
    }
}
