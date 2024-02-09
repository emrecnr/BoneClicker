using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private RectTransform shopPanel;

    [Header(" Settings ")]
    private Vector2 _panelOpenedPosition;
    private Vector2 _panelClosedPosition;

    [Header(" Actions ")]
    public static Action OnPanelOpened;

    private void Start()
    {
        _panelOpenedPosition = Vector2.zero;
        _panelClosedPosition = new Vector2(0,-shopPanel.rect.height);

        shopPanel.anchoredPosition = _panelClosedPosition;
    }

    public void OpenPanel()
    {
        LeanTween.cancel(shopPanel);
        OnPanelOpened?.Invoke();
        LeanTween.move(shopPanel,_panelOpenedPosition,.3f).setEase(LeanTweenType.easeInOutSine);
    }

    public void ClosePanel()
    {
        LeanTween.cancel(shopPanel);
        OnPanelOpened?.Invoke();
        LeanTween.move(shopPanel, _panelClosedPosition, .2f).setEase(LeanTweenType.easeInOutSine);
    }

}
