using Mixter.Domain.Core.Subscriptions.Events;
using Mixter.Domain.Identity;

namespace Mixter.Domain.Core.Subscriptions
{
    [Aggregate]
    public class Subscription
    {
        public static void FollowUser(IEventPublisher eventPublisher, UserId follower, UserId followee)
        {
            var suscriptionId=new SubscriptionId(follower, followee);
            eventPublisher.Publish(new UserFollowed(suscriptionId));
        }
    }
}
