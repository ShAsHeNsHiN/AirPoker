using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AgainButton : MonoBehaviour
{
    [SerializeField] private Button _againButton;

    private void Awake()
    {
        if(_againButton == null)
        {
            _againButton = GetComponent<Button>();
        }

        _againButton.onClick.AddListener(() => 
        {
            AgainGame();
        });
    }

    private void AgainGame()
    {
        DataManager.IsSavedData = false;

        Loader.Load(EScene.All_In_OneScene);
    }
}
