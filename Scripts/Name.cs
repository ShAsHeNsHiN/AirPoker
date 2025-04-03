using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Name : MonoBehaviour
{
    private TextMeshProUGUI _textMeshProUGUI;

    private void Awake()
    {
        _textMeshProUGUI = GetComponent<TextMeshProUGUI>();
    }

    public void UpdatePlayerName(EPlayerIdentify ePlayerIdentify)
    {
        _textMeshProUGUI.text = ePlayerIdentify.ToString();
    }
}
