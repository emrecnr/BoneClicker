using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OfflineEarningsPopup : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private TMP_Text earningText;
    [SerializeField] private Button claimButton;


    public void ConfigureEarningText(string earningsString)
    {
        earningText.text = earningsString;
    }

    public Button GetClaimButton()
    {
        return claimButton;
    }
}
