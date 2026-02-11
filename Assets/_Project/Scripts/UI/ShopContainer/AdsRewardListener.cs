using System;
using YG;

namespace UI.Shop
{
    public class AdsRewardListener : IDisposable
    {
        private readonly CardSelector _cardSelector;

        public AdsRewardListener(CardSelector cardSelector)
        {
            _cardSelector = cardSelector ?? throw new ArgumentNullException(nameof(cardSelector));

            Subscribe();
        }

        public void Dispose() =>
            Unsubscribe();

        public void ShowRewardedAd(string rewardId) =>
            YG2.RewardedAdvShow(rewardId);

        private void Subscribe() => 
            YG2.onRewardAdv += OnRewardAdv;

        private void Unsubscribe() => 
            YG2.onRewardAdv -= OnRewardAdv;

        private void OnRewardAdv(string id) =>
            _cardSelector.TryUnlockWithReward(id);
    }
}