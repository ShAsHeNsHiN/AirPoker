using System;
using UnityEngine;

public class UIManager : MonoBehaviour , IUIManagerEvents
{
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindAnyObjectByType<UIManager>();

                if(_instance == null)
                {
                    _instance = ComponentCreator<UIManager>.Create();
                }
            }

            return _instance;
        }
    }

    public event EventHandler<IUIManagerEvents.TotalNumberCardInformationArgs> OnChooseTotalNumberCard;

    public event EventHandler<IUIManagerEvents.MakePokerEventArgs> OnMakePoker;

    public event EventHandler<IUIManagerEvents.PokerVsEventArgs> OnPokerVs;

    public event EventHandler<IUIManagerEvents.ResultEventArgs> OnResult;

    public event EventHandler<IUIManagerEvents.GameEndEventArgs> OnGameEnd;

    // *專門給 OnNextRound 用的
    private void Handle_SceneChangedToMakePokerScene()
    {
        SceneChanged(ETableElementScene.MakePokerScene);
    }

    public void SceneChanged(ETableElementScene eTableElementScene , EPlayerIdentify? winnerPlayerIdentify = default)
    {
        switch (eTableElementScene)
        {
            case ETableElementScene.ChoosePokerTotalNumberScene:

                OnChooseTotalNumberCard?.Invoke(this , IUIManagerEvents.TotalNumberCardInformationArgs.Empty);

                break;

            case ETableElementScene.MakePokerScene:

                OnMakePoker?.Invoke(this , IUIManagerEvents.MakePokerEventArgs.Empty);

                break;

            case ETableElementScene.VsScene:

                OnPokerVs?.Invoke(this , IUIManagerEvents.PokerVsEventArgs.Empty);

                break;

            case ETableElementScene.ResultScene:

                OnResult?.Invoke(this , IUIManagerEvents.ResultEventArgs.Empty);

                break;

            case ETableElementScene.WinnerScene:

                OnGameEnd?.Invoke(this , new IUIManagerEvents.GameEndEventArgs
                {
                    WinnerEPlayerIdentify = winnerPlayerIdentify
                });

                break;
        }
    }
}
