using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UsedPokerCardPool : CardPoolBase
{
    private static UsedPokerCardPool _instance;
    public static UsedPokerCardPool Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindAnyObjectByType<UsedPokerCardPool>();

                if(_instance == null)
                {
                    _instance = ComponentCreator<UsedPokerCardPool>.Create();
                }
            }

            return _instance;
        }
    }

    [SerializeField] private Transform _usedPokerCardTransform;

    /// <summary>
    /// UsedPokerCard 要掛在這個 MakePokerListUITransform 下，這樣玩家才會看到
    /// </summary>
    /// <remarks>若是不掛在 Canvas 裡，這個物件是不會被玩家看見。</remarks>
    [SerializeField] private Transform _makePokerListUITransform;

    private Player _player;

    public event EventHandler<CardPoolInformationEventArgs> OnCardPoolEmpty;

    public class CardPoolInformationEventArgs : EventArgs
    {
        public static readonly new CardPoolInformationEventArgs Empty = new();

        public bool WantToState;
    }

    private void Awake()
    {
        Initialize();

        PokerCardManager.OnUsedPokerCardInstantiate += Handle_AddCardToCardTable;

        UsedPokerCardManager.OnUsedPokerCardRemoved += Handle_ReturnUsedPokerCard;

        MyGameManager.Instance.OnLoadDataCompleted += Handle_AddCardToCardTableAfterLoadData;

        _player = Player.Instance;
    }

    public void Handle_AddCardToCardTableAfterLoadData(object sender , MyGameManager.LoadDataInformationEventArgs loadDataInformationEventArgs)
    {
        AddCardToCardTable(_player.PokerCards);
    }

    protected override void Initialize()
    {
        SetPoolSizeTo(5);

        GenerateCard(_usedPokerCardTransform);
    }
    
    private void Handle_AddCardToCardTable(object sender , PokerCardManager.UsedPokerCardInformationEventArgs usedPokerCardInformationEventArgs)
    {
        AddCardToCardTable(usedPokerCardInformationEventArgs.PlayerPoker);
    }

    /// <summary>
    /// 在牌桌上生成玩家的撲克牌
    /// </summary>
    /// <param name="playerPoker">玩家的撲克牌</param>
    private void AddCardToCardTable(List<int> playerPoker)
    {
        // 把原本的撲克牌移回至 _usedPokerCardPool
        while(_makePokerListUITransform.childCount != 0)
        {
            Transform usedPokerCardTransform = _makePokerListUITransform.GetChild(0);
            
            Handle_ReturnUsedPokerCard(this , new UsedPokerCardManager.UsedPokerCardInformationEventArgs
            {
                UsedPokerCardObject = usedPokerCardTransform.gameObject
            });
        }

        // 生成撲克牌
        foreach (var item in playerPoker)
        {
            GetUsedPokerCard(item);
        }

        // 以上的寫法才不會造成生成錯誤
        if(CardPool.Count == 0)
        {
            // 顯示 ReadyButton
            OnCardPoolEmpty?.Invoke(this , new CardPoolInformationEventArgs
            {
                WantToState = true
            });
        }
    }

    /// <summary>
    /// 取得 UsedPokerCard
    /// </summary>
    /// <param name="usedPokerCardNumber"></param>
    /// <returns></returns>
    private GameObject GetUsedPokerCard(int usedPokerCardNumber)
    {
        if(CardPool.Count > 0)
        {
            GameObject usedPokerCard = CardPool.Dequeue();

            usedPokerCard.transform.SetParent(_makePokerListUITransform);

            // 數字卡會因為更換 Parent 而大小跑掉，因此我需要重新賦予
            usedPokerCard.transform.localScale = CardProperSize;

            #region 設計數字卡的樣式
            usedPokerCard.transform.Find(CARDTMP).GetComponent<TextMeshProUGUI>().text =
            usedPokerCardNumber switch
            {
                1 => "A" ,
                11 => "J" ,
                12 => "Q" ,
                13 => "K" ,
                _ => usedPokerCardNumber.ToString()
            };

            usedPokerCard.transform.GetChild(NAME_INDEX_IN_CARDTRANSFORM).name = usedPokerCardNumber.ToString();
            #endregion

            return usedPokerCard;
        }

        else
        {
            GameObject usedPokerCard = Instantiate(_usedPokerCardTransform).gameObject;

            return usedPokerCard;
        }
    }

    private void Handle_ReturnUsedPokerCard(object sender , UsedPokerCardManager.UsedPokerCardInformationEventArgs usedPokerCardInformationEventArgs)
    {
        ReturnUsedPokerCard(usedPokerCardInformationEventArgs.UsedPokerCardObject);
    }

    /// <summary>
    /// 取消 UsedPokerCard
    /// </summary>
    /// <param name="usedPokerCardGameObject"> UsedPokerCard 物件</param>
    private void ReturnUsedPokerCard(GameObject usedPokerCardGameObject)
    {
        usedPokerCardGameObject.transform.SetParent(transform);

        CardPool.Enqueue(usedPokerCardGameObject);
    }

    public void Handle_MoveBackToCardPoolFrom(object sender , IJudgePoker.PlayerInformationEventArgs playerInformationEventArgs)
    {
        MoveCardBackToPoolFrom(_makePokerListUITransform);
    }
}
