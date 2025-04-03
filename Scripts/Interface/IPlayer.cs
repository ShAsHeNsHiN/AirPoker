using System;

public interface IPlayer
{
    /// <summary>
    /// 當玩家生成時觸發
    /// </summary>
    public event EventHandler<InstantiateInformationEventArgs> OnPlayerInstantiate;

    public class InstantiateInformationEventArgs : EventArgs
    {
        public static readonly new InstantiateInformationEventArgs Empty = new();

        public PlayerBase PlayerBase;
    }

    /// <summary>
    /// 玩家身份
    /// </summary>
    public EPlayerIdentify PlayerIdentify {get;}

    /// <summary>
    /// 玩家原始血量
    /// </summary>
    public int OriginalBlood{get;}

    /// <summary>
    /// 玩家血量
    /// </summary>
    public int Blood {get;}
}