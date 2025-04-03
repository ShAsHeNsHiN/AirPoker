using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PokerTotalNumberListBase : MonoBehaviour
{
    [SerializeField] private Transform _totalNumberCardTransform;

    protected const string TOTALNUMBERTEXT = "TotalNumberText";

    protected const int NUMBERINDEX_IN_TOTALNUMBERCARD = 2;
    
    /// <summary>
    /// 移除首張數字卡
    /// </summary>
    /// <remarks>第一個數字都是玩家所使用的</remarks>
    public void RemoveFirstTotalNumberCard()
    {
        Destroy(transform.GetChild(0).gameObject);
    }

    /// <summary>
    /// 生成數字卡
    /// </summary>
    /// <param name="playerBase">玩家</param>
    public void GenerateTotalNumberCards(PlayerBase playerBase)
    {
        // 儲存資料且數字卡不為空
        // *這樣寫能應付儲存到「空白」數字卡資料
        if(playerBase.SavedData && playerBase.TotalNumberCards.Count != 0)
        {
            while (transform.childCount != playerBase.TotalNumberCards.Count)
            {
                Instantiate(_totalNumberCardTransform , transform);
            }
        }

        // 非儲存資料(2)
        else
        {
            while (transform.childCount != playerBase.TotalNumberCardAtStart)
            {
                Instantiate(_totalNumberCardTransform , transform);
            }
        }
    }

    /// <summary>
    /// 更新玩家的數字卡資訊
    /// </summary>
    /// <param name="playerBase">玩家</param>
    public void UpdateTotalNumberCardVisual(PlayerBase playerBase)
    {
        var totalNumberCards = playerBase.TotalNumberCards;

        for (int i = 0; i < totalNumberCards.Count; i++)
        {
            #region 設計數字卡樣式
            
            transform.GetChild(i).Find(TOTALNUMBERTEXT).GetComponent<TextMeshProUGUI>().text = totalNumberCards[i].ToString();

            transform.GetChild(i).GetChild(NUMBERINDEX_IN_TOTALNUMBERCARD).name = totalNumberCards[i].ToString();

            #endregion
        }
    }
}
