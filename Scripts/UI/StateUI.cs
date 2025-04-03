using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateUI : MonoBehaviour
{
    /// <summary>
    /// 這個變數是讓該物件能有進度條的功能
    /// </summary>
    [SerializeField] private GameObject _hasProgressGameObject;
    
    [SerializeField] private Image _barImage;

    private IHasProgress _hasProgress;

    private Color32 _blueColor = new(111 , 204 , 226 , 255);

    private Color32 _redColor = new(255 , 0 , 15 , 255);

    private void Awake()
    {
        _barImage.fillAmount = 1f;

        _hasProgress = _hasProgressGameObject.GetComponent<IHasProgress>();

        if(_hasProgress == null)
        {
            Debug.LogError("Game Object" + _hasProgressGameObject + "doesn't have a component implements IHasProgress!");
        }

        _hasProgress.OnProgressChanged += HasProgress_OnProgressChanged;
    }

    private void HasProgress_OnProgressChanged(object sender , IHasProgress.OnPrgressChangedEventArgs e)
    {
        _barImage.fillAmount = e.progressNormalized;
    }

    public void BarColorTurnToBlue()
    {
        _barImage.color = _blueColor;
    }

    public void BarColorTurnToRed()
    {
        _barImage.color = _redColor;
    }
}
