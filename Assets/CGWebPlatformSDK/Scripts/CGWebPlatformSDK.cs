using UnityEngine;
using CGWebPlatform.Yandex;
using UnityEngine.Events;

namespace CGWebPlatform
{
    public class CGWebPlatformSDK : Singletone<CGWebPlatformSDK>
    {
        public PlatformProvider PlatformProvider
        {
            get
            {
                return _provider;
            }
        }

        private PlatformProvider _provider;

        public UnityEvent<string> OnGetAdReward;

        public override void Awake()
        {
            base.Awake();

            if (_isDestroyed) return;

            _provider = gameObject.AddComponent<YandexPlatformProvider>();
            _provider.Init();
        }

        public void OnGetRewardCallback(string reward)
        {
            _provider.OnGetAdReward.RemoveListener(OnGetRewardCallback);
            OnGetAdReward?.Invoke(reward);
        }

        public string GetLanguage()
        {
            return "";
        }

        [ContextMenu("ShowInterstitial")]
        public void ShowInterstitial()
        {
            _provider.AdProvider.ShowInterstitial();
        }

        public void ShowRewarded(string rewardName)
        {
            _provider.OnGetAdReward.AddListener(OnGetRewardCallback);
            _provider.AdProvider.ShowRewarded(rewardName);
        }


    }
}