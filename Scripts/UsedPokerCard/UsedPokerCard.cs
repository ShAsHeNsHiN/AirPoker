using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Linq;

public class UsedPokerCard : MonoBehaviour , IPointerClickHandler
{
	private const int USEDPOKERCARDNUMBER_INDEX = 2;

	private int GetNumber() => int.Parse(transform.GetChild(USEDPOKERCARDNUMBER_INDEX).name);

	// PokerCard 我都是照數字順序排列，數字若是 1 ~ 13，那 Index 就是 0 ~ 12
	// Ex : 假設我選的為撲克牌 6 ，那它在 MakePokerListUI 的 Index 就為 5(MakePokerListUI[5])
	private int GetPokerCardNumberIndex() => GetNumber() - 1;

	public void OnPointerClick(PointerEventData eventData)
	{
		Chosen();
	}

	public void Chosen()
	{
		UsedPokerCardManager.PlayerGetNumber(GetNumber());

		UsedPokerCardManager.RestorePokerCardAmountVisual(GetPokerCardNumberIndex());

		UsedPokerCardManager.UsedPokerCardRemoved(gameObject);
	}
}
