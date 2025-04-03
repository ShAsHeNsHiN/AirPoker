using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static bool IsSavedData;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
