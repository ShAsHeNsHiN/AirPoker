using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour
{
    [SerializeField] private Button _exitButton;

    private void Awake()
    {
        if(_exitButton == null)
        {
            _exitButton = GetComponent<Button>();
        }

        _exitButton.onClick.AddListener(() => 
        {
            Exit();
        });
    }

    private void Exit()
    {
        Loader.Load(EScene.TitleScene);
    }
}
