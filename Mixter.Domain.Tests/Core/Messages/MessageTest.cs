﻿using System.Linq;
using Mixter.Domain.Core.Messages;
using Mixter.Domain.Core.Messages.Events;
using Mixter.Domain.Identity;
using NFluent;
using Xunit;

namespace Mixter.Domain.Tests.Core.Messages
{
    public class MessageTest
    {
        private const string MessageContent = "Hello miixit";

        private static readonly UserId Author = new UserId("pierre@mixit.fr");
        private static readonly UserId Requacker = new UserId("alfred@mixit.fr");
        private static readonly MessageId MessageId = MessageId.Generate();

        private readonly EventPublisherFake _eventPublisher;

        public MessageTest()
        {
            _eventPublisher = new EventPublisherFake();
        }

        [Fact]
        public void WhenQuackMessageThenRaiseUserMessageQuacked()
        {
            Message.Quack(_eventPublisher, Author, MessageContent);

            var evt = (MessageQuacked) _eventPublisher.Events.First();
            Check.That(evt.Content).IsEqualTo(MessageContent);
            Check.That(evt.Author).IsEqualTo(Author);
        }

        [Fact]
        public void WhenQuackSeveralMessageThenMessageIdIsNotSame()
        {
            Message.Quack(_eventPublisher, Author, MessageContent);
            Message.Quack(_eventPublisher, Author, MessageContent);

            var events = _eventPublisher.Events.OfType<MessageQuacked>().ToArray();
            Check.That(events[0].Id).IsNotEqualTo(events[1].Id);
        }

        [Fact]
        public void WhenQuackMessageThenReturnMessageId()
        {
            var messageId = Message.Quack(_eventPublisher, Author, MessageContent);

            var evt = (MessageQuacked) _eventPublisher.Events.First();
            Check.That(evt.Id).IsEqualTo(messageId);
        }

        [Fact]
        public void WhenRequackMessageThenRaiseMessageRequacked()
        {
            var message = CreateMessage(new MessageQuacked(MessageId, Author, MessageContent));

            message.Requack(_eventPublisher, Requacker);

            Check.That(_eventPublisher.Events)
                .ContainsExactly(new MessageRequacked(MessageId, Requacker));
        }

        [Fact]
        public void WhenRequackMyOwnMessageThenDoNotRaiseMessageRequacked()
        {
            var message = CreateMessage(new MessageQuacked(MessageId, Author, MessageContent));

            message.Requack(_eventPublisher, Author);

            Check.That(_eventPublisher.Events).IsEmpty();
        }

        [Fact]
        public void WhenRequackTwoTimesSameMessageThenDoNotRaiseMessageRequacked()
        {
            var message = CreateMessage(
                new MessageQuacked(MessageId, Author, MessageContent),
                new MessageRequacked(MessageId, Requacker));

            message.Requack(_eventPublisher, Requacker);

            Check.That(_eventPublisher.Events).IsEmpty();
        }

        [Fact]
        public void WhenDeleteThenRaiseMessageDeleted()
        {
            var message = CreateMessage(new MessageQuacked(MessageId, Author, MessageContent));

            message.Delete(_eventPublisher, Author);

            Check.That(_eventPublisher.Events)
                .ContainsExactly(new MessageDeleted(MessageId, Author));
        }

        [Fact]
        public void WhenDeleteBySomeoneElseThanAuthorThenDoNotRaiseMessageDeleted()
        {
            var message = CreateMessage(new MessageQuacked(MessageId, Author, MessageContent));

            message.Delete(_eventPublisher, new UserId("clement@mix-it.fr"));

            Check.That(_eventPublisher.Events).IsEmpty();
        }

        [Fact]
        public void GivenDeletedMessageWhenDeleteThenNothing()
        {
            var message = CreateMessage(
                new MessageQuacked(MessageId, Author, MessageContent),
                new MessageDeleted(MessageId, Author));

            message.Delete(_eventPublisher, Author);

            Check.That(_eventPublisher.Events).IsEmpty();
        }

        [Fact]
        public void GivenDeletedMessageWhenRequackThenDoNotRaiseMessageRequacked()
        {
            var message = CreateMessage(
                new MessageQuacked(MessageId, Author, MessageContent),
                new MessageDeleted(MessageId, Author));

            message.Requack(_eventPublisher, new UserId("emilien@mix-it.fr"));

            Check.That(_eventPublisher.Events).IsEmpty();
        }

        private Message CreateMessage(params IDomainEvent[] events)
        {
            return new Message(events);
        }
    }
}
