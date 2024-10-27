using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text _moneyText;
    [SerializeField] private TMP_Text _bestScoreText;
    [SerializeField] private RectTransform _playBtn;
    [SerializeField] private RectTransform _playIcon;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        _moneyText.text = MoneyController.Instance.GetMoney().ToString();
        _bestScoreText.text = SaveController.Instance.GetBestScore().ToString();

        MoneyController.Instance.OnChangeMoney.AddListener(OnUpdateMoney);
    }

    private void OnUpdateMoney(int money)
    {
        _moneyText.text = money.ToString();
    }

    public void Play()
    {
        _playBtn.GetComponent<Button>().interactable = false;

        Sequence sequence = DOTween.Sequence();

        sequence.Append(_playBtn.DOScale(5.7f, 1.5f));
        sequence.Join(_playBtn.DOLocalRotate(new Vector3(0, 0, -360f), 1.5f, RotateMode.FastBeyond360));
        sequence.onComplete += delegate
        {
            SceneManager.LoadScene("SampleScene");
        };

        _playIcon.DOScale(0f, 0.6f);
    }
}