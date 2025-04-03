using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ReadyButton : MonoBehaviour , IPointerClickHandler
{
    private static ReadyButton instance;
    public static ReadyButton Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindAnyObjectByType<ReadyButton>();
            }

            return instance;
        }
    }

    public event EventHandler<PlayerInformationEventArgs> OnPlayerClickReadyButton;

    public class PlayerInformationEventArgs : EventArgs
    {
        public static readonly new PlayerInformationEventArgs Empty;
    }

    private void Awake()
    {
        UsedPokerCardPool.Instance.OnCardPoolEmpty += Handle_SetReadyButtonActive_UsedPokerCardPool;        
    }

    private void Start()
    {   
        SetReadyButtonActiveTo(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnPlayerClickReadyButton?.Invoke(this , PlayerInformationEventArgs.Empty);
    }

    public void Handle_SetReadyButtonActive_IForPlayerExceptComputer(object sender , IForPlayerExceptComputer.ReadyButtonInformationEventArgs readyButtonInformationEventArgs)
    {
        SetReadyButtonActiveTo(readyButtonInformationEventArgs.WantToState);
    }

    public void Handle_SetReadyButtonActive_UsedPokerCardPool(object sender , UsedPokerCardPool.CardPoolInformationEventArgs cardPoolInformationEventArgs)
    {
        SetReadyButtonActiveTo(cardPoolInformationEventArgs.WantToState);
    }

    private void SetReadyButtonActiveTo(bool wantToState)
    {
        gameObject.SetActive(wantToState);
    }
}
