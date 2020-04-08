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

        public void Handle(MessageQuacked evt)
        {
            _timelineMessageRepository.Save(new TimelineMessageProjection(evt.Author, evt.Author, evt.Content, evt.Id));
        }

        public void Handle(MessageDeleted evt)
        {
            _timelineMessageRepository.Delete(evt.MessageId);
        }
    }
}
