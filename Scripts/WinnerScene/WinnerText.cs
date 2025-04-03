using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinnerText : MonoBehaviour
{
    private TextMeshProUGUI _textMeshProUGUI;

    private void Awake()
    {
        _textMeshProUGUI = GetComponent<TextMeshProUGUI>();

        UIManager.Instance.OnGameEnd += Handle_UpdateWinnerText;
    }

    private void Handle_UpdateWinnerText(object sender , IUIManagerEvents.GameEndEventArgs gameEndEventArgs)
    {
        _textMeshProUGUI.text = 
        $"Winner : {gameEndEventArgs.WinnerEPlayerIdentify}" ?? "Tieeeeeeeeeee";
    }
}
