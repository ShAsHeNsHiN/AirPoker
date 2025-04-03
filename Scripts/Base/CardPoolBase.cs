using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CardPoolBase : MonoBehaviour
{
    public int PoolSize {get; private set;}

    protected Queue<GameObject> CardPool = new();

    protected Vector3 CardProperSize => Vector3.one;

    protected const string CARDTMP = "CardTMP";
    protected const int NAME_INDEX_IN_CARDTRANSFORM = 2;

    /// <summary>
    /// 初始化要跑的程式
    /// </summary>
    protected virtual void Initialize()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 設置牌池大小
    /// </summary>
    /// <param name="poolSize"></param>
    protected void SetPoolSizeTo(int poolSize)
    {
        PoolSize = poolSize;
    }

    /// <summary>
    /// 將卡牌移回卡池
    /// </summary>
    /// <param name="originalTransform">要移回來的物件源頭</param>
    protected void MoveCardBackToPoolFrom(Transform originalTransform)
    {
        while (originalTransform.childCount != 0)
        {
            Transform cardTransform = originalTransform.GetChild(0);

            CardPool.Enqueue(cardTransform.gameObject);

            cardTransform.SetParent(transform);
        }
    }

    /// <summary>
    /// 生成卡牌
    /// </summary>
    /// <param name="generatedObjectTransform">要生成的物件</param>
    protected void GenerateCard(Transform generatedObjectTransform)
    {
        for (int i = 0; i < PoolSize; i++)
        {
            Transform cardTransform = Instantiate(generatedObjectTransform , transform);

            CardPool.Enqueue(cardTransform.gameObject);
        }
    }

    /// <summary>
    /// 更新數字
    /// </summary>
    /// <remarks>給 PokerCardPool 用</remarks>
    protected void UpdateNumber()
    {
        for (int i = 1; i <= PoolSize; i++)
        {
            int transformIndex = i - 1;

            #region 為撲克牌命名
            transform.GetChild(transformIndex).GetChild(NAME_INDEX_IN_CARDTRANSFORM).name = i.ToString();

            transform.GetChild(transformIndex).Find(CARDTMP).GetComponent<TextMeshProUGUI>().text = i.ToString() switch
            {
                "1" => "A",
                "11" => "J",
                "12" => "Q",
                "13" => "K",
                _ => i.ToString(),
            };
            #endregion
        }
    }
}