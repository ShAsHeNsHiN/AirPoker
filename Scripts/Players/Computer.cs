using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Computer : PlayerBase
{
    private static Computer _instance;
    public static Computer Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindAnyObjectByType<Computer>();

                if(_instance == null)
                {
                    _instance = ComponentCreator<Computer>.Create();
                }
            }

            return _instance;
        }
    }

    public override void ReadyToJudgePoker()
    {
        if(IsValidPoker)
        {
            PokerJudged(Instance);
        }

        else
        {
            Debug.Log("撲克牌總和與選擇數字卡不符，請重新組牌!");
        }
    }

    #region 測試用函式
    public void AutoAddTotalNumberCards()
    {
        HashSet<int> totalNumberCardsHashSet = new();

        TotalNumberCards.Clear();

        while (totalNumberCardsHashSet.Count != TotalNumberCardAtStart)
        {
            totalNumberCardsHashSet.Add(Random.Range(15 , 51));
        }

        foreach (var item in totalNumberCardsHashSet)
        {
            AddTotalNumberCard(item);
        }
    }

    // 組牌只與數字卡總和相等(初階)
    // 組牌除初階以外，會考慮牌型(中階)
    // 組牌除初中階以外，還能針對對手(高階)
    // *這個自動組牌只有初階
    public void AutoMakePoker()
    {
        if(TotalNumberCards.Count == 0)
        {
            Debug.Log("數字卡清單不可為空！！");
            return;
        }

        SetChosenTotalNumberCardToFront(TotalNumberCards[Random.Range(0 , TotalNumberCards.Count)]);

        AddPokerCards();
    }

    private void AddPokerCards()
    {
        int maxExecution = default;

        while (true)
        {
            int target = TotalNumberCards[0];

            int pokerCardTotal = default;

            PokerCards.Clear();

            for (int i = 0; i < 5; i++)
            {
                int choosePokerNumber = Random.Range(1 , 14);

                AddPokerCard(choosePokerNumber);

                pokerCardTotal += choosePokerNumber;
            }

            // 在這個迴圈，沒設中斷點就會自動重來

            if(target == pokerCardTotal)
            {
                if(pokerCardTotal % 5 == 0)
                {
                    // 這樣才有可能為 5 張一模一樣的
                    var pokerCardsDictionary = PokerCards
                        .GroupBy(item => item)
                        .ToDictionary(group => group.Key , group => group.Count());

                    // 若是 PokerCards 沒有 5 張 一模一樣的就直接跳出(5 張一模一樣的撲克牌是不可能的事)
                    if(!pokerCardsDictionary.ContainsValue(5))
                    {
                        break;
                    }
                }

                else
                {
                    break;
                }
            }

            // 希望系統不要死機，設個執行次數上限
            if(maxExecution == 10000)
            {
                break;
            }

            maxExecution++;
        }
    }
    
    public void Handle_ComputerReady(object sender , System.EventArgs e)
    {
        // 給電腦執行一定的次數
        for (int i = 0; i < 9; i++)
        {
            AutoMakePoker();
        }

        ReadyToJudgePoker();
    }

    public void Handle_ComputerAddTotalNumberCards(object sender , System.EventArgs e)
    {
        AutoAddTotalNumberCards();
    }
    #endregion

    #region 資料相關
    public const string COMPUTER_DATA = "ComputerData";
    #endregion

    protected override void Awake()
    {
        base.Awake();

        SetSavedDataString(COMPUTER_DATA);
    }

    private void Start()
    {
        PlayerInstantiate(Instance);
    }
}
