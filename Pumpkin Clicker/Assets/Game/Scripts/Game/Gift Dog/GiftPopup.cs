using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GiftPopup : MonoBehaviour
{
    [SerializeField] private TMP_Text giftAmount;
    [SerializeField] private Button watchGetButton;

    private bool isGiftEarned;

    private double _giftAmount;

    public void Configure(double giftAmount)
    {
        _giftAmount = giftAmount;
        this.giftAmount.text = DoubleUtilities.ToIdleNotation((double)_giftAmount);
        watchGetButton.onClick.AddListener(()=>Watch());
    }

    public void Watch()
    {
        // Show Ads
        AdManager.Instance.ShowRewardedAd(()=>
        {
            BoneManager.Instance.AddBones(_giftAmount);
            GiftManager.Instance.ClosePanel();
        });
    }
}
