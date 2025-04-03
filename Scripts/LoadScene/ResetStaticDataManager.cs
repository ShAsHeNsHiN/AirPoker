using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetStaticDataManager : MonoBehaviour
{
    // 通常這支程式會放在 MainMenu
    private void Awake()
    {
        ResetStaticData();
    }

    public static void ResetStaticData()
    {
        TotalNumberCardManager.ResetStaticData();
        PokerCardManager.ResetStaticData();
        UsedPokerCardManager.ResetStaticData();
    }
}
