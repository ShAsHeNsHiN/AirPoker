using System;
using System.Collections.Generic;

public interface ITotalNumberCard
{
    /// <summary>
    /// 當玩家選擇數字卡時會觸發
    /// </summary>
    public event EventHandler<CheckingEventArgs> OnCheckEveryPlayerChooseTotalNumberCardFinished;

    public class CheckingEventArgs : EventArgs
    {
        public static readonly new CheckingEventArgs Empty = new();
    }

    /// <summary>
    /// 可選擇的數字卡數量
    /// </summary>
    public int TotalNumberCardAtStart {get;}

    /// <summary>
    /// 數字卡清單
    /// </summary>
    /// <returns></returns>
    public List<int> TotalNumberCards {get;}

    /// <summary>
    /// 判斷玩家是否選好數字卡(選好的數量為 TotalNumberCardAtStart)
    /// </summary>
    /// <returns></returns>
    public bool ChooseTotalNumberCardFinished {get;}

    /// <summary>
    /// 將選擇的數字卡加入數字卡清單
    /// </summary>
    /// <param name="target_Number">選擇的數字</param>
    public void AddTotalNumberCard(int target_Number);

    /// <summary>
    /// 將選擇的數字卡移至首項--MakePokerScene 要用
    /// </summary>
    /// <param name="target_Number">選擇的數字</param>
    public void SetChosenTotalNumberCardToFront(int target_Number);

    /// <summary>
    /// 移除首張數字卡--一局遊戲結束要用
    /// </summary>
    public void RemoveFirstTotalNumberCard();
}