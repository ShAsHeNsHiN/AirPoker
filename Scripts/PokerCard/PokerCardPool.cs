using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PokerCardPool : CardPoolBase
{
    private static PokerCardPool _instance;
    public static PokerCardPool Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindAnyObjectByType<PokerCardPool>();
            }

            return _instance;
        }
    }

    [SerializeField] private Transform _pokerCardTransform;

    [SerializeField] private Transform _pokerListUITransform;

    private void Awake()
    {
        UIManager.Instance.OnMakePoker += Handle_UpdatePokerListUI;

        UIManager.Instance.OnResult += Handle_MoveBackToCardPool;

        Initialize();
    }

    protected override void Initialize()
    {
        SetPoolSizeTo(13);

        // *這個函式一定只能放在 Awake()，這樣儲存資料後重啟遊戲時才不會出錯。若是想要更改順序，就得連同 MyGameManager 中的 Start() 中的函式一併修改順序
        GenerateCard(_pokerCardTransform);

        UpdateNumber();
    }

    private void Handle_MoveBackToCardPool(object sender , IUIManagerEvents.ResultEventArgs resultEventArgs)
    {
        MoveCardBackToPoolFrom(_pokerListUITransform);
    }

    private void Handle_UpdatePokerListUI(object sender , IUIManagerEvents.MakePokerEventArgs makePokerEventArgs)
    {
        PokerCardAmountVisual();

        AddCardToHands();
    }

    /// <summary>
    /// 在手(畫面)上生成可用撲克牌
    /// </summary>
    private void AddCardToHands()
    {
        while(CardPool.Count != 0)
        {
            var pokerCardObject = CardPool.Dequeue();

            pokerCardObject.transform.SetParent(_pokerListUITransform);

            pokerCardObject.transform.localScale = CardProperSize;
        }
    }

    private void PokerCardAmountVisual()
    {
        InitialPokerCardAmountVisual();

        ChooseThreeNumberToCountsMinusOne(PokerCardCountsMinusOneDictionary.Instance);
    }

    /// <summary>
    /// 減少 3 個數字 1 張牌(特殊規則)
    /// </summary>
    /// <remarks>避免大數字碾壓小數字，若是不想加可以把它註解掉</remarks>
    public void ChooseThreeNumberToCountsMinusOne(PokerCardCountsMinusOneDictionary pokerCardCountsMinusOneDictionary)
    {
        foreach (var minusPokerCard in pokerCardCountsMinusOneDictionary.PokerCardCountsMinusOneDict)
        {
            int childIndex = minusPokerCard.Key - 1;

            transform.GetChild(childIndex).GetComponent<PokerCard>().PokerEffectForSpecialRule(PokerCardDictionary.Instance);
        }
    }

    private void InitialPokerCardAmountVisual()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<PokerCard>().InitialAmountVisual(PokerCardDictionary.Instance);
        }
    }

}
