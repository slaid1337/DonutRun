using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;

namespace CGWebPlatform.Yandex
{

    public class YandexPlatformProvider : PlatformProvider
    {
#region DllImports
        [DllImport("__Internal")]
        private static extern void AuthenticateUser();
        [DllImport("__Internal")]
        private static extern string GetLang();
        [DllImport("__Internal")]
        private static extern void SetScoreToTable(int score);
        [DllImport("__Internal")]
        public static extern void ReadyTab();
        [DllImport("__Internal")]
        public static extern void StopTab();
        [DllImport("__Internal")]
        public static extern void StartTab();
        [DllImport("__Internal")]
        private static extern void GetUserData();
        #endregion

        public event Action onInitializeData;
        public bool IsFirstOpen = true;
        private YandexAdProvider _adProvider;
        private YandexDataProvider _dataProvider;

        public override void Init()
        {
            _adProvider = gameObject.AddComponent<YandexAdProvider>();
            AdProvider = _adProvider;
            AdProvider.Init();

            _adProvider.onRewardedAdReward += OnReward;

            InitDataPrefs();
        }

        public void OnReward(string reward)
        {
            OnGetAdReward?.Invoke(reward);
        }

        public void InitDataPrefs()
        {
            _dataProvider = gameObject.AddComponent<YandexDataProvider>();
            _dataProvider.Init();

            IsFirstOpen = false;
            onInitializeData?.Invoke();
        }
        public static void Ready()
        {
            ReadyTab();
        }

        /// <summary>
        /// Call this to ask user to authenticate
        /// </summary>
        public void Authenticate()
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            AuthenticateUser();
#endif
        }

        public void SetScore(int score)
        {
            try
            {
                SetScoreToTable(score);
            }
            catch
            {
                Debug.Log("can't set score to table");
            }
        }

        /// <summary>
        /// Call this to receive user data
        /// </summary>
        public void RequestUserData()
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            GetUserData();
#endif
        }
    }
}