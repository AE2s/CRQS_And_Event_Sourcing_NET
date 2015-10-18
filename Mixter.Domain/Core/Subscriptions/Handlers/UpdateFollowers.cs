namespace Mixter.Domain.Core.Subscriptions.Handlers
{
    [Handler]
    public class UpdateFollowers : 
        IEventHandler<UserFollowed>,
        IEventHandler<UserUnfollowed>
    {
    }
}
