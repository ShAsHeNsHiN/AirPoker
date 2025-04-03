using System.Collections.Generic;
using UnityEngine;

public class PokerCardDictionary : MonoBehaviour , IData
{
    private static PokerCardDictionary _instance;
    public static PokerCardDictionary Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindAnyObjectByType<PokerCardDictionary>();

                if(_instance == null)
                {
                    _instance = ComponentCreator<PokerCardDictionary>.Create();
                }
            }

            return _instance;
        }
    }

    private const string POKERCARD_DICTIONARY_DATA = "PokerCardDictionaryData";

    private const int POKERCARD_NUMBER = 13;

    private readonly Dictionary<int , int> _pokerCardDict = new();

    public Dictionary<int , int> PokerCardDict
    {
        get
        {
            if(_pokerCardDict.Count == 0)
            {
                InstantiatePokerCards(_pokerCardDict);
            }

            return _pokerCardDict;
        }
    }

    private void Awake()
    {
        NextRoundButton.Instance.OnNextRound += Handle_ClearPokerCardDictionary;
    }

    /// <summary>
    /// 重置 PokerCardDictionary
    /// </summary>
    public void Handle_ClearPokerCardDictionary()
    {
        _pokerCardDict.Clear();
    }

    private void InstantiatePokerCards(Dictionary<int , int> target_PokerCardDictionary)
    {
        int pokerCardMinimumValue = 1;
        int pokerCardMaximumValue = 13;
        int pokerCardInitialAmount = 4;

        // 每次初始化時先清空，避免新舊資料衝突
        target_PokerCardDictionary.Clear();

        // 初始化撲克牌數量
        for (int i = pokerCardMinimumValue; i <= pokerCardMaximumValue; i++)
        {
            target_PokerCardDictionary.Add(i , pokerCardInitialAmount);
        }
    }

    public void SaveData()
    {
        PokerCardDictionaryData pokerCardDictionaryData = new()
        {
            PokerCardNumber = new(_pokerCardDict.Keys) ,
            PokerCardNumberCounts = new(_pokerCardDict.Values)
        };

        PersistentDataManager<PokerCardDictionaryData>.SaveData(pokerCardDictionaryData , POKERCARD_DICTIONARY_DATA);
    }

    public void GetData()
    {
        var pokerCardDictionaryData = PersistentDataManager<PokerCardDictionaryData>.GetDataChecked(POKERCARD_DICTIONARY_DATA);

        if(pokerCardDictionaryData != null)
        {
            // *適用於在選數字卡儲存檔案
            if(pokerCardDictionaryData.PokerCardNumber.Count == 0 && pokerCardDictionaryData.PokerCardNumberCounts.Count == 0)
            {
                return;
            }

            for (int i = 0; i < POKERCARD_NUMBER; i++)
            {
                _pokerCardDict.Add(pokerCardDictionaryData.PokerCardNumber[i] , pokerCardDictionaryData.PokerCardNumberCounts[i]);
            }
        }
    }
}
