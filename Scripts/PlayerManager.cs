using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class PlayerManager : MonoBehaviour
{
    private static PlayerManager _instance;
    public static PlayerManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindAnyObjectByType<PlayerManager>();

                if(_instance == null)
                {
                    _instance = ComponentCreator<PlayerManager>.Create();
                }
            }

            return _instance;
        }
    }

    public int PlayersCount => _players.Count;

    [SerializeField] private List<PlayerBase> _players = new();

    public void Handle_AddPlayerToPlayerList(object sender , IPlayer.InstantiateInformationEventArgs instantiateInformationEventArgs)
    {
        _players.Add(instantiateInformationEventArgs.PlayerBase);
    }

    public void Handle_CheckingToMakePoker(object sender , ITotalNumberCard.CheckingEventArgs checkingEventArgs)
    {
        bool everyPlayerReady = true;

        foreach(var item in _players)
        {
            everyPlayerReady = item.ChooseTotalNumberCardFinished;

            // 要是有一人沒選好，這函式就能退出了
            if(!everyPlayerReady)
            {
                return;
            }
        }

        if(everyPlayerReady)
        {
            TableElement.Instance.ChangeScene(ETableElementScene.MakePokerScene);

            UIManager.Instance.SceneChanged(ETableElementScene.MakePokerScene);
        }
    }
}
