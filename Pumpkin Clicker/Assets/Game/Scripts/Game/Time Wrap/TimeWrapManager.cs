using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeWrapManager : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private RectTransform timeWrapPanel;
    [SerializeField] private TimeWrapContainer timeWrapContainer;

    [Header(" Settings ")]
    private Vector2 _panelOpenedPosition;
    private Vector2 _panelClosedPosition;

    private float _timeToForward = 86400f;

    [Header(" Actions ")]
    public static Action OnPanelOpened;



    private void Start()
    {
        _panelOpenedPosition = Vector2.zero;
        _panelClosedPosition = new Vector2(0, timeWrapPanel.rect.height);

        timeWrapPanel.anchoredPosition = _panelClosedPosition;
   }

    public void OpenPanel()
    {
        timeWrapContainer.Configure(CalculateOneDay());

        LeanTween.cancel(timeWrapPanel);
        OnPanelOpened?.Invoke();
        LeanTween.move(timeWrapPanel, _panelOpenedPosition, .3f).setEase(LeanTweenType.easeInOutSine);
    }

    public void ClosePanel()
    {
        LeanTween.cancel(timeWrapPanel);
        OnPanelOpened?.Invoke();
        LeanTween.move(timeWrapPanel, _panelClosedPosition, .2f).setEase(LeanTweenType.easeInOutSine);
    }

    private double CalculateOneDay()
    {
        float currentCps = DataManager.Instance.CurrentCps;

        float amount = currentCps * _timeToForward;

        return amount;
    }


}
