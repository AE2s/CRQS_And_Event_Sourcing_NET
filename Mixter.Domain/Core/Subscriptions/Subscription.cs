using System.Collections.Generic;
using System.Linq;
using Mixter.Domain.Core.Messages;
using Mixter.Domain.Core.Subscriptions.Events;
using Mixter.Domain.Identity;

namespace Mixter.Domain.Core.Subscriptions
{
    [Aggregate]
    public class Subscription
    {
        DecisionProjection _projections = new DecisionProjection();
        public Subscription(IDomainEvent[] events)
        {


            foreach (var @event in events)
            {
                _projections.Apply(@event);
            }
        }

        public static void FollowUser(IEventPublisher eventPublisher, UserId follower, UserId followee)
        {
            var suscriptionId = new SubscriptionId(follower, followee);
            eventPublisher.Publish(new UserFollowed(suscriptionId));
        }

        public void Unfollow(IEventPublisher evt)
        {
           
            evt.Publish(new UserUnfollowed(_projections.SubscriptionId));
        }

        public void NotifyFollower(IEventPublisher evt, MessageId messageId)
        {
            if (!_projections.Quackers.Contains(_projections.SubscriptionId))
                return;
            evt.Publish(new FolloweeMessageQuacked(_projections.SubscriptionId, messageId));
        }

        [Projection]
        private class DecisionProjection : DecisionProjectionBase
        {
            private readonly IList<SubscriptionId> _quackers = new List<SubscriptionId>();

            public MessageId MessageId { get; private set; }

            public SubscriptionId SubscriptionId { get; private set; }

            public IEnumerable<SubscriptionId> Quackers
            {
                get { return _quackers; }
            }

            public DecisionProjection()
            {
                AddHandler<UserFollowed>(When);
                AddHandler<UserUnfollowed>(When);
                AddHandler<FolloweeMessageQuacked>(When);
            }

            private void When(UserFollowed evt)
            {
                SubscriptionId = evt.SubscriptionId;

                _quackers.Add(evt.SubscriptionId);
            }

            private void When(UserUnfollowed evt)
            {
                if (_quackers.Contains(evt.SubscriptionId))
                {
                    SubscriptionId = evt.SubscriptionId;
                    _quackers.Remove(evt.SubscriptionId);
                }
            }

            private void When(FolloweeMessageQuacked evt)
            {
                if (!_quackers.Contains(evt.SubscriptionId))
                    return;

                SubscriptionId = evt.SubscriptionId;
                MessageId = evt.MessageId;
               

            }
        }
    }


}
