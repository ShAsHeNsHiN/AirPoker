using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class PokerCardManager
{
    public static event EventHandler<UsedPokerCardInformationEventArgs> OnUsedPokerCardInstantiate;

    public class UsedPokerCardInformationEventArgs : EventArgs
    {
        public static readonly new UsedPokerCardInformationEventArgs Empty = new();

        public List<int> PlayerPoker;
    }

    public static void UsedPokerCardInstantiate(List<int> playerPoker)
    {
        OnUsedPokerCardInstantiate?.Invoke(null , new UsedPokerCardInformationEventArgs
        {
            PlayerPoker = playerPoker
        });
    }

    public static event EventHandler<PokerCardInformationEventArgs> OnPlayerGetNumber;

    public class PokerCardInformationEventArgs : EventArgs
    {
        public static readonly new PokerCardInformationEventArgs Empty = new();

        public int PokerCardNumber;
    }

    public static void PlayerGetNumber(int pokerCardNumber)
    {
        OnPlayerGetNumber?.Invoke(null , new PokerCardInformationEventArgs
        {
            PokerCardNumber = pokerCardNumber
        });
    }

    // 閒置，無聊寫寫
    public static void PokerCard(Func<PokerCardDictionary , bool> func , PlayerBase playerBase , Action<PokerCardDictionary> action , int pokerCardNumber , PokerCardDictionary pokerCardDictionary)
    {
        if(func(pokerCardDictionary) && !playerBase.ChoosePokerCardFinished)
        {
            action(pokerCardDictionary);

            PlayerGetNumber(pokerCardNumber);

            UsedPokerCardInstantiate(playerBase.PokerCards);
        }
    }

    public static void ResetStaticData()
    {
        OnUsedPokerCardInstantiate = null;
        OnPlayerGetNumber = null;
    }
}
