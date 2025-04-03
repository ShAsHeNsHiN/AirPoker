using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IHasProgress
{
    public event EventHandler<OnPrgressChangedEventArgs> OnProgressChanged;

    public class OnPrgressChangedEventArgs : EventArgs
    {
        public float progressNormalized;
    }
}
