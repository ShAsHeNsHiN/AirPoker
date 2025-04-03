using System;

public interface IJudgePoker
{
    public bool IsValidPoker{get;}

    public event EventHandler<PlayerInformationEventArgs> OnPlayerReadyToAnalyzePoker;

    public class PlayerInformationEventArgs : EventArgs
    {
        public static readonly new PlayerInformationEventArgs Empty = new();

        public PlayerBase PlayerBase;
    }

    /// <summary>
    /// 準備好分析牌型
    /// </summary>
    public void ReadyToJudgePoker();

    /// <summary>
    /// 贏家扣血
    /// </summary>
    /// <param name="minusBlood">要扣的血</param>
    public void WinnerMinusBlood(EPlayerIdentify winner , int minusBlood);

    /// <summary>
    /// 撲克牌用超過需要扣血
    /// </summary>
    public void PokerCardOverUsed();
}