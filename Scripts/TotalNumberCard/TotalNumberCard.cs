using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TotalNumberCard : MonoBehaviour , IPointerClickHandler
{
    [SerializeField] private EClickTotalNumberCardScenes  _eClickTotalNumberCardScenes;

    private Color32 chosenColor = new(230 , 149 , 169 , 255);

    private const string BACKGROUND = "Background";

    private const int TOTALNUMBER_INDEX = 2;

    private int GetNumber() => int.Parse(transform.GetChild(TOTALNUMBER_INDEX).name);

    public void OnPointerClick(PointerEventData eventData)
    {
        Chosen();
    }

    /// <summary>
    /// 被選取後處理
    /// </summary>
    public void Chosen()
    {
        switch(_eClickTotalNumberCardScenes)
        {
            case EClickTotalNumberCardScenes.ChoosePokerTotalNumberScene:

                // 先給視覺回饋，再做資料處理
                TotalNumberCardChangesColor(true);

                TotalNumberCardManager.PlayerGetNumberInChoosePokerTotalNumberScene(GetNumber());

                break;
            case EClickTotalNumberCardScenes.MakePokerScene:

                TotalNumberCardManager.PlayerGetNumberInMakePokerScene(GetNumber());

                break;
        }
    }

    /// <summary>
    /// 玩家選擇後的回饋
    /// </summary>
    /// <param name="changedColorOrNot">變色有無</param>
    public void TotalNumberCardChangesColor(bool changedColorOrNot)
    {
        if(changedColorOrNot)
        {
            transform.Find(BACKGROUND).GetComponent<Image>().color = chosenColor;
        }

        else
        {
            transform.Find(BACKGROUND).GetComponent<Image>().color = Color.white;
        }
    }
}