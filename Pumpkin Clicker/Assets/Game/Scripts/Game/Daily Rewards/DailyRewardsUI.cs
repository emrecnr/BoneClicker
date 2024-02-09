using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DailyRewardsUI : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private GameObject panel;

    [Header(" Timer Elements ")]
    [SerializeField] private GameObject claimButton;
    [SerializeField] private GameObject timerContainer;
    [SerializeField] private TMP_Text timerText;

    private int timerSeconds;
    
    public void InitializeTimer(int seconds)
    {
        claimButton.SetActive(false);
        timerContainer.SetActive(true);

        timerSeconds = seconds;

        UpdateTimerText();

        InvokeRepeating("UpdateTimer", 0, 1f);
    }

    public void ResetTimer()
    {
        timerSeconds = 60 * 60 * 24; // 1 day(-1 = 23:59:59)
        InitializeTimer(timerSeconds);
    }
    
    private void UpdateTimer()
    {
        timerSeconds--;
        UpdateTimerText();

        if(timerSeconds <= 0)
            StopTimer();
    }

    private void StopTimer()
    {
        CancelInvoke("UpdateTimer");

        claimButton.SetActive(true);
        timerContainer.SetActive(false);
    }

    private void UpdateTimerText()
    {
        timerText.text = TimeSpan.FromSeconds(timerSeconds).ToString();
    }

    public void AllRewardsClaimed()
    {
        claimButton.SetActive(false);
        timerContainer.SetActive(false);
    }

    public void OpenPanel()
    {
        panel.SetActive(true);
    }

    public void ClosePanel()
    {
        panel.SetActive(false);
    }
}
