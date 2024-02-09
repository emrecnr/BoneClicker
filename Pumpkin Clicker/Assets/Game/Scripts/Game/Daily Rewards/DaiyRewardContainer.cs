using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DaiyRewardContainer : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text amountText;
    [SerializeField] private TMP_Text dayText;
    [SerializeField] private GameObject claimCheckmark;
 
    public void Configure(Sprite icon, string amount, string day, bool claimed)
    {
        iconImage.sprite = icon;
        amountText.text = amount;
        dayText.text = day;

        if(claimed)
            Claim();
    }

    public void Claim()
    {
        claimCheckmark.SetActive(true);
    }
}
