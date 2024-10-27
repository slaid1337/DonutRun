using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace CGWebPlatform.Yandex
{
    public class YandexDataProvider : MonoBehaviour, IDataProvider
    {
        [DllImport("__Internal")]
        private static extern void InitPlayerData();
        [DllImport("__Internal")]
        private static extern void SetData(string key, string value);
        [DllImport("__Internal")]
        private static extern void SetScore(string key, int value);

        private YandexPrefsParams param;
        public UserData user;
        public event Action onUserDataReceived;

        public void Init()
        {
            param = new YandexPrefsParams();
            var operation = new InitAsyncOperation(param, this);
            InitData();
        }

        public event Action<GetDataCallback> onDataRecived;
        public event Action noAutorized;
        public void OnGetData(string json)
        {
            GetDataCallback callback;
            if (!string.IsNullOrEmpty(json))
            {
                callback = JsonUtility.FromJson<GetDataCallback>(json);
                if (callback.data == null)
                {
                    callback.data = new KeyValuePairStringCallback[0];
                }
                if (callback.score == null)
                {
                    callback.score = new KeyValuePairIntCallback[0];
                }
            }
            else
            {
                callback = new GetDataCallback();
                callback.data = new KeyValuePairStringCallback[0];
                callback.score = new KeyValuePairIntCallback[0];
            }
            onDataRecived?.Invoke(callback);
        }
        public void NoAutorized()
        {
            noAutorized?.Invoke();
        }

        public void SetPlayerData(string key, string value)
        {
#if !UNITY_EDITOR
            SetData(key, value);
#endif
        }

        public void SetPlayerScore(string key, int value)
        {
#if !UNITY_EDITOR
            SetScore(key, value);
#endif
        }

        public void InitData()
        {
#if !UNITY_EDITOR
            InitPlayerData();
#else
            NoAutorized();
#endif
        }

        public  void SetInt(string key, int value)
        {
            Debug.Log(key + " " + value);
            if (param.IsAutorised)
            {
                param.score[key] = value;
                SetPlayerScore(key, value);
            }
            else
            {
                PlayerPrefs.SetInt(key, value);
            }
        }
        public void SetString(string key, string value)
        {
            Debug.Log(key + " " + value);
            if (param.IsAutorised)
            {
                param.data[key] = value;
                SetPlayerData(key, value);
            }
            else
            {
                PlayerPrefs.SetString(key, value);
            }
        }
        public int GetInt(string key)
        {
            if (param.IsAutorised)
            {
                if (param.score.TryGetValue(key, out var value))
                    return value;
                else
                    return 0;
            }
            else
            {
                return PlayerPrefs.GetInt(key, 0);
            }
        }
        public string GetString(string key)
        {
            if (param.IsAutorised)
            {
                if (param.data.TryGetValue(key, out var value))
                    return value;
                else
                    return "";
            }
            else
            {
                return PlayerPrefs.GetString(key, "");
            }
        }
        public int GetInt(string key, int df)
        {
            if (param.IsAutorised)
            {
                if (param.score.TryGetValue(key, out var value))
                    return value;
                else
                    return df;
            }
            else
            {
                return PlayerPrefs.GetInt(key, df);
            }
        }

        public string GetString(string key, string df)
        {
            if (param.IsAutorised)
            {
                if (param.data.TryGetValue(key, out var value))
                    return value;
                else
                    return df;
            }
            else
            {
                return PlayerPrefs.GetString(key, df);
            }
        }

        public void StoreUserData(string data)
        {
            user = JsonUtility.FromJson<UserData>(data);
            onUserDataReceived?.Invoke();
        }
    }
    public class InitAsyncOperation : CustomYieldInstruction
    {
        private YandexDataProvider _dataProvider;

        public InitAsyncOperation(YandexPrefsParams param, YandexDataProvider dataProvider)
        {
            _dataProvider = dataProvider;

            _dataProvider.noAutorized += Instance_noAutorized;
            _dataProvider.onDataRecived += Instance_onDataRecived;
            this.param = param;
        }

        private void Instance_onDataRecived(GetDataCallback obj)
        {
            param.Fill(obj);
            Callback(true);
        }

        private void Instance_noAutorized()
        {
            Callback(false);
        }

        private YandexPrefsParams param;
        public bool IsSuccess;
        public override bool keepWaiting => _keepWaiting;
        private bool _keepWaiting = true;
        private void Callback(bool success)
        {
            param.IsInit = true;
            _keepWaiting = false;
            param.IsAutorised = success;
            IsSuccess = success;
        }
    }

    public class YandexPrefsParams
    {
        public bool IsAutorised { get; set; } = false;
        public bool IsInit { get; set; } = false;
        public Dictionary<string, string> data;
        public Dictionary<string, int> score;
        public YandexPrefsParams()
        {
            data = new Dictionary<string, string>();
            score = new Dictionary<string, int>();
        }
        public void Fill(GetDataCallback data)
        {
            foreach (var item in data.data)
            {
                this.data.Add(item.key, item.value);
            }
            foreach (var item in data.score)
            {
                this.score.Add(item.key, item.value);
            }
        }
    }
}