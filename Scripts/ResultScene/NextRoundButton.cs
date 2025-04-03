using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextRoundButton : MonoBehaviour
{
    private static NextRoundButton _instance;
    public static NextRoundButton Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindAnyObjectByType<NextRoundButton>();
            }

            return _instance;
        }
    }

    [SerializeField] private Button _nextRoundButton;

    public event Action OnNextRound;

    private void Awake()
    {
        if(_nextRoundButton == null)
        {
            _nextRoundButton = GetComponent<Button>();
        }

        _nextRoundButton.onClick.AddListener(() => 
        {
            NextRound();
        });
    }

    private void NextRound()
    {
        OnNextRound?.Invoke();

        TableElement.Instance.ChangeScene(ETableElementScene.MakePokerScene);

        UIManager.Instance.SceneChanged(ETableElementScene.MakePokerScene);
    }
}
