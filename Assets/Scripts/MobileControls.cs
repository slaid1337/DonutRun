using UnityEngine;

public class MobileControls : MonoBehaviour
{
    [SerializeField] private CheckWebGLPlatform _webGLPlatform;

    private void Start()
    {
        if (!_webGLPlatform.CheckIfMobile())
        {
            gameObject.SetActive(false);
        }
    }
}
