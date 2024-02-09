using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(LeaderBoardUI))]
public class LeaderBoardManager : MonoBehaviour
{
    [Header( "Elements" )]
    [SerializeField] private LeaderboardContainer[] leaderboardContainers;

    [SerializeField] private TMP_Text playerRankText;

    private void Start()
    {
        InitializeLeaderBoard();
    }

    private void OnEnable()
    {
        UpgradeManager.OnUpgradePurchased += OnUpgradePurchasedHandler;
    }

    private void OnDisable()
    {
        UpgradeManager.OnUpgradePurchased += OnUpgradePurchasedHandler;
    }

    private void InitializeLeaderBoard()
    {
        List<Data> leaderboardUsers = DataManager.Instance.LeaderboardUsers.Take(50).ToList();

        Debug.Log(leaderboardUsers.Count);

        if (leaderboardUsers == null)
            return;

        int index = 0;
        foreach (var userData in leaderboardUsers)
        {
            string nickname = userData.Nickname;
            string totalBones = DoubleUtilities.ToIdleNotation(userData.TotalBones);
            string totalCps = DoubleUtilities.ToIdleNotation(userData.TotalCps);


            leaderboardContainers[index].Configure(nickname,totalBones,totalCps);

            index++;
        }

       int playerRank = FindPlayerRank();
       playerRankText.text = "#" + playerRank;
    }

    private int FindPlayerRank()
    {
        List<Data> leaderboardUsers = DataManager.Instance.LeaderboardUsers;

        for (int i = 0; i < leaderboardUsers.Count; i++)
        {
            if(DataManager.Instance.Nickname == leaderboardUsers[i].Nickname)
            {
                return i + 1;
            }
        }

        return -1;
    }

    private void OnUpgradePurchasedHandler()
    {
       // DataManager.Instance.LoadData();
        //InitializeLeaderBoard();
    }
}
