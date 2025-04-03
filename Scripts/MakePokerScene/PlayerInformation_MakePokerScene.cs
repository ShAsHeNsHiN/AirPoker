using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PlayerInformation_MakePokerScene : PlayerInformationBase
{
    // 因為只會執行 1 次，因此這樣寫或在 Awake() 賦值意思一樣
    private IForPlayerExceptComputer ForPlayerExceptComputer => Player.Instance;

    protected override void Awake()
    {
        base.Awake();

        UIManager.Instance.OnMakePoker += Handle_UpdatePlayerInformation;

        if(_ePlayerIdentify == EPlayerIdentify.Gamer)
        {
            ForPlayerExceptComputer.OnTotalNumberCardsOrderChanged += Handle_TotalNumberCardsOrderChanged;
        }

        PokerGameJudge.Instance.OnWinnerGenerated += Handle_RemoveFirstTotalNumberCard;

        MyGameManager.Instance.OnLoadDataCompleted += Handle_GenerateTotalNumberCards;
    }

    private void Handle_UpdatePlayerInformation(object sender , IUIManagerEvents.MakePokerEventArgs makePokerEventArgs)
    {
        UpdatePlayerName();
        UpdatePlayerBlood();
        UpdateTotalNumberCardVisual();
    }

    public override void UpdateTotalNumberCardVisual()
    {
        _pokerTotalNumberListTransform.GetComponent<PokerTotalNumberListBase>().UpdateTotalNumberCardVisual(PlayerInstance);

        CertainTotalNumberCardMarked(target_TotalNumberCardIndex : 0 , _ePlayerIdentify);
    }

    private void Handle_RemoveFirstTotalNumberCard(object sender , PokerGameJudge.WinnerInformationEventArgs winnerInformationEventArgs)
    {
        RemoveFirstTotalNumberCard();
    }

    private void Handle_GenerateTotalNumberCards(object sender , MyGameManager.LoadDataInformationEventArgs loadDataInformationEventArgs)
    {
        GenerateTotalNumberCards();
    }

    private void Handle_TotalNumberCardsOrderChanged(object sender , IForPlayerExceptComputer.TotalNumberCardsInformationEventArgs totalNumberCardsInformationEventArgs)
    {
        TotalNumberCardsOrderChanged(totalNumberCardsInformationEventArgs.TotalNumberCards);
    }
}
