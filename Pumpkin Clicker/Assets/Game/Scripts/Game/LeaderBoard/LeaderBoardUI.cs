using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderBoardUI : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private RectTransform leaderBoard;

    [Header(" Settings ")]
    private Vector2 _panelOpenedPosition;
    private Vector2 _panelClosedPosition;

    [Header(" Actions ")]
    public static Action OnPanelOpened;

    private void Start()
    {
        _panelOpenedPosition = Vector2.zero;
        _panelClosedPosition = new Vector2(0, leaderBoard.rect.height);

        leaderBoard.anchoredPosition = _panelClosedPosition;
    }


    public void OpenPanel()
    {
        LeanTween.cancel(leaderBoard);
        OnPanelOpened?.Invoke();
        LeanTween.move(leaderBoard, _panelOpenedPosition, .3f).setEase(LeanTweenType.easeInOutSine);
    }

    public void ClosePanel()
    {
        LeanTween.cancel(leaderBoard);
        OnPanelOpened?.Invoke();
        LeanTween.move(leaderBoard, _panelClosedPosition, .2f).setEase(LeanTweenType.easeInOutSine);
    }
}
