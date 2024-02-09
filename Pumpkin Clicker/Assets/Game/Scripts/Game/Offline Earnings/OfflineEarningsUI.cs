
using UnityEngine;

public class OfflineEarningsUI : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private OfflineEarningsPopup popup;

    public void DisplayPopup(double earnings)
    {
        popup.ConfigureEarningText(DoubleUtilities.ToIdleNotation(earnings));
        popup.GetClaimButton().onClick.AddListener(()=> ClaimButtonClickedCallback(earnings));

        popup.gameObject.SetActive(true);
    }

    private void ClaimButtonClickedCallback(double earnings)
    {
        Debug.Log("Give the player " + earnings + " bones");
        BoneManager.Instance.AddBones(earnings);

        popup.gameObject.SetActive(false);
    }
}
