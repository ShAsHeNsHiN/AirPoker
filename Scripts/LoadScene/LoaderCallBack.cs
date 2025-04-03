using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaderCallBack : MonoBehaviour
{
    private float _transferTimer = 1;

    private void Update()
    {
        _transferTimer -= Time.deltaTime;

        if(_transferTimer <= 0f)
        {
            Loader.LoaderCallBack();
        }
    }
}
