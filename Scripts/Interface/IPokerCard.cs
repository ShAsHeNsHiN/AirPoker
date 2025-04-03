using System.Collections.Generic;

public interface IPokerCard
{
    public int PokerCardAmountMax {get;}

    public List<int> PokerCards {get;}

    /// <summary>
    /// 判斷玩家是否組好撲克牌(選好的數量為 PokerCardAmountMax)
    /// </summary>
    /// <returns></returns>
    public bool ChoosePokerCardFinished {get;}

    /// <summary>
    /// 將選擇的撲克牌加入撲克牌清單
    /// </summary>
    /// <param name="target_Number">選擇的數字</param>
    public void AddPokerCard(int target_Number);

    /// <summary>
    /// 移除選擇的撲克牌
    /// </summary>
    /// <param name="target_Number">選擇的數字</param>
    public void RemovePokerCard(int target_Number);

    /// <summary>
    /// 清空組牌清單
    /// </summary>
    public void ResetPokerCard();
}