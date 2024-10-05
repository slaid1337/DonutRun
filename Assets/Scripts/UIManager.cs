using DonutRun;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreTMP;

    [SerializeField] private Donut _player;

    private void OnEnable() 
    {
        _player.OnScoreChanged += OnScoreChanged;
    }

    private void OnDisable() 
    {
        _player.OnScoreChanged += OnScoreChanged;
    }

    private void OnScoreChanged()
    {
        _scoreTMP.text = $"{_player.Score}";
    }
}
