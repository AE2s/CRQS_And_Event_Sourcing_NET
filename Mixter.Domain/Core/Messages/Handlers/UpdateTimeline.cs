using Mixter.Domain.Core.Messages.Events;

namespace Mixter.Domain.Core.Messages.Handlers
{
    [Handler]
    public class UpdateTimeline : 
        IEventHandler<MessageQuacked>
    {
        private readonly ITimelineMessageRepository _timelineMessageRepository;

        public UpdateTimeline(ITimelineMessageRepository timelineMessageRepository)
        {
            _timelineMessageRepository = timelineMessageRepository;
        }

        public void Handle(IDomainEvent evt)
        {
            var messageQuacked = (MessageQuacked) evt;
            _timelineMessageRepository.Save(new TimelineMessageProjection(messageQuacked.Author, messageQuacked.Author,messageQuacked.Content,messageQuacked.Id));
        }
    }
}
