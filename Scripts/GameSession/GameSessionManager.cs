using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSessionManager : MonoBehaviour
{
    [SerializeField] private Transform _gameSessionPageTransform;

    private GameObject _gameSessionPageGameObject;

    private void Awake()
    {
        _gameSessionPageGameObject = _gameSessionPageTransform.gameObject;

        Hide();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            // 我這邊應該是反轉狀態才對
            Control();
        }
    }

    private void Hide()
    {
        _gameSessionPageGameObject.SetActive(false);
    }

    private void Control()
    {
        _gameSessionPageGameObject.SetActive(!_gameSessionPageGameObject.activeSelf);
    }
}
