using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class PokerListUI : MonoBehaviour
{
    private void Start()
    {
        UsedPokerCardManager.OnRestorePokerCardAmountVisual += Handle_RestorePokerCardAmountVisual;
    }

    private void Handle_RestorePokerCardAmountVisual(object sender , UsedPokerCardManager.UsedPokerCardInformationEventArgs usedPokerCardInformationEventArgs)
    {
        transform.GetChild(usedPokerCardInformationEventArgs.PokerCardNumberIndex).GetComponent<PokerCard>().RestorePokerCardAmountVisual(PokerCardDictionary.Instance);
    }
}
