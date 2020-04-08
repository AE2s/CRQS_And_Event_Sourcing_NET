using System.Collections.Generic;
using System.Linq;
using Mixter.Domain.Core.Subscriptions.Events;
using Mixter.Domain.Identity;

namespace Mixter.Domain.Core.Subscriptions
{
    [Aggregate]
    public class Subscription
    {
        private IList<object> subscriptionIds;
        public Subscription(IDomainEvent[] events)
        {
            subscriptionIds=new List<object>();
            foreach (var @event in events)
            {
                subscriptionIds.Add(@event.GetAggregateId());
            }
        }

        public static void FollowUser(IEventPublisher eventPublisher, UserId follower, UserId followee)
        {
            var suscriptionId=new SubscriptionId(follower, followee);
            eventPublisher.Publish(new UserFollowed(suscriptionId));
        }

        public void Unfollow(IEventPublisher evt)
        {
            var id =(SubscriptionId)subscriptionIds.First();
            evt.Publish(new UserUnfollowed(id));
        }
    }
}
