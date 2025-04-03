using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderExecution : MonoBehaviour
{
    private string testString1;
    private string testString2;

    // 這 Class 讓我先把順序給釐清(完成!)
    private void Test()
    {
        // Player.Instance.SetSavedDataString(testString1);
        // Computer.Instance.SetSavedDataString(testString2);

        // PokerCardPool.Instance.Initialize();
        // UsedPokerCardPool.Instance.Initialize(); // 這其實不太重要，不過為了一致性還是將它擺進來

        MyGameManager.LoadData();

        // PlayerInformation_MakePokerScene 生成數字卡
        // PlayerInformation_ResultScene 生成數字卡
        // *生成數字卡只要在開始遊戲執行一次就好

        TableElement.Instance.ChangeScene(TableElement.Instance.CurrentScene);

        UIManager.Instance.SceneChanged(TableElement.Instance.CurrentScene);
    }
}
