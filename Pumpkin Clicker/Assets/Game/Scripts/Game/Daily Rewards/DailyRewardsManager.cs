using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DailyRewardsUI))]
public class DailyRewardsManager : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private DaiyRewardContainer[] dailyRewardContainers;
    private DailyRewardsUI _dailyRewardsUI;

    [Header(" Data ")]
    [SerializeField] private DailyReward[] dailyRewardsData;
    private int _rewardIndex;
    private const string _rewardIndexKey = "RewardIndex";
    private const string _lastClaimKey = "LastClaimDateTime";
    private DateTime _lastClaimDateTime; // DATA


    private void Awake()
    {
        _dailyRewardsUI = GetComponent<DailyRewardsUI>();
        LoadData();
    }

    private void Start()
    {
        ConfigureRewards();
    }

    #region METHODS
    private void ConfigureRewards()
    {
        for (int i = 0; i < dailyRewardContainers.Length; i++)
        {
            Sprite icon = dailyRewardsData[i].Icon;
            string amount = dailyRewardsData[i].Amount.ToString();
            string day = "Day " + (i + 1);

            bool claimed = false;

            if (_rewardIndex > i)
                claimed = true;

            dailyRewardContainers[i].Configure(icon, amount, day, claimed);
        }

        if (!CheckIfAllRewardsHaveBeenClaimed())
            _dailyRewardsUI.OpenPanel();
    }

    public void ClaimButtonCallback()
    {
        DailyReward dailyReward = dailyRewardsData[_rewardIndex];
        RewardPlayer(dailyReward);
        _rewardIndex++;
        SaveData();

        UpdateRewardContainers();

        if (!CheckIfAllRewardsHaveBeenClaimed())
            ResetTimer();
        else
            _dailyRewardsUI.AllRewardsClaimed();

        _dailyRewardsUI.ClosePanel();
    }

    private bool CheckIfAllRewardsHaveBeenClaimed()
    {
        return _rewardIndex > 6 ? true : false;
    }

    private void UpdateRewardContainers()
    {
        for (int i = 0; i < dailyRewardContainers.Length; i++)
        {
            if (_rewardIndex > i)
                dailyRewardContainers[i].Claim();
        }
    }

    private void RewardPlayer(DailyReward dailyReward)
    {
        switch (dailyReward.RewardType)
        {
            case DailyRewardType.Bone:
                RewardBones(dailyReward.Amount);
                break;
            case DailyRewardType.Upgrade:
                //
                break;
        }
    }

    private void RewardBones(double amount)
    {
        BoneManager.Instance.AddBones(amount);
    }

    private void ResetTimer()
    {
        _dailyRewardsUI.ResetTimer();
    }

    private void CheckIfCanClaim()
    {
        TimeSpan timeSpan = DateTime.Now.Subtract(_lastClaimDateTime);
        double elapsedHours = timeSpan.TotalHours;

        if (elapsedHours < 24)
        {
            int seconds = 60 * 60 * 24 - (int)timeSpan.TotalSeconds;
            _dailyRewardsUI.InitializeTimer(seconds);
        }
    }

    private void LoadData()
    {
        _rewardIndex = PlayerPrefs.GetInt(_rewardIndexKey);

        if (LoadLastClaimDateTime())
            CheckIfCanClaim();
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt(_rewardIndexKey, _rewardIndex);

        SaveLastDateTime();
    }

    private bool LoadLastClaimDateTime()
    {
        bool validDateTime = DateTime.TryParse(PlayerPrefs.GetString(_lastClaimKey), out _lastClaimDateTime);

        return validDateTime;
    }

    private void SaveLastDateTime()
    {
        DateTime now = DateTime.Now;
        Debug.Log(now);

        PlayerPrefs.SetString(_lastClaimKey, now.ToString());
    }
    #endregion
}