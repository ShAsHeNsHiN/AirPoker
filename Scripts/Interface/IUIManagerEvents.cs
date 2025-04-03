using System;
using UnityEngine;

public interface IUIManagerEvents
{
    /// <summary>
    /// 選擇數字卡
    /// </summary>
    public event EventHandler<TotalNumberCardInformationArgs> OnChooseTotalNumberCard;

    public class TotalNumberCardInformationArgs : EventArgs
    {
        public static readonly new TotalNumberCardInformationArgs Empty = new();
    }

    /// <summary>
    /// 組牌開始
    /// </summary>
    public event EventHandler<MakePokerEventArgs> OnMakePoker;

    public class MakePokerEventArgs : EventArgs
    {
        public static readonly new MakePokerEventArgs Empty = new();

        public int ExecutionCount;
    }

    public event EventHandler<PokerVsEventArgs> OnPokerVs;

    public class PokerVsEventArgs : EventArgs
    {
        public static readonly new PokerVsEventArgs Empty = new();
    }

    /// <summary>
    /// 每局結束
    /// </summary>
    public event EventHandler<ResultEventArgs> OnResult;

    public class ResultEventArgs : EventArgs
    {
        public static readonly new ResultEventArgs Empty = new();
    }

    /// <summary>
    /// 遊戲結束
    /// </summary>
    public event EventHandler<GameEndEventArgs> OnGameEnd;

    public class GameEndEventArgs : EventArgs
    {
        public static readonly new GameEndEventArgs Empty = new();

        public EPlayerIdentify? WinnerEPlayerIdentify;
    }
}