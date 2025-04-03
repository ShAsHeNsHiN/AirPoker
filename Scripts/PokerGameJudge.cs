using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PokerGameJudge : MonoBehaviour , IData
{
    private static PokerGameJudge _instance;
    public static PokerGameJudge Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindAnyObjectByType<PokerGameJudge>();

                if(_instance == null)
                {
                    _instance = ComponentCreator<PokerGameJudge>.Create();
                }
            }

            return _instance;
        }
    }

    [SerializeField] private List<WinnerInformation> _winners = new();

    private const string POKERGAMEJUDGE_DATA = "PokerGameJudgeData";

    private ITotalNumberCard _iTotalNumberCards;

    private bool WinnersCountHaveFour => _winners.Count == _iTotalNumberCards.TotalNumberCardAtStart;

    #region Events
    public event EventHandler<WinnerInformationEventArgs> OnWinnerGenerated;

    public event EventHandler<NextRoundInformationEventArgs> OnWinnerGeneratedFinished;

    public class WinnerInformationEventArgs : EventArgs
    {
        public static readonly new WinnerInformationEventArgs Empty = new();

        public WinnerInformation WinnerInformation;
    }

    public class NextRoundInformationEventArgs : EventArgs
    {
        public static readonly new NextRoundInformationEventArgs Empty = new();

        public bool PlayerLose;

        public bool ComputerLose;

        public bool WinnersCountHaveFour;
    }
    #endregion

    private void Awake()
    {
        ResultPokerManager.Instance.OnPokerComparedStart += Handle_PokerCompared;

        OnWinnerGeneratedFinished += Handle_GameEnd;

        OnWinnerGeneratedFinished += Handle_RoundOver;

        _iTotalNumberCards = Player.Instance;
    }

    /// <summary>
    /// 雙方比較撲克牌
    /// </summary>
    public void Handle_PokerCompared(object sender , ResultPokerManager.PokerInformationEventArgs pokerInformationEventArgs)
    {
        var winnerInformation = GetWinnerInformation(ResultPokerManager.Instance);

        // 這個 Event 會讓玩家和電腦扣血
        OnWinnerGenerated?.Invoke(this , new WinnerInformationEventArgs
        {
            WinnerInformation = winnerInformation
        });

        // 我這裡的 Blood 直接抓兩人的 Instance 似乎比較好
        bool playerLose = Player.Instance.Blood <= 0;
        bool computerLose = Computer.Instance.Blood <= 0;

        OnWinnerGeneratedFinished?.Invoke(this , new NextRoundInformationEventArgs
        {
            WinnersCountHaveFour = WinnersCountHaveFour ,
            PlayerLose = playerLose ,
            ComputerLose = computerLose
        });
    }

    /// <summary>
    /// 遊戲結束，傳送至 WinnerScene
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="nextRoundInformationEventArgs"></param>
    private void Handle_GameEnd(object sender , NextRoundInformationEventArgs nextRoundInformationEventArgs)
    {
        // 1)當玩家沒血
        if(nextRoundInformationEventArgs.PlayerLose)
        {
            TableElement.Instance.ChangeScene(ETableElementScene.WinnerScene);

            UIManager.Instance.SceneChanged(ETableElementScene.WinnerScene , EPlayerIdentify.Computer);
        }

        // 2)當電腦沒血
        else if(nextRoundInformationEventArgs.ComputerLose)
        {
            TableElement.Instance.ChangeScene(ETableElementScene.WinnerScene);

            UIManager.Instance.SceneChanged(ETableElementScene.WinnerScene , EPlayerIdentify.Gamer);
        }

        // 3)_winners 有四個時
        else if(nextRoundInformationEventArgs.WinnersCountHaveFour)
        {
            int playerWinCount = default;

            int computerWinCount = default;

            foreach (var item in _winners)
            {
                if(item.Winner == EPlayerIdentify.Gamer)
                {
                    playerWinCount++;
                }

                else
                {
                    computerWinCount++;
                }
            }

            if(playerWinCount > computerWinCount)
            {
                TableElement.Instance.ChangeScene(ETableElementScene.WinnerScene);

                UIManager.Instance.SceneChanged(ETableElementScene.WinnerScene , EPlayerIdentify.Gamer);
            }

            else if (playerWinCount < computerWinCount)
            {
                TableElement.Instance.ChangeScene(ETableElementScene.WinnerScene);

                UIManager.Instance.SceneChanged(ETableElementScene.WinnerScene , EPlayerIdentify.Computer);
            }

            else
            {
                // 平手的話比血量
                if(Player.Instance.Blood > Computer.Instance.Blood)
                {
                    TableElement.Instance.ChangeScene(ETableElementScene.WinnerScene);

                    UIManager.Instance.SceneChanged(ETableElementScene.WinnerScene , EPlayerIdentify.Gamer);
                }

                else if(Player.Instance.Blood < Computer.Instance.Blood)
                {
                    TableElement.Instance.ChangeScene(ETableElementScene.WinnerScene);

                    UIManager.Instance.SceneChanged(ETableElementScene.WinnerScene , EPlayerIdentify.Computer);
                }

                else
                {
                    // 到這邊就沒得比了，宣布平手好了
                    TableElement.Instance.ChangeScene(ETableElementScene.WinnerScene);

                    UIManager.Instance.SceneChanged(ETableElementScene.WinnerScene);
                }
            }
        }
    }

    /// <summary>
    /// 此局結束，傳送至 ResultScene
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="nextRoundInformationEventArgs"></param>
    private void Handle_RoundOver(object sender , NextRoundInformationEventArgs nextRoundInformationEventArgs)
    {
        if(nextRoundInformationEventArgs.PlayerLose || nextRoundInformationEventArgs.ComputerLose || nextRoundInformationEventArgs.WinnersCountHaveFour)
        {
            return;
        }

        StartCoroutine(WaitAndGoToResultScene());
    }

    /// <summary>
    /// 待下一幀再轉至 ResultScene
    /// </summary>
    /// <returns></returns>
    /// <remarks>PokerTotalNumberList_ResultScene 中的 RemoveFirstTotalNumberCard() 中的 Destroy() 會在下一幀才執行，故需要 IEnumerator 的協助</remarks>
    private IEnumerator WaitAndGoToResultScene()
    {
        yield return null;  // 等待到下一幀
        TableElement.Instance.ChangeScene(ETableElementScene.ResultScene);

        UIManager.Instance.SceneChanged(ETableElementScene.ResultScene);
    }

    /// <summary>
    /// 取得贏家資訊
    /// </summary>
    /// <param name="resultPokers">比較的清單</param>
    /// <param name="player">玩家</param>
    /// <param name="computer">電腦</param>
    /// <returns></returns>
    private WinnerInformation GetWinnerInformation(ResultPokerManager resultPokerManager)
    {
        var winner = GetWinner(resultPokerManager);

        // 產生了贏家，接著計算贏家要扣的血量
        var winnerNeedToMinusBlood = WinnerGetMinusBlood(playerIdentify : winner , resultPokerManager);

        var winnerInformation = new WinnerInformation
        {
            Winner = winner ,
            MinusBlood = winnerNeedToMinusBlood
        };

        _winners.Add(winnerInformation);

        return winnerInformation;
    }

    /// <summary>
    /// 產生贏家(比較撲克牌)
    /// </summary>
    /// <param name="playerInformation"></param>
    private EPlayerIdentify GetWinner(ResultPokerManager resultPokerManager)
    {
        // 雙方已組完，開始比較
        var player = resultPokerManager.ResultPokers.Single(item => item.PlayerIdentify == EPlayerIdentify.Gamer);

        var computer = resultPokerManager.ResultPokers.Single(item => item.PlayerIdentify == EPlayerIdentify.Computer);

        EPlayerIdentify winner = default;

        // 先比牌型
        if((int)player.PokerRankings > (int)computer.PokerRankings)
        {
            winner = EPlayerIdentify.Gamer;
        }

        else if((int)player.PokerRankings < (int)computer.PokerRankings)
        {
            winner = EPlayerIdentify.Computer;
        }

        else
        {
            // 皇家同花順不可能兩方同時拿到，因為數字卡 47 僅有 1 張。所以跑進來的都是非皇家同花順的牌型

            if(player.PokerRankings == EPokerRankings.同花順 || computer.PokerRankings == EPokerRankings.同花順)
            {
                var playerCompareListTotal = player.StriaghtTotal;

                var computerCompareListTotal = computer.StriaghtTotal;

                if(playerCompareListTotal > computerCompareListTotal)
                {
                    winner = EPlayerIdentify.Gamer;
                }

                else
                {
                    winner = EPlayerIdentify.Computer;
                }

                return winner;
            }

            ReplaceOneToFourTeen(player.CompareList);
            ReplaceOneToFourTeen(computer.CompareList);

            // 因為牌型一樣，也意謂著雙方的 CompareList 數量一模一樣，因此取誰的都可
            for (int i = 0; i < player.CompareList.Count; i++)
            {
                if(player.CompareList[i] > computer.CompareList[i])
                {
                    winner = EPlayerIdentify.Gamer;
                    return winner;
                }

                if(player.CompareList[i] < computer.CompareList[i])
                {
                    winner = EPlayerIdentify.Computer;
                    return winner;
                }
            }

            // 正常來說，這邊的 return 理應是跑不到，因為雙方的數字卡沒有相同
            return winner;
        }

        return winner;
    }

    /// <summary>
    /// 將清單中的 1 更換成 14 好讓我去比大小
    /// </summary>
    /// <param name="targetCompareList">要更換的清單</param>
    private void ReplaceOneToFourTeen(List<int> targetCompareList)
    {
        for (int i = 0; i < targetCompareList.Count; i++)
        {
            if(targetCompareList[i] == 1)
            {
                targetCompareList[i] = 14;
            }
        }
    }

    /// <summary>
    /// 勝者需要扣血
    /// </summary>
    /// <param name="playerIdentify">勝者</param>
    /// <param name="resultPokers"></param>
    /// <returns></returns>
    private int WinnerGetMinusBlood(EPlayerIdentify playerIdentify , ResultPokerManager resultPokerManager)
    {
        var player = resultPokerManager.ResultPokers.Single(item => item.PlayerIdentify == EPlayerIdentify.Gamer);

        var playerPokerCardToDictionary = player.PokerCards
            .GroupBy(item => item)
            .ToDictionary(group => group.Key , group => group.Count());

        var computer = resultPokerManager.ResultPokers.Single(item => item.PlayerIdentify == EPlayerIdentify.Computer);

        var computerPokerCardToDictionary = computer.PokerCards
            .GroupBy(item => item)
            .ToDictionary(group => group.Key , group => group.Count());

        int minusBlood = default;

        switch (playerIdentify)
        {
            case EPlayerIdentify.Gamer:
                // 扣除電腦的血量

                foreach (var item in playerPokerCardToDictionary)
                {
                    if(computerPokerCardToDictionary.TryGetValue(item.Key , out int value))
                    {
                        minusBlood += playerPokerCardToDictionary[item.Key] > value ? value : playerPokerCardToDictionary[item.Key];
                    }
                }

                break;
            case EPlayerIdentify.Computer:
                // 扣除玩家的血量

                foreach (var item in computerPokerCardToDictionary)
                {
                    if(playerPokerCardToDictionary.TryGetValue(item.Key , out int value))
                    {
                        minusBlood += computerPokerCardToDictionary[item.Key] > value ? value : computerPokerCardToDictionary[item.Key];
                    }
                }

                break;
        }

        return minusBlood;
    }

    public void SaveData()
    {
        PokerGameJudgeData judgePokerData = new()
        {
            Winners = new(_winners)
        };

        PersistentDataManager<PokerGameJudgeData>.SaveData(judgePokerData , POKERGAMEJUDGE_DATA);
    }

    public void GetData()
    {
        var pokerGameJudgeData = PersistentDataManager<PokerGameJudgeData>.GetDataChecked(POKERGAMEJUDGE_DATA);

        if(pokerGameJudgeData != null)
        {
            _winners = new(pokerGameJudgeData.Winners);
        }
    }
}