using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class ResultPokerManager : MonoBehaviour , IData
{
    private static ResultPokerManager _instance;
    public static ResultPokerManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindAnyObjectByType<ResultPokerManager>();

                if(_instance == null)
                {
                    _instance = ComponentCreator<ResultPokerManager>.Create();
                }
            }

            return _instance;
        }
    }

    [SerializeField] private List<ResultPoker> _resultPokers = new();

    private const string RESULTPOKERMANAGER_DATA = "ResultPokerManagerData";

    public List<ResultPoker> ResultPokers => _resultPokers;

    private bool EachPlayerReady() => _resultPokers.Count == PlayerManager.Instance.PlayersCount;

    public event EventHandler<PokerInformationEventArgs> OnPokerComparedStart;

    public class PokerInformationEventArgs : EventArgs
    {
        public static readonly new PokerInformationEventArgs Empty = new();
    }

    private void Awake()
    {
        PokerGameJudge.Instance.OnWinnerGenerated += Handle_ClearResultPokers;
    }

    /// <summary>
    /// 分析玩家的撲克牌--牌型及比較清單
    /// </summary>
    public void Handle_AnalyzePoker(object sender , IJudgePoker.PlayerInformationEventArgs playerInformationEventArgs)
    {
        // 阻擋同一個人重覆輸入資料
        if(_resultPokers.Any(item => item.PlayerIdentify == playerInformationEventArgs.PlayerBase.PlayerIdentify))
        {
            return;
        }

        ResultPoker resultPoker = new()
        {
            PlayerIdentify = playerInformationEventArgs.PlayerBase.PlayerIdentify ,
            PokerCards = new(playerInformationEventArgs.PlayerBase.PokerCards) ,
        };

        var playerPokerCards = playerInformationEventArgs.PlayerBase.PokerCards
            .GroupBy(item => item)
            .ToDictionary(group => group.Key , group => group.Count());

        var reversePlayerPokerCards = playerPokerCards
            .ToLookup(group => group.Value , group => group.Key);

        EPossiblePokerRankings possiblePokerRankings = (EPossiblePokerRankings)playerPokerCards.Count;

        switch (possiblePokerRankings)
        {
            case EPossiblePokerRankings.鐵支或葫蘆:

                if(playerPokerCards.ContainsValue(4))
                {
                    var firstCompare = reversePlayerPokerCards[4].Single();

                    var secondCompare = reversePlayerPokerCards[1].Single();

                    resultPoker.PokerRankings = EPokerRankings.鐵支;

                    resultPoker.CompareList = new()
                    {
                        firstCompare ,
                        secondCompare
                    };
                }

                else
                {
                    var firstCompare = reversePlayerPokerCards[3].Single();

                    var secondCompare = reversePlayerPokerCards[2].Single();

                    resultPoker.PokerRankings = EPokerRankings.葫蘆;

                    resultPoker.CompareList = new()
                    {
                        firstCompare ,
                        secondCompare
                    };
                }

                break;
            case EPossiblePokerRankings.三條或兔胚:

                if(playerPokerCards.ContainsValue(3))
                {
                    var oneAmountPokerInThreeOfAKind = reversePlayerPokerCards[1].OrderBy(item => item).ToArray();

                    var firstCompare = reversePlayerPokerCards[3].Single();

                    var secondCompare = oneAmountPokerInThreeOfAKind[1];

                    var thirdCompare = oneAmountPokerInThreeOfAKind[0];

                    resultPoker.PokerRankings = EPokerRankings.三條;

                    resultPoker.CompareList = new()
                    {
                        firstCompare ,
                        secondCompare ,
                        thirdCompare
                    };
                }

                else
                {
                    var twoAmountPokerInTwoPairs = reversePlayerPokerCards[2].OrderBy(item => item).ToArray();

                    var firstCompare = twoAmountPokerInTwoPairs[1];

                    var secondCompare = twoAmountPokerInTwoPairs[0];

                    var thirdCompare = reversePlayerPokerCards[1].Single();

                    resultPoker.PokerRankings = EPokerRankings.兔胚;

                    resultPoker.CompareList = new()
                    {
                        firstCompare ,
                        secondCompare ,
                        thirdCompare
                    };
                }

                break;
            case EPossiblePokerRankings.單胚:
                {
                    var oneAmountPokerInOnePair = reversePlayerPokerCards[1].OrderBy(item => item).ToArray();

                    var firstCompare = reversePlayerPokerCards[2].Single();

                    var secondCompare = oneAmountPokerInOnePair[2];

                    var thirdCompare = oneAmountPokerInOnePair[1];

                    var fourthCompare = oneAmountPokerInOnePair[0];

                    resultPoker.PokerRankings = EPokerRankings.單胚;

                    resultPoker.CompareList = new()
                    {
                        firstCompare ,
                        secondCompare ,
                        thirdCompare ,
                        fourthCompare
                    };
                }

                break;
            case EPossiblePokerRankings.散牌或同花順:

                var pokerCardKeysArray = reversePlayerPokerCards[1].OrderBy(item => item).ToArray();

                bool isStraight = true;

                #region 判斷皇家同花順
                int[] royalStraight = new []{1 , 10 , 11 , 12 , 13};

                // 皇家同花順只會有一種
                bool isRoyalStraight = new HashSet<int>(pokerCardKeysArray).SetEquals(royalStraight);

                if(isRoyalStraight)
                {
                    resultPoker.PokerRankings = EPokerRankings.皇家同花順;

                    break;
                }
                #endregion

                #region 判斷一般的順子
                for (int i = 0; i < pokerCardKeysArray.Length - 1; i++)
                {
                    if((pokerCardKeysArray[i + 1] - pokerCardKeysArray[i]) == 1)
                    {
                        continue;
                    }

                    else
                    {
                        isStraight = false;
                        break;
                    }
                }
                #endregion

                if(isStraight)
                {
                    var oneAmountPokerInFiveCards = reversePlayerPokerCards[1].OrderBy(item => item).ToArray();

                    resultPoker.PokerRankings = EPokerRankings.同花順;

                    resultPoker.StriaghtTotal = pokerCardKeysArray.Sum();
                }

                else
                {
                    var oneAmountPokerInFiveCards = reversePlayerPokerCards[1].OrderBy(item => item).ToArray();

                    var firstCompare = oneAmountPokerInFiveCards[4];

                    var secondCompare = oneAmountPokerInFiveCards[3];

                    var thirdCompare = oneAmountPokerInFiveCards[2];

                    var fourthCompare = oneAmountPokerInFiveCards[1];

                    var fifthCompare = oneAmountPokerInFiveCards[0];

                    resultPoker.PokerRankings = EPokerRankings.散牌;

                    resultPoker.CompareList = new()
                    {
                        firstCompare ,
                        secondCompare, 
                        thirdCompare ,
                        fourthCompare ,
                        fifthCompare
                    };
                }

                break;
        }

        _resultPokers.Add(resultPoker);

        if(EachPlayerReady())
        {
            OnPokerComparedStart?.Invoke(this , PokerInformationEventArgs.Empty);
        }
    }

    /// <summary>
    /// 當勝者生成時，撲克牌型清單才能清空
    /// </summary>
    public void Handle_ClearResultPokers(object sender , PokerGameJudge.WinnerInformationEventArgs winnerInformationEventArgs)
    {
        _resultPokers.Clear();
    }

    public void GetData()
    {
        var resultPokerManagerData = PersistentDataManager<ResultPokerManagerData>.GetDataChecked(RESULTPOKERMANAGER_DATA);

        if(resultPokerManagerData != null)
        {
            _resultPokers = new(resultPokerManagerData.ResultPokers);
        }
    }

    public void SaveData()
    {
        ResultPokerManagerData resultPokerManagerData = new()
        {
            ResultPokers = new(_resultPokers)
        };

        PersistentDataManager<ResultPokerManagerData>.SaveData(resultPokerManagerData , RESULTPOKERMANAGER_DATA);
    }
}
