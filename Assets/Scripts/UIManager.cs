using System;
using System.Collections;
using System.Collections.Generic;
using DonutRun;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreTMP;
    [SerializeField] private TextMeshProUGUI _highScoreTMP;

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
        _scoreTMP.text = $"Счёт: {_player.Score}";
    }
}
