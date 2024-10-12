using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProccesingLoading : MonoBehaviour
{
    [SerializeField] private Volume _volume;
    [SerializeField] private CheckWebGLPlatform _checkWebGLPlatform;

    private void Start()
    {
        if (_checkWebGLPlatform.CheckIfMobile())
        {
            VolumeProfile profile = _volume.sharedProfile;

            profile.TryGet<Vignette>(out var vignette);
            vignette.active = false;

            profile.TryGet<DepthOfField>(out var depth);
            depth.active = false;
        }
        else
        {
            VolumeProfile profile = _volume.sharedProfile;

            profile.TryGet<Vignette>(out var vignette);
            vignette.active = true;

            profile.TryGet<DepthOfField>(out var depth);
            depth.active = true;
        }
    }
}
