using System.Collections.Generic;
using System.Linq;
using Mixter.Domain.Core.Messages.Events;
using Mixter.Domain.Identity;

namespace Mixter.Domain.Core.Messages
{
    [Aggregate]
    public class Message
    {
        private readonly DecisionProjection _projection = new DecisionProjection();

        public Message(IEnumerable<IDomainEvent> events)
        {
            foreach (var @event in events)
            {
                _projection.Apply(@event);
            }
        }

        [Command]
        public static MessageId Quack(IEventPublisher eventPublisher, UserId author, string content)
        {
            var messageId = MessageId.Generate();
            eventPublisher.Publish(new MessageQuacked(messageId, author, content));
            return messageId;
        }

        [Command]
        public void Requack(IEventPublisher eventPublisher, UserId requacker)
        {
            if (_projection.Quackers.Contains(requacker))
            {
                return;
            }

            var evt = new MessageRequacked(_projection.Id, requacker);
            eventPublisher.Publish(evt);
        }

        [Command]
        public void Delete(IEventPublisher eventPublisher, UserId userId)
        {
            if (!_projection.Quackers.Contains(userId))
            {
                return;
            }


            var evt = new MessageDeleted(_projection.Id, userId);

            eventPublisher.Publish(evt);

        }

        [Projection]
        private class DecisionProjection : DecisionProjectionBase
        {
            private readonly IList<UserId> _quackers = new List<UserId>();

            public MessageId Id { get; private set; }

            public UserId Author { get; private set; }

            public IEnumerable<UserId> Quackers
            {
                get { return _quackers; }
            }

            public DecisionProjection()
            {
                AddHandler<MessageQuacked>(When);
                AddHandler<MessageRequacked>(When);
                AddHandler<MessageDeleted>(When);
            }

            private void When(MessageQuacked evt)
            {
                Id = evt.Id;
                Author = evt.Author;
                _quackers.Add(evt.Author);
            }

            private void When(MessageRequacked evt)
            {
                _quackers.Add(evt.Requacker);
            }

            private void When(MessageDeleted evt)
            {
                if (_quackers.Contains(evt.Deleter))
                    _quackers.Remove(evt.Deleter);
            }
        }

    }
}
