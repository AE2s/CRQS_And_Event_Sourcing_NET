using Mixter.Domain.Core.Messages.Events;
using Mixter.Domain.Core.Subscriptions.Events;

namespace Mixter.Domain.Core.Subscriptions.Handlers
{
    [Handler]
    public class NotifyFollowerOfFolloweeMessage
    {
        private IFollowersRepository _followersRepository;
        private ISubscriptionsRepository _subscriptionsRepository;
        private IEventPublisher _eventPublisher;

        public NotifyFollowerOfFolloweeMessage(IFollowersRepository followersRepository, ISubscriptionsRepository subscriptionsRepository, IEventPublisher eventPublisher)
        {
            _followersRepository = followersRepository;
            _subscriptionsRepository = subscriptionsRepository;
            _eventPublisher = eventPublisher;
        }

        public void Handle(MessageQuacked evt)
        {
            var userSubscriptions = _subscriptionsRepository.GetSubscriptionsOfUser(evt.Author);
            
            foreach (var userSubscription in userSubscriptions)
            {
               userSubscription.NotifyFollower(_eventPublisher, evt.Id);
            }
            
        }

        public void Handle(MessageRequacked evt)
        {
            var userSubscriptions = _subscriptionsRepository.GetSubscriptionsOfUser(evt.Requacker);
            
            foreach (var userSubscription in userSubscriptions)
            {
                userSubscription.NotifyFollower(_eventPublisher, evt.Id);
            }

        }
    }
}
