public interface IPlayerInformation
{
    /// <summary>
    /// 生成數字卡
    /// </summary>
    public void GenerateTotalNumberCards();

    /// <summary>
    /// 更新玩家名字
    /// </summary>
    public void UpdatePlayerName();

    /// <summary>
    /// 更新玩家數字卡
    /// </summary>
    public void UpdateTotalNumberCardVisual();

    /// <summary>
    /// 更新玩家血量
    /// </summary>
    public void UpdatePlayerBlood();

    /// <summary>
    /// 移除首張數字卡
    /// </summary>
    public void RemoveFirstTotalNumberCard();
}