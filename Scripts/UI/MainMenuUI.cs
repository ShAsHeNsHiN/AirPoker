using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button _singlePlayerButton;

    private void Awake()
    {
        _singlePlayerButton.onClick.AddListener(() => 
        {
            Loader.Load(EScene.All_In_OneScene);
        });
    }
}
