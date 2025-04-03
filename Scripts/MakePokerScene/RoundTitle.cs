using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoundTitle : MonoBehaviour
{
    private TextMeshProUGUI _textMeshProUGUI;

    private int RoundNumber => 5 - Player.Instance.TotalNumberCards.Count;

    private void Awake()
    {
        _textMeshProUGUI = GetComponent<TextMeshProUGUI>();

        UIManager.Instance.OnMakePoker += Handle_UpdateRoundInformation;
    }

    private void Handle_UpdateRoundInformation(object sender , IUIManagerEvents.MakePokerEventArgs makePokerEventArgs)
    {
        UpdateRoundTitle();
    }

    private void UpdateRoundTitle()
    {
        _textMeshProUGUI.text = $"Round{RoundNumber}";
    }
}
