using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(OfflineEarningsUI))]
public class OfflineEarningsManager : MonoBehaviour
{
    [Header(" Elements ")]
    private OfflineEarningsUI _offlineEarningsUI;

    [Header(" Settings ")]
    [SerializeField] private int maxOfflineSeconds;

    private DateTime _lastDateTime;

    public static Action OnPanelOpened;

    private void Start()
    {
        _offlineEarningsUI = GetComponent<OfflineEarningsUI>();

        if (LoadLastDateTime())
            CalculateOfflineSeconds();
        else
            Debug.LogError("Unable to parse the last date time.");
        
    }

    private void CalculateOfflineSeconds()
    {
        TimeSpan timeSpan = DateTime.Now.Subtract(_lastDateTime);
        Debug.Log("You were off for " + timeSpan.TotalSeconds);

        int offlineSeconds = (int)timeSpan.TotalSeconds;
        offlineSeconds = Mathf.Min(offlineSeconds, maxOfflineSeconds);

        CalculateOfflineEarnings(offlineSeconds);
    }

    private void CalculateOfflineEarnings(int offlineSeconds)
    {
        double offlineEarnings = offlineSeconds * AutoEarningsManager.Instance.GetBonesPerSecond();

        if(offlineEarnings <= 0)
            return;

        Debug.Log("You earned " + offlineEarnings + " bones!");

        _offlineEarningsUI.DisplayPopup(offlineEarnings);
        OnPanelOpened?.Invoke();
    }
    
    private bool LoadLastDateTime()
    {
        bool validDateTime = DateTime.TryParse(DataManager.Instance.LastDateTime, out _lastDateTime);

        Debug.Log(_lastDateTime);
        return validDateTime;
    }
}
