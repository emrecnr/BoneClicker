using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PawModeUI))]
public class PawModeManager : MonoBehaviour
{
    [Header(" Elements ")]
    private PawModeUI _pawModeUI;

    private float _pawModeTimer = 10f; // 5 min
    private float _pawModeTime = 10f; // 10s

    private float _time;

    [Header(" Actions ")]
    public static Action OnPawModeStarted;
    public static Action OnPawModeStopped;

    private void Awake()
    {
        _pawModeUI = GetComponent<PawModeUI>();    
    }

    private void Start()
    {
        StartCoroutine(StartPawModeTimer());
    }

    private IEnumerator StartPawModeTimer()
    {
        float elapsedTime = 0;
        _pawModeUI.InitializeSlider(_pawModeTimer,0f);

        while (elapsedTime < _pawModeTimer)
        {
            // Update slider value:
            _pawModeUI.IncreaseSlider();

            // Waiting time
            yield return new WaitForSeconds(1f);

            elapsedTime++;
        }

        StartPawMode();
    }

    private IEnumerator StopPawModeTime()
    {
        float elapsedTime = 0;
        _pawModeUI.InitializeSlider(_pawModeTime,_pawModeTime);

        while (elapsedTime < _pawModeTime)
        {
            _pawModeUI.DecreaseSlider();

            yield return new WaitForSeconds(1f);

            elapsedTime ++;
        }
        StopPawMode();
    }

    private void StartPawMode()
    {
        Debug.Log("Paw Mode Started!");
        OnPawModeStarted?.Invoke();
        StartCoroutine(StopPawModeTime());        
    }

    private void StopPawMode()
    {
        OnPawModeStopped?.Invoke();
        Debug.Log("Paw Mode Stopped!");
        StartCoroutine(StartPawModeTimer());
    }   
}

