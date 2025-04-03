using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokerCardCountsMinusOneDictionary : MonoBehaviour , IData
{
    private static PokerCardCountsMinusOneDictionary _instance;
    public static PokerCardCountsMinusOneDictionary Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindAnyObjectByType<PokerCardCountsMinusOneDictionary>();
            }

            return _instance;
        }
    }

    private readonly Dictionary<int , int> _pokerCardCountsMinusOneDict = new();

    public Dictionary<int , int> PokerCardCountsMinusOneDict
    {
        get
        {
            if(_pokerCardCountsMinusOneDict.Count == 0)
            {
                RandomChooseThreeNumber();
            }

            return _pokerCardCountsMinusOneDict;
        }
    }

    private const int POKRECARD_MINUS_COUNTS = 3;

    private const string POKERCARD_COUNTS_MINUS_ONE_DICTIONARY_DATA = "PokerCardCountsMinusOneDictionaryData";

    private void Awake()
    {
        NextRoundButton.Instance.OnNextRound += Handle_ClearPokerCardCountsMinusOneDictionary;
    }

    private void RandomChooseThreeNumber()
    {
        while (_pokerCardCountsMinusOneDict.Count < POKRECARD_MINUS_COUNTS)
        {
            int chooseNumber = Random.Range(4 , 14);

            if(!_pokerCardCountsMinusOneDict.ContainsKey(chooseNumber))
            {
                _pokerCardCountsMinusOneDict.Add(chooseNumber , 1);
            }
        }
    }

    /// <summary>
    /// 重置 PokerCardCountsMinusOneDictionary
    /// </summary>
    public void Handle_ClearPokerCardCountsMinusOneDictionary()
    {
        _pokerCardCountsMinusOneDict.Clear();
    }

    public void SaveData()
    {
        PokerCardCountsMinusOneDictionaryData pokerCardCountsMinusOneDictionaryData = new()
        {
            PokerCardNumber = new(_pokerCardCountsMinusOneDict.Keys) ,
            PokerCardNumberCounts = new(_pokerCardCountsMinusOneDict.Values)
        };

        PersistentDataManager<PokerCardCountsMinusOneDictionaryData>.SaveData(pokerCardCountsMinusOneDictionaryData , POKERCARD_COUNTS_MINUS_ONE_DICTIONARY_DATA);
    }

    public void GetData()
    {
        var pokerCardCountsMinusOneDictionaryData = PersistentDataManager<PokerCardCountsMinusOneDictionaryData>.GetDataChecked(POKERCARD_COUNTS_MINUS_ONE_DICTIONARY_DATA);

        if(pokerCardCountsMinusOneDictionaryData != null)
        {
            // *適用於在選數字卡儲存檔案
            if(pokerCardCountsMinusOneDictionaryData.PokerCardNumber.Count == 0 && pokerCardCountsMinusOneDictionaryData.PokerCardNumberCounts.Count == 0)
            {
                return;
            }

            for (int i = 0; i < POKRECARD_MINUS_COUNTS; i++)
            {
                _pokerCardCountsMinusOneDict.Add(pokerCardCountsMinusOneDictionaryData.PokerCardNumber[i] , pokerCardCountsMinusOneDictionaryData.PokerCardNumberCounts[i]);
            }
        }
    }
}
