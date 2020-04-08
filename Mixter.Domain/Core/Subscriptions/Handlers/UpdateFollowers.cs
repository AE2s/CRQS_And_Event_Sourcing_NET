using Mixter.Domain.Core.Subscriptions.Events;
using Mixter.Domain.Identity;

namespace Mixter.Domain.Core.Subscriptions.Handlers
{
    [Handler]
    public class UpdateFollowers
    {
        private readonly IFollowersRepository _followersRepository;

        public UpdateFollowers(IFollowersRepository followersRepository)
        {
            _followersRepository = followersRepository;
        }

        public void Handle(UserFollowed userFollowed)
        {
            var followerProjection = new FollowerProjection(userFollowed.SubscriptionId.Followee, userFollowed.SubscriptionId.Follower);
            _followersRepository.Save(followerProjection);
        }

        public void Handle(UserUnfollowed userUnfollowed)
        {
            var followerProjection = new FollowerProjection(userUnfollowed.SubscriptionId.Followee, userUnfollowed.SubscriptionId.Follower);
            _followersRepository.Remove(followerProjection);
        }
    }
}
