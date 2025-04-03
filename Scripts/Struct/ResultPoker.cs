using System;
using System.Collections.Generic;

[Serializable]
public struct ResultPoker
{
    public EPlayerIdentify PlayerIdentify;

    public EPokerRankings PokerRankings;

    public int Blood;

    // 會用 List 是因為我不想一個個變數下去比較，直接寫個 for 迴圈就能比完
    // *compareList[0] = FirstCompare , compareList[1] = SecondCompare , ……
    public List<int> CompareList;

    public List<int> PokerCards;

    public int StriaghtTotal;
}