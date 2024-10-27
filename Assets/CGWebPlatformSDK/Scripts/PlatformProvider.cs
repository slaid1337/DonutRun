using UnityEngine;
using UnityEngine.Events;

namespace CGWebPlatform
{
    public abstract class PlatformProvider : MonoBehaviour
    {
        public UnityEvent<string> OnGetAdReward;

        public IAdProvider AdProvider { get; protected set; }
        public IDataProvider DataProvider { get; protected set; }

        public abstract void Init();
    }
}