using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PokerTotalNumberList_MakePokerScene : PokerTotalNumberListBase
{
    /// <summary>
    /// 玩家選擇的數字卡特效
    /// </summary>
    /// <param name="i">index</param>
    /// <param name="ePlayerIdentify">玩家身份</param>
    public void CertainTotalNumberCardMarked(int i , EPlayerIdentify ePlayerIdentify)
    {
        if(ePlayerIdentify == EPlayerIdentify.Gamer)
        {
            Transform totalNumberCardTransform = transform.GetChild(i);

            totalNumberCardTransform.GetComponent<TotalNumberCard>().TotalNumberCardChangesColor(true);
        }
    }

    /// <summary>
    /// 玩家選擇數字卡觸發
    /// </summary>
    /// <param name="totalNumberCardsInformationEventArgs"></param>
    public void TotalNumberCardsOrderChanged(List<int> totalNumberCards)
    {
        for (int i = 0; i < totalNumberCards.Count; i++)
        {
            transform.GetChild(i).Find(TOTALNUMBERTEXT).GetComponent<TextMeshProUGUI>().text = totalNumberCards[i].ToString();

            transform.GetChild(i).GetChild(NUMBERINDEX_IN_TOTALNUMBERCARD).name = totalNumberCards[i].ToString();
        }
    }
}
