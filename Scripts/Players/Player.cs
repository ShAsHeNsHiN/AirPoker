using System;
using UnityEngine;

public class Player : PlayerBase , IForPlayerExceptComputer
{
    private static Player _instance;
    public static Player Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindAnyObjectByType<Player>();

                if(_instance == null)
                {
                    _instance = ComponentCreator<Player>.Create();
                }
            }

            return _instance;
        }
    }

    #region 數字卡相關

    private void Handle_AddTotalNumberCard(object sender , TotalNumberCardManager.TotalNumberCardInformation totalNumberCardInformation)
    {
        AddTotalNumberCard(totalNumberCardInformation.TotalNumberCardNumber);

        #region 測試用
        if(ChooseTotalNumberCardFinished)
        {
            OnComputerAddTotalNumberCards?.Invoke(this , EventArgs.Empty);
        }
        #endregion
    }

    private void Handle_SetTotalNumberCardToFront(object sender , TotalNumberCardManager.TotalNumberCardInformation totalNumberCardInformation)
    {
        SetChosenTotalNumberCardToFront(totalNumberCardInformation.TotalNumberCardNumber);
    }

    public override void SetChosenTotalNumberCardToFront(int target_Number)
    {
        int target_TotalNumberCardIndex = TotalNumberCards.FindIndex(item => item == target_Number);

        // 不在首項才要交換
        if(target_TotalNumberCardIndex != 0)
        {
            TotalNumberCards[target_TotalNumberCardIndex] = TotalNumberCards[0];

            TotalNumberCards[0] = target_Number;

            OnTotalNumberCardsOrderChanged?.Invoke(this , new IForPlayerExceptComputer.TotalNumberCardsInformationEventArgs
            {
                TotalNumberCards = TotalNumberCards
            });
        }
    }
    #endregion

    #region 撲克牌相關
    private void Handle_AddPokerCard(object sender , PokerCardManager.PokerCardInformationEventArgs pokerCardInformationEventArgs)
    {
        AddPokerCard(pokerCardInformationEventArgs.PokerCardNumber);
    }

    private void Handle_RemovePokerCard(object sender , UsedPokerCardManager.UsedPokerCardInformationEventArgs usedPokerCardInformationEventArgs)
    {
        base.RemovePokerCard(usedPokerCardInformationEventArgs.UsedPokerCardNumber);

        // 若是 ReadyButton 已經被關閉就不必再關閉了
        if(ReadyButton.Instance.gameObject.activeSelf)
        {
            OnSetReadyButtonActiveTo?.Invoke(this , new IForPlayerExceptComputer.ReadyButtonInformationEventArgs
            {
                WantToState = false
            });
        }
    }
    #endregion

    #region 給玩家的
    public event EventHandler<IForPlayerExceptComputer.ReadyButtonInformationEventArgs> OnSetReadyButtonActiveTo;
    public event EventHandler<IForPlayerExceptComputer.TotalNumberCardsInformationEventArgs> OnTotalNumberCardsOrderChanged;

    public void SubscribeEventsForPlayer()
    {
        #region 數字卡相關事件
        TotalNumberCardManager.OnPlayerGetNumberInChoosePokerTotalNumberScene += Handle_AddTotalNumberCard;

        TotalNumberCardManager.OnPlayerGetNumberInMakePokerScene += Handle_SetTotalNumberCardToFront;
        #endregion

        PokerCardManager.OnPlayerGetNumber += Handle_AddPokerCard;

        UsedPokerCardManager.OnPlayerGetNumber += Handle_RemovePokerCard;

        // 因為未來可能會有多個玩家，所以我不能依賴 Player.Instance，但 ReadyButton 只會有一個，所以這樣 Subscribe 比較合理
        OnSetReadyButtonActiveTo += ReadyButton.Instance.Handle_SetReadyButtonActive_IForPlayerExceptComputer;

        ReadyButton.Instance.OnPlayerClickReadyButton += Handle_ReadyToJudgePoker;

        OnPlayerReadyToAnalyzePoker += UsedPokerCardPool.Instance.Handle_MoveBackToCardPoolFrom;
    }
    #endregion

    #region 資料相關
    public const string PLAYER_DATA = "PlayerData";
    #endregion

    #region 判斷撲克牌相關
    private void Handle_ReadyToJudgePoker(object sender , ReadyButton.PlayerInformationEventArgs playerInformationEventArgs)
    {
        ReadyToJudgePoker();
    }

    public override void ReadyToJudgePoker()
    {
        if(IsValidPoker)
        {
            PokerJudged(Instance);

            OnSetReadyButtonActiveTo?.Invoke(this , new IForPlayerExceptComputer.ReadyButtonInformationEventArgs
            {
                WantToState = false
            });

            OnComputerReady?.Invoke(this , EventArgs.Empty);
        }

        else
        {
            Debug.Log("撲克牌總和與選擇數字卡不符，請重新組牌!");
        }
    }
    #endregion

    #region 測試用
    public event EventHandler OnComputerReady;

    public event EventHandler OnComputerAddTotalNumberCards;
    #endregion

    protected override void Awake()
    {
        base.Awake();

        SetSavedDataString(PLAYER_DATA);

        SubscribeEventsForPlayer();

        OnComputerReady += Computer.Instance.Handle_ComputerReady;

        OnComputerAddTotalNumberCards += Computer.Instance.Handle_ComputerAddTotalNumberCards;
    }

    private void Start()
    {
        PlayerInstantiate(Instance);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        OnSetReadyButtonActiveTo = null;

        OnTotalNumberCardsOrderChanged = null;

        OnComputerReady = null;

        OnComputerAddTotalNumberCards = null;
    }
}
