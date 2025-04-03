using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveDataButton : MonoBehaviour
{
    [SerializeField] private Button _saveDataButton;

    private void Awake()
    {
        if(_saveDataButton == null)
        {
            _saveDataButton = GetComponent<Button>();
        }

        _saveDataButton.onClick.AddListener(() => 
        {
            SaveData();
        });
    }

    private void SaveData()
    {
        MyGameManager.SaveData();
    }
}
