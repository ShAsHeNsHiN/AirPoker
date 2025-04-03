using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MyGameManager : MonoBehaviour
{
    private static MyGameManager _instance;
    public static MyGameManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindAnyObjectByType<MyGameManager>();

                if(_instance == null)
                {
                    _instance = ComponentCreator<MyGameManager>.Create();
                }
            }

            return _instance;
        }
    }

    public static bool IsSavedData;

    public event EventHandler<LoadDataInformationEventArgs> OnLoadDataCompleted;

    public class LoadDataInformationEventArgs : EventArgs
    {
        public static readonly new LoadDataInformationEventArgs Empty = new();
    }

    // 現在這樣的順序是可以正常執行的狀態，若是想調整請重新打開 OrderExecution 程式
    private void Start()
    {
        if(DataManager.IsSavedData)
        {
            ContinueGame();
        }

        else
        {
            AgainGame();
        }
    }

    public void AgainGame()
    {
        ResetData();

        LoadData();

        OnLoadDataCompleted?.Invoke(this , LoadDataInformationEventArgs.Empty);

        TableElement.Instance.ChangeScene(TableElement.Instance.CurrentScene);

        UIManager.Instance.SceneChanged(TableElement.Instance.CurrentScene);
    }

    public void ContinueGame()
    {
        LoadData();

        OnLoadDataCompleted?.Invoke(this , LoadDataInformationEventArgs.Empty);

        TableElement.Instance.ChangeScene(TableElement.Instance.CurrentScene);

        UIManager.Instance.SceneChanged(TableElement.Instance.CurrentScene);
    }

    public static void LoadData()
    {
        // 資料
        PokerGameJudge.Instance.GetData();
        ResultPokerManager.Instance.GetData();
        PokerCardCountsMinusOneDictionary.Instance.GetData();
        PokerCardDictionary.Instance.GetData();
        Player.Instance.GetData();
        Computer.Instance.GetData();
        TableElement.Instance.GetData();
    }

    public static void SaveData()
    {
        TableElement.Instance.SaveData();
        PokerGameJudge.Instance.SaveData();
        ResultPokerManager.Instance.SaveData();
        PokerCardCountsMinusOneDictionary.Instance.SaveData();
        PokerCardDictionary.Instance.SaveData();
        Player.Instance.SaveData();
        Computer.Instance.SaveData();

        DataManager.IsSavedData = true;
    }

    public static void ResetData()
    {
        PlayerPrefs.DeleteAll();
    }
}
