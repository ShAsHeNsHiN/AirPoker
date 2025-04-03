using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerInformationBase : MonoBehaviour, IPlayerInformation
{
    [SerializeField] protected EPlayerIdentify _ePlayerIdentify;

    [SerializeField] protected Transform _nameTransform;

    [SerializeField] protected Transform _playerStateTransform;

    [SerializeField] protected Transform _pokerTotalNumberListTransform;

    protected PlayerBase PlayerInstance;

    protected virtual void Awake()
    {
        PlayerInstance = _ePlayerIdentify switch
        {
            EPlayerIdentify.Gamer => Player.Instance ,
            EPlayerIdentify.Computer => Computer.Instance ,
            _ => throw new NotImplementedException()
        };
    }

    public void RemoveFirstTotalNumberCard()
    {
        _pokerTotalNumberListTransform.GetComponent<PokerTotalNumberListBase>().RemoveFirstTotalNumberCard();
    }

    public void GenerateTotalNumberCards()
    {
        _pokerTotalNumberListTransform.GetComponent<PokerTotalNumberListBase>().GenerateTotalNumberCards(PlayerInstance);
    }

    public void UpdatePlayerName()
    {
        _nameTransform.GetComponent<Name>().UpdatePlayerName(_ePlayerIdentify);
    }

    public virtual void UpdateTotalNumberCardVisual()
    {
        throw new Exception();
    }

    public void UpdatePlayerBlood()
    {
        _playerStateTransform.GetComponent<PlayerState>().UpdatePlayerBlood(PlayerInstance.Blood);
    }

    protected void CertainTotalNumberCardMarked(int target_TotalNumberCardIndex , EPlayerIdentify ePlayerIdentify)
    {
        _pokerTotalNumberListTransform.GetComponent<PokerTotalNumberList_MakePokerScene>().CertainTotalNumberCardMarked(target_TotalNumberCardIndex , ePlayerIdentify);
    }

    protected void TotalNumberCardsOrderChanged(List<int> totalNumberCards)
    {
        _pokerTotalNumberListTransform.GetComponent<PokerTotalNumberList_MakePokerScene>().TotalNumberCardsOrderChanged(totalNumberCards);
    }
}
