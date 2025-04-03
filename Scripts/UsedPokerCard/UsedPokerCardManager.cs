using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class UsedPokerCardManager
{
    public static event EventHandler<UsedPokerCardInformationEventArgs> OnPlayerGetNumber;

	public static event EventHandler<UsedPokerCardInformationEventArgs> OnUsedPokerCardRemoved;

	public static event
	EventHandler<UsedPokerCardInformationEventArgs>
	OnRestorePokerCardAmountVisual;

	public class UsedPokerCardInformationEventArgs : EventArgs
	{
		public static readonly new UsedPokerCardInformationEventArgs Empty = new();

		public int UsedPokerCardNumber;
		public int PokerCardNumberIndex;
		public GameObject UsedPokerCardObject;
	}
    
    public static void PlayerGetNumber(int usedPokerCardNumber)
    {
        OnPlayerGetNumber?.Invoke(null , new UsedPokerCardInformationEventArgs
        {
            UsedPokerCardNumber = usedPokerCardNumber
        });
    }

    public static void UsedPokerCardRemoved(GameObject usedPokerCardObject)
    {
        OnUsedPokerCardRemoved?.Invoke(null , new UsedPokerCardInformationEventArgs
        {
            UsedPokerCardObject = usedPokerCardObject
        });
    }

    public static void RestorePokerCardAmountVisual(int pokerCardNumberIndex)
    {
        OnRestorePokerCardAmountVisual?.Invoke(null , new UsedPokerCardInformationEventArgs
        {
            PokerCardNumberIndex = pokerCardNumberIndex
        });
    }

    public static void ResetStaticData()
	{
		OnPlayerGetNumber = null;
		OnUsedPokerCardRemoved = null;
		OnRestorePokerCardAmountVisual = null;
	}
}
