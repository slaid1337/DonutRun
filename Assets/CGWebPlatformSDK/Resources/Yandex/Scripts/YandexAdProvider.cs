using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;


namespace CGWebPlatform.Yandex
{
    public class YandexAdProvider : MonoBehaviour, IAdProvider
    {
        public const int ReloadAdsSeconds = 60;
        public const string key = "AddsOff";
        public bool AdsEnabled { get; private set; }

        [DllImport("__Internal")]
        private static extern void ShowFullscreenAd();        

        [DllImport("__Internal")]
        private static extern int ShowRewardedAd(string placement);

        [DllImport("__Internal")]
        private static extern void Review();

        public event Action addsOnReloaded;
        

        public event Action onInterstitialShown;
        public event Action<string> onInterstitialFailed;
        
        public event Action<int> onRewardedAdOpened;
        
        public event Action<string> onRewardedAdReward;
        
        public event Action<int> onRewardedAdClosed;
        
        public event Action<string> onRewardedAdError;

        public event Action onClose;
        
        public Queue<int> rewardedAdPlacementsAsInt = new Queue<int>();
        public Queue<string> rewardedAdsPlacements = new Queue<string>();
        private Action<ReviewCallback> actionReview;
        public bool addsAvailable;
        private bool IsReviewed = false;
        
        public string Lang = "en";
        
        public bool IsFocused;



#if UNITY_EDITOR
        [SerializeField] YandexProviderSettingsObject _settings;

        public CanvasAddEditor editorCanvas;
#endif

        public void Init()
        {
            

            StartCoroutine(WaitAddReload());
#if UNITY_EDITOR
            editorCanvas = _settings.EditorCanvas;
            editorCanvas = Instantiate(editorCanvas);
            editorCanvas.Init(this);
#endif
            AdsEnabled = 0 == PlayerPrefs.GetInt(key, 0);

            
        }

        public void ShowReview(Action<ReviewCallback> action = null)
        {
            actionReview = action;
            if (IsReviewed)
            {
                OnReview(JsonUtility.ToJson(
                new ReviewCallback()
                {
                    CanReview = false,
                    FeedbackSent = false,
                    Reason = IsReviewed ? "GAME_RATED" : "Success"
                }));

                return;
            }
#if !UNITY_EDITOR && UNITY_WEBGL
            Review();
#else
            editorCanvas.ShowReview();
#endif
        }

        public void ShowInterstitial()
        {
            if (addsAvailable)
            {
                StartCoroutine(WaitAddReload());
#if !UNITY_EDITOR && UNITY_WEBGL
                ShowFullscreenAd();
#else
                editorCanvas.OpenFullScreen();
#endif

                AudioListener.pause = true;
                
            }
            else
            {
                Debug.LogWarning("Ad not ready!");
            }
        }

        public void ShowRewarded(string placement)
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            int placemantId = ShowRewardedAd(placement);
            
#else
            int placemantId = 0;
#endif
            rewardedAdPlacementsAsInt.Enqueue(placemantId);
            rewardedAdsPlacements.Enqueue(placement);
            
            AudioListener.pause = true;
#if UNITY_EDITOR
            editorCanvas.OpenReward(placemantId);

#endif
        }

        /// <summary>
        /// Callback from index.html
        /// </summary>
        public void OnInterstitialShown()
        {
            AudioListener.pause = false;
            
            onInterstitialShown?.Invoke();
        }

        /// <summary>
        /// Callback from index.html
        /// </summary>
        /// <param name="error"></param>
        public void OnInterstitialError(string error)
        {
            AudioListener.pause = false;
            onInterstitialFailed?.Invoke(error);
        }

        /// <summary>
        /// Callback from index.html
        /// </summary>
        /// <param name="placement"></param>
        public void OnRewardedOpen(int placement)
        {
            onRewardedAdOpened?.Invoke(placement);
        }

        /// <summary>
        /// Callback from index.html
        /// </summary>
        /// <param name="placement"></param>
        public void OnRewarded(int placement)
        {
            AudioListener.pause = false;
            
            if (placement == rewardedAdPlacementsAsInt.Dequeue())
            {
                onRewardedAdReward?.Invoke(rewardedAdsPlacements.Dequeue());
            }
        }

        /// <summary>
        /// Callback from index.html
        /// </summary>
        /// <param name="placement"></param>
        public void OnRewardedClose(int placement)
        {
            AudioListener.pause = false;
            
            onRewardedAdClosed?.Invoke(placement);
        }

        /// <summary>
        /// Callback from index.html
        /// </summary>
        /// <param name="placement"></param>
        public void OnRewardedError(string placement)
        {
            AudioListener.pause = false;
            onRewardedAdError?.Invoke(placement);
            rewardedAdsPlacements.Clear();
            rewardedAdPlacementsAsInt.Clear();
        }

        /// <summary>
        /// Browser tab has been closed
        /// </summary>
        /// <param name="error"></param>
        public void OnClose()
        {
            onClose?.Invoke();
        }

        public IEnumerator WaitAddReload()
        {
            addsAvailable = false;
            yield return new WaitForSecondsRealtime(ReloadAdsSeconds);
            addsAvailable = true;
            addsOnReloaded?.Invoke();
        }
        
        public void OnReview(string callback)
        {
            ReviewCallback review = JsonUtility.FromJson<ReviewCallback>(callback);
            if (review.FeedbackSent)
            {
                IsReviewed = true;
            }
            actionReview?.Invoke(review);
        }
        
    }
    [Serializable]
    public class GetDataCallback
    {
        public KeyValuePairStringCallback[] data;
        public KeyValuePairIntCallback[] score;
    }
    [Serializable]
    public class KeyValuePairStringCallback
    {
        public string key;
        public string value;
    }
    [Serializable]
    public class KeyValuePairIntCallback
    {
        public string key;
        public int value;
    }

    public struct ReviewCallback
    {
        public bool CanReview;
        public string Reason;
        public bool FeedbackSent;
    }

    public struct UserData
    {
        public string id;
        public string name;
        public string avatarUrlSmall;
        public string avatarUrlMedium;
        public string avatarUrlLarge;
    }
}