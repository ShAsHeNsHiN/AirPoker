using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class PlayerBase : MonoBehaviour , IData , ITotalNumberCard , IPlayer , IPokerCard , IJudgePoker
{
    #region 玩家相關
    [SerializeField] private EPlayerIdentify _playerIdentify;
    
    public EPlayerIdentify PlayerIdentify => _playerIdentify;

    public event EventHandler<IPlayer.InstantiateInformationEventArgs> OnPlayerInstantiate;

    public int OriginalBlood => 9;

    [field : SerializeField] public int Blood {get; set;}
    #endregion

    #region 數字卡相關
    public int TotalNumberCardAtStart => 4;

    public bool ChooseTotalNumberCardFinished => totalNumberCards.Count == TotalNumberCardAtStart;

    [SerializeField] private List<int> totalNumberCards = new();

    public List<int> TotalNumberCards => totalNumberCards;

    public event EventHandler<ITotalNumberCard.CheckingEventArgs> OnCheckEveryPlayerChooseTotalNumberCardFinished;

    public void AddTotalNumberCard(int target_Number)
    {
        if(!totalNumberCards.Contains(target_Number) && !ChooseTotalNumberCardFinished)
        {
            totalNumberCards.Add(target_Number);

            OnCheckEveryPlayerChooseTotalNumberCardFinished?.Invoke(this , ITotalNumberCard.CheckingEventArgs.Empty);
        }
    }

    public virtual void SetChosenTotalNumberCardToFront(int target_Number)
    {
        int target_TotalNumberCardIndex = TotalNumberCards.FindIndex(item => item == target_Number);

        // 不在首項才要交換
        if(target_TotalNumberCardIndex != 0)
        {
            TotalNumberCards[target_TotalNumberCardIndex] = TotalNumberCards[0];

            TotalNumberCards[0] = target_Number;
        }
    }

    protected void Handle_RemoveFirstTotalNumberCard(object sender , PokerGameJudge.WinnerInformationEventArgs winnerInformationEventArgs)
    {
        RemoveFirstTotalNumberCard();
    }

    public void RemoveFirstTotalNumberCard()
    {
        totalNumberCards.RemoveAt(0);
    }
    #endregion

    #region 撲克相關

    public int PokerCardAmountMax => 5;

    public bool ChoosePokerCardFinished => pokerCards.Count == PokerCardAmountMax;

    [SerializeField] private List<int> pokerCards = new();

    public List<int> PokerCards => pokerCards;

    public void AddPokerCard(int target_Number)
    {
        pokerCards.Add(target_Number);

        pokerCards.Sort();
    }

    public virtual void RemovePokerCard(int target_Number)
    {
        pokerCards.Remove(target_Number);
    }

    protected void Handle_ResetPokerCard(object sender , PokerGameJudge.WinnerInformationEventArgs winnerInformationEventArgs)
    {
        ResetPokerCard();
    }

    public void ResetPokerCard()
    {
        pokerCards.Clear();
    }

    #endregion

    #region 判斷撲克牌相關

    public bool IsValidPoker => pokerCards.Sum() == totalNumberCards[0];

    public event EventHandler<IJudgePoker.PlayerInformationEventArgs> OnPlayerReadyToAnalyzePoker;

    public virtual void ReadyToJudgePoker()
    {
        throw new NotImplementedException();
    }

    protected void PokerJudged(PlayerBase playerBase)
    {
        OnPlayerReadyToAnalyzePoker?.Invoke(this , new IJudgePoker.PlayerInformationEventArgs
        {
            PlayerBase = playerBase
        });
    }

    protected void Handle_WinnerMinusBlood(object sender , PokerGameJudge.WinnerInformationEventArgs resultPokersInformationEventArgs)
    {
        WinnerMinusBlood(resultPokersInformationEventArgs.WinnerInformation.Winner , resultPokersInformationEventArgs.WinnerInformation.MinusBlood);
    }

    public void WinnerMinusBlood(EPlayerIdentify winner , int minusBlood)
    {
        if(winner == _playerIdentify)
        {
            Blood -= minusBlood;
        }
    }

    protected void Handle_PokerCardOverUsed(object sender , IJudgePoker.PlayerInformationEventArgs playerInformationEventArgs)
    {
        PokerCardOverUsed();
    }

    public void PokerCardOverUsed()
    {
        // 從 PokerCardCountsMinusOneDictionary 去拿取扣牌的字典
        var pokerCardCountsMinusOneDictionary = PokerCardCountsMinusOneDictionary.Instance.PokerCardCountsMinusOneDict;

        // 將玩家的撲克牌以字典的形式整理好
        var playerPokerCardsToDictionary = PokerCards
            .GroupBy(item => item)
            .ToDictionary(group => group.Key , group => group.Count());

        // 拿 PokerCards 來對照，有用四張就扣血
        foreach (var item in pokerCardCountsMinusOneDictionary)
        {
            int needToMinusCount = 4;

            if(playerPokerCardsToDictionary.ContainsKey(item.Key) && playerPokerCardsToDictionary[item.Key] == needToMinusCount)
            {
                // 確認 playerPokerCardsToDictionary 有無 4 張，有的話就扣一滴血(若是有用超過，最多只會扣到 1 滴，因為組牌張數不可能這麼多)
                Blood--;
            }
        }
    }

    #endregion

    #region 資料相關
    public bool SavedData{get ; private set;}

    public string SaveDataString {get ; private set;}

    public void SaveData()
    {
        PlayerData playerData = new()
        {
            ePlayerIdentify = _playerIdentify ,
            Blood = Blood ,
            TotalNumberCards = new(totalNumberCards) ,
            PokerCards = new(PokerCards)
        };

        PersistentDataManager<PlayerData>.SaveData(playerData , SaveDataString);
    }

    public void GetData()
    {
        if(PlayerPrefs.HasKey(SaveDataString))
        {
            var playerData = PersistentDataManager<PlayerData>.GetDataNoChecked(SaveDataString);

            _playerIdentify = playerData.ePlayerIdentify;

            Blood = playerData.Blood;

            totalNumberCards = new(playerData.TotalNumberCards);

            pokerCards = new(playerData.PokerCards);

            SavedData = true;
        }

        else
        {
            Blood = OriginalBlood;

            SavedData = false;
        }
    }

    protected void SetSavedDataString(string dataString)
    {
        SaveDataString = dataString;
    }

    #endregion

    public void SubscribeEvents()
    {
        OnPlayerInstantiate += PlayerManager.Instance.Handle_AddPlayerToPlayerList;

        OnCheckEveryPlayerChooseTotalNumberCardFinished += PlayerManager.Instance.Handle_CheckingToMakePoker;
        
        OnPlayerReadyToAnalyzePoker += ResultPokerManager.Instance.Handle_AnalyzePoker;

        OnPlayerReadyToAnalyzePoker += Handle_PokerCardOverUsed;

        PokerGameJudge.Instance.OnWinnerGenerated += Handle_WinnerMinusBlood;

        PokerGameJudge.Instance.OnWinnerGenerated += Handle_RemoveFirstTotalNumberCard;

        PokerGameJudge.Instance.OnWinnerGenerated += Handle_ResetPokerCard;
    }

    protected virtual void Awake()
    {
        SubscribeEvents();
    }

    protected void PlayerInstantiate(PlayerBase playerBase)
    {
        OnPlayerInstantiate?.Invoke(playerBase , new IPlayer.InstantiateInformationEventArgs
        {
            PlayerBase = playerBase
        });
    }

    protected virtual void OnDestroy()
    {
        OnPlayerInstantiate = null;

        OnCheckEveryPlayerChooseTotalNumberCardFinished = null;

        OnPlayerReadyToAnalyzePoker = null;
    }
}