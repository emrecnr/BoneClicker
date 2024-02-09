using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderboardContainer : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private TMP_Text NicknameText;
    [SerializeField] private TMP_Text TotalBones;
    [SerializeField] private TMP_Text TotalCps;

    public void Configure(string nickname, string totalBones, string totalCps)
    {
        NicknameText.text = nickname;
        TotalBones.text = totalBones;
        TotalCps.text = totalCps;
    }
}
