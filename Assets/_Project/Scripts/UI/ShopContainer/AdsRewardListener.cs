using System;

namespace UI.Shop
{
    public class AdsRewardListener : IDisposable
    {
        private readonly CardSelector _cardSelector;

        public AdsRewardListener(CardSelector cardSelector)
        {
            _cardSelector = cardSelector ?? throw new ArgumentNullException(nameof(cardSelector));            
        }

        public void Dispose() =>
            Unsubscribe();

        public void ShowRewardedAd(string rewardId) =>
            YandexGameConnector.RewardedAdvShow(rewardId);

        public void Subscribe() =>
            YandexGameConnector.AdvRewarded += OnRewardAdv;

        public void Unsubscribe() =>
            YandexGameConnector.AdvRewarded -= OnRewardAdv;

        private void OnRewardAdv(string id) =>
            _cardSelector.TryUnlockWithReward(id, true);
    }
}