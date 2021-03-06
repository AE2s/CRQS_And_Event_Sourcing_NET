﻿using System.Linq;
using Mixter.Domain.Core.Messages;
using Mixter.Domain.Core.Messages.Events;
using Mixter.Domain.Identity;
using Mixter.Domain.Tests;
using NFluent;
using Xunit;

namespace Mixter.Infrastructure.Tests
{
    public class MessagesRepositoryTest
    {
        private readonly EventsStore _eventsStore;
        private readonly MessagesRepository _repository;

        public MessagesRepositoryTest()
        {
            _eventsStore = new EventsStore();
            _repository = new MessagesRepository(_eventsStore);
        }

        [Fact]
        public void GivenMessageQuackedThenGetMessageThenReturnTheMessage()
        {
            var messageQuacked = new MessageQuacked(MessageId.Generate(), new UserId("bob@mixit.fr"), "Hello");
            _eventsStore.Store(messageQuacked);

            var message = _repository.Get(messageQuacked.Id);

            var eventsPublisher = new EventPublisherFake();
            message.Requack(eventsPublisher, new UserId("joe@mitit.fr"));
            Check.That(eventsPublisher.Events.Cast<MessageRequacked>().Single().Id).IsEqualTo(messageQuacked.Id);
        }

        [Fact]
        public void GivenMessageQuackedThenGetDescriptionThenReturnMessageDescription()
        {
            var messageQuacked = new MessageQuacked(MessageId.Generate(), new UserId("bob@mixit.fr"), "Hello");
            _eventsStore.Store(messageQuacked);

            var description = _repository.GetDescription(messageQuacked.Id);

            Check.That(description.Author).IsEqualTo(messageQuacked.Author);
            Check.That(description.Content).IsEqualTo(messageQuacked.Content);
        }

        [Fact]
        public void GivenNoEventsThenGetMessageThenThrowUnknownMessage()
        {
            Check.ThatCode(() => _repository.Get(MessageId.Generate())).Throws<UnknownMessage>();
        }

        [Fact]
        public void GivenNoEventsThenGetDescriptionThenThrowUnknownMessage()
        {
            Check.ThatCode(() => _repository.GetDescription(MessageId.Generate())).Throws<UnknownMessage>();
        }
    }
}
