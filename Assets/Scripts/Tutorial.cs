using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private GameObject _pc;
    [SerializeField] private GameObject _mobile;
    [SerializeField] private CheckWebGLPlatform _checkWebGLPlatform;
    private float _time;

    private void Start()
    {
        bool isFirst = PlayerPrefs.GetInt("IsPlayedTutor", 0) == 0;

        if (isFirst)
        {
            if (_checkWebGLPlatform.CheckIfMobile())
            {
                _mobile.SetActive(true);
            }
            else
            {
                _pc.SetActive(true);
            }

            PlayerPrefs.SetInt("IsPlayedTutor", 1);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        _time += Time.fixedDeltaTime;

        if ((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) && _time >= 1f)
        {
            gameObject.SetActive(false);
        }
    }
}
