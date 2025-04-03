using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChoosePokerTotalNumberListUI : MonoBehaviour
{
    private const string TOTALNUMBERTEXT = "TotalNumberText";

    [SerializeField] private Transform _totalNumberCardTransform;

    private void Awake()
    {
        UIManager.Instance.OnChooseTotalNumberCard += Handle_InstantiateTotalNumberCards;
    }

    private void Handle_InstantiateTotalNumberCards(object sender , IUIManagerEvents.TotalNumberCardInformationArgs totalNumberCardInformationArgs)
    {
        InstantiateTotalNumberCards();
    }

    private void InstantiateTotalNumberCards()
    {
        int nameIndexInCardTransform = 2;
        int totalNumberStart = 15;
        int totalNumberEnd = 50;

        int totalNumberCardAmountInOneRow = (totalNumberEnd - totalNumberStart + 1) / transform.childCount;

        int currentRow = 0;

        for(int totalNumber = totalNumberStart ; totalNumber <= totalNumberEnd ; totalNumber++)
        {
            #region 設計數字卡樣式
            _totalNumberCardTransform.GetChild(nameIndexInCardTransform).name = totalNumber.ToString();

            _totalNumberCardTransform.Find(TOTALNUMBERTEXT).GetComponent<TextMeshProUGUI>().text = _totalNumberCardTransform.GetChild(nameIndexInCardTransform).name;
            #endregion

            // 一排放滿後換下一排
            if(transform.GetChild(currentRow).childCount == totalNumberCardAmountInOneRow)
            {
                currentRow++;
            }

            Instantiate(_totalNumberCardTransform , transform.GetChild(currentRow));
        }
    }
}
