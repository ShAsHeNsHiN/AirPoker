using System;
using UnityEngine;

public class TableElement : MonoBehaviour , IData
{
    private static TableElement _instance;
    public static TableElement Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindAnyObjectByType<TableElement>();
            }

            return _instance;
        }
    }

    /// <summary>
    /// 當前場景
    /// </summary>
    [field : SerializeField] public ETableElementScene CurrentScene {get ; set;}

    /// <summary>
    /// 布簾(用來遮擋非當前場景的)
    /// </summary>
    [SerializeField] private Transform _playCurtainTransform;

    private const string TABLEELEMNET_DATA = "TableElementData";

    /// <summary>
    /// 顯示特定的場景
    /// </summary>
    /// <param name="targetTableElementScene">想要顯示的場景</param>
    private void ShowCertainScene(ETableElementScene targetTableElementScene)
    {
        // 將布幕移至最後一個
        _playCurtainTransform.SetAsLastSibling();

        // 將顯示場景移至最後一個
        transform.Find(targetTableElementScene.ToString()).SetAsLastSibling();

        // 這樣可以確保布幕一定會在顯示場景上面
    }

    public void ChangeScene(ETableElementScene eTableElementScene)
    {
        CurrentScene = eTableElementScene;

        ShowCertainScene(CurrentScene);
    }

    public void SaveData()
    {
        TableElementData tableElementData = new()
        {
            eTableElementScene = CurrentScene
        };

        PersistentDataManager<TableElementData>.SaveData(tableElementData , TABLEELEMNET_DATA);
    }

    public void GetData()
    {
        var tableElementData = PersistentDataManager<TableElementData>.GetDataChecked(TABLEELEMNET_DATA);

        CurrentScene = tableElementData.eTableElementScene;
    }

    #region 暫時用不到的 Function
    /// <summary>
    /// 對所有場景做開或關
    /// </summary>
    /// <param name="currentState">想要的狀態(開啟或關閉)</param>
    private void SetAllSceneActiveTo(bool currentState)
    {
        foreach (var item in Enum.GetValues(typeof(ETableElementScene)))
        {
            transform.Find(item.ToString()).gameObject.SetActive(currentState);
        }
    }
    #endregion
}