using Mixter.Domain.Core.Messages.Events;

namespace Mixter.Domain.Core.Subscriptions.Handlers
{
    [Handler]
    public class NotifyFollowerOfFolloweeMessage :
        IEventHandler<MessageQuacked>,
        IEventHandler<MessageRequacked>
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
            throw new System.NotImplementedException();
        }
    }
}
