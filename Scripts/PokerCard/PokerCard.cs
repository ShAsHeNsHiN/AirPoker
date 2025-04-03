using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PokerCard : MonoBehaviour , IHasProgress, IPointerClickHandler
{
    [SerializeField] private Transform _stateUITransform;

    public event EventHandler<IHasProgress.OnPrgressChangedEventArgs> OnProgressChanged;

    private const int NUMBER_INDEX_IN_POKERCARD = 2;
    private const float POKERCARD_NORMAL_AMOUNT = 4;

    private int GetPokerCardNumber() => int.Parse(transform.GetChild(NUMBER_INDEX_IN_POKERCARD).name);

    private PokerCardDictionary GetPokerCardDictionary() => PokerCardDictionary.Instance;

    private Player GetPlayer() => Player.Instance;

    /// <summary>
    /// 撲克牌數量是否為 3 張
    /// </summary>
    private bool _countIsThree;

    /// <summary>
    /// 將每張撲克牌的初始化數量呈現
    /// </summary>
    /// <param name="pokerCardDictionary">撲克牌來源</param>
    public void InitialAmountVisual(PokerCardDictionary target_PokerCardDictionary)
    {
        var pokerCardDictionary = target_PokerCardDictionary.PokerCardDict;

        OnProgressChanged?.Invoke(this , new IHasProgress.OnPrgressChangedEventArgs
        {
            progressNormalized = pokerCardDictionary[GetPokerCardNumber()] / POKERCARD_NORMAL_AMOUNT
        });

        _stateUITransform.GetComponent<StateUI>().BarColorTurnToBlue();
    }

    /// <summary>
    /// 確保撲克牌不會選超過
    /// </summary>
    /// <param name="pokerCardDictionary">撲克牌來源</param>
    public bool PokerCardCanBeChoosed(PokerCardDictionary target_PokerCardDictionary)
    {
        var pokerCardDictionary = target_PokerCardDictionary.PokerCardDict;

        if(pokerCardDictionary[GetPokerCardNumber()] > 0)
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    /// <summary>
    /// 選取撲克牌後的回饋
    /// </summary>
    /// <param name="pokerCardDictionary">撲克牌來源</param>
    public void MinusPokerCardAmountVisual(PokerCardDictionary target_PokerCardDictionary)
    {
        var pokerCardDictionary = target_PokerCardDictionary.PokerCardDict;

        pokerCardDictionary[GetPokerCardNumber()]--;

        // 數量為 3 的才需要特別規範
        if(_countIsThree)
        {
            if(pokerCardDictionary[GetPokerCardNumber()] == 0)
            {
                // 若是此撲克牌的數量為 3，當玩家對此撲克牌點 4 張時，我需要有紅色的進度條

                // 1.Progress 變 1 / 4
                OnProgressChanged?.Invoke(this , new IHasProgress.OnPrgressChangedEventArgs
                {
                    progressNormalized = 1 / POKERCARD_NORMAL_AMOUNT
                });

                // 2.進度條變紅色
                _stateUITransform.GetComponent<StateUI>().BarColorTurnToRed();
            }

            else
            {
                // 特效為 3 張，但實際是 4 張，所以進度條不能跟著字典數量跑
                var effectAmount = pokerCardDictionary[GetPokerCardNumber()] - 1;

                OnProgressChanged?.Invoke(this , new IHasProgress.OnPrgressChangedEventArgs
                {
                    progressNormalized = effectAmount / POKERCARD_NORMAL_AMOUNT
                });
            }
        }

        else
        {   
            OnProgressChanged?.Invoke(this , new IHasProgress.OnPrgressChangedEventArgs
            {
                progressNormalized = pokerCardDictionary[GetPokerCardNumber()] / POKERCARD_NORMAL_AMOUNT
            });
        }
    }

    /// <summary>
    /// 讓特殊規則的撲克牌反映在進度條上
    /// </summary>
    /// <param name="pokerCardDictionary">撲克牌來源</param>
    public void PokerEffectForSpecialRule(PokerCardDictionary target_PokerCardDictionary)
    {
        _countIsThree = true;

        var pokerCardDictionary = target_PokerCardDictionary.PokerCardDict;

        int effectAmount = pokerCardDictionary[GetPokerCardNumber()] - 1;

        OnProgressChanged?.Invoke(this , new IHasProgress.OnPrgressChangedEventArgs
        {
            progressNormalized = effectAmount / POKERCARD_NORMAL_AMOUNT
        });
    }

    /// <summary>
    /// 取消選取撲克牌後的回饋
    /// </summary>
    /// <param name="pokerCardDictionary">撲克牌來源</param>
    public void RestorePokerCardAmountVisual(PokerCardDictionary target_PokerCardDictionary)
    {
        var pokerCardDictionary = target_PokerCardDictionary.PokerCardDict;

        pokerCardDictionary[GetPokerCardNumber()]++;

        // 數量為 3 的才需要特別規範
        if(_countIsThree)
        {
            var effectAmount = pokerCardDictionary[GetPokerCardNumber()] - 1;

            OnProgressChanged?.Invoke(this , new IHasProgress.OnPrgressChangedEventArgs
            {
                progressNormalized = effectAmount / POKERCARD_NORMAL_AMOUNT
            });
        }

        else
        {
            OnProgressChanged?.Invoke(this , new IHasProgress.OnPrgressChangedEventArgs
            {
                progressNormalized = pokerCardDictionary[GetPokerCardNumber()] / POKERCARD_NORMAL_AMOUNT
            });
        }

        // 進度條變藍色
        _stateUITransform.GetComponent<StateUI>().BarColorTurnToBlue();
    }

    // *這樣比剛才用 Instance 漂亮多了，但還是差了一點。我比較希望這個 Func 能被 Player 訂閱並自動執行
    /// <summary>
    /// 被選取後處理
    /// </summary>
    public void Chosen()
    {
        if(PokerCardCanBeChoosed(GetPokerCardDictionary()) && !GetPlayer().ChoosePokerCardFinished)
        {
            MinusPokerCardAmountVisual(GetPokerCardDictionary());

            PokerCardManager.PlayerGetNumber(GetPokerCardNumber());

            PokerCardManager.UsedPokerCardInstantiate(GetPlayer().PokerCards);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Chosen();
    }
}
