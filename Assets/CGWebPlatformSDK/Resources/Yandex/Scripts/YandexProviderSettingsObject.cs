using UnityEngine;

namespace CGWebPlatform.Yandex
{
    [CreateAssetMenu(fileName = "YandexProviderSettingsObject", menuName = "Scriptable Objects/YandexProviderSettingsObject")]
    public class YandexProviderSettingsObject : ScriptableObject
    {
#if UNITY_EDITOR
        public CanvasAddEditor EditorCanvas;
#endif
    }
}