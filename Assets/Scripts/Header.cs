using DonutRun;
using TMPro;
using UnityEngine;

public class Header : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _moneyText;

    [SerializeField] private Donut _player;
    [SerializeField] private MoneyController _moneyController;

    private void Start()
    {
        _moneyController = MoneyController.Instance;
        OnMoneyChanged(_moneyController.GetMoney());
    }

    private void OnEnable()
    {
        _player.OnScoreChanged += OnScoreChanged;
        MoneyController.Instance.OnChangeMoney.AddListener(OnMoneyChanged);
    }

    private void OnDisable()
    {
        _player.OnScoreChanged -= OnScoreChanged;
        _moneyController.OnChangeMoney.RemoveListener(OnMoneyChanged);
    }

    private void OnScoreChanged()
    {
        _scoreText.text = $"{_player.Score}";
    }

    private void OnMoneyChanged(int money)
    {
        _moneyText.text = money.ToString();
    }
}