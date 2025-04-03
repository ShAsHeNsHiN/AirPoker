using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour , IHasProgress
{
    public event EventHandler<IHasProgress.OnPrgressChangedEventArgs> OnProgressChanged;

    // *這邊只是要取得原始血量，因此 Instance 拿誰都一樣
    private IPlayer GetPlayer() => Player.Instance;

    public void UpdatePlayerBlood(int playerBlood)
    {
        OnProgressChanged?.Invoke(this , new IHasProgress.OnPrgressChangedEventArgs
        {
            progressNormalized = playerBlood / (float)GetPlayer().OriginalBlood
        });
    }
}
