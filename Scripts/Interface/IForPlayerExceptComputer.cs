using System;
using System.Collections.Generic;

public interface IForPlayerExceptComputer
{
    /// <summary>
    /// 在 UsedPokerCard 有 5 張或 UsedPokerCard 被銷毀時觸發
    /// </summary>
    public event EventHandler<ReadyButtonInformationEventArgs> OnSetReadyButtonActiveTo;

    public class ReadyButtonInformationEventArgs : EventArgs
    {
        public static readonly new ReadyButtonInformationEventArgs Empty = new();

        public bool WantToState;
    }

    /// <summary>
    /// 在 MakePokerScene 點擊非最左邊的數字觸發
    /// </summary>
    public event EventHandler<TotalNumberCardsInformationEventArgs> OnTotalNumberCardsOrderChanged;

    public class TotalNumberCardsInformationEventArgs : EventArgs
    {
        public static readonly new TotalNumberCardsInformationEventArgs Empty = new();

        public List<int> TotalNumberCards;
    }

    /// <summary>
    /// 需訂閱的事件
    /// </summary>
    public void SubscribeEventsForPlayer();
}
