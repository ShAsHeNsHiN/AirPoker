using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class PlayerInformation_ResultScene : PlayerInformationBase
{
    protected override void Awake()
    {
        base.Awake();

        UIManager.Instance.OnResult += Handle_UpdatePlayerInformation;

        PokerGameJudge.Instance.OnWinnerGenerated += Handle_RemoveFirstTotalNumberCard;

        MyGameManager.Instance.OnLoadDataCompleted += Handle_GenerateTotalNumberCards;
    }

    private void Handle_UpdatePlayerInformation(object sender , IUIManagerEvents.ResultEventArgs resultEventArgs)
    {
        UpdatePlayerName();
        UpdatePlayerBlood();
        UpdateTotalNumberCardVisual();
    }

    public override void UpdateTotalNumberCardVisual()
    {
        _pokerTotalNumberListTransform.GetComponent<PokerTotalNumberListBase>().UpdateTotalNumberCardVisual(PlayerInstance);
    }

    private void Handle_RemoveFirstTotalNumberCard(object sender , PokerGameJudge.WinnerInformationEventArgs winnerInformationEventArgs)
    {
        RemoveFirstTotalNumberCard();
    }

    public void Handle_GenerateTotalNumberCards(object sender , MyGameManager.LoadDataInformationEventArgs loadDataInformationEventArgs)
    {
        GenerateTotalNumberCards();
    }
}
