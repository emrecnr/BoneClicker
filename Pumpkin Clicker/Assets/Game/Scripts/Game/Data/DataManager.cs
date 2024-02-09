using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance {get; private set;}

    #region Properties
    public string UserID {get; private set;}

    public string Nickname {get; private set;}
    public double CurrentBones {get; set;}
    public double TotalBones { get; set; }
    public double TotalBonesEarnedByClicking { get; set; } 
    public double TotalClicks { get; set; }
    public double TotalGoldBonesEarned { get; set; }
    public double TotalUpgrades { get; set; }
    public double TotalTimeWrap{get; set;}
    public float CurrentCps { get; set; }
    public float TotalCps { get; set; }
    public double RemoveAds { get; set; }
    public double AutoEarningLevel { get; set; }
    public string LastDateTime{get; set;}

    public Dictionary<string,double> UpgradeButtonDictionary {get; set;} = new Dictionary<string,double>();

    public List<Data> LeaderboardUsers  {get; set;} = new List<Data>();
    #endregion

    public bool IsDataLoaded {get; private set;} = false;

    private void Awake()
    {
        SingletonObject();
    }

    private void OnEnable()
    {
        FirebaseManager.OnLogged += LoadData;
    }

    private void OnDisable()
    {
        FirebaseManager.OnLogged -= LoadData;
    }

    private void OnApplicationFocus(bool focusStatus)
    {
        if(focusStatus)
        {
            SaveData();
        }
    }

    private void  OnApplicationQuit() 
    {
        SaveData();
    }

    private void SingletonObject()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(gameObject);
    }

    public void AddBones(double bonesAmount)
    {
        CurrentBones += bonesAmount;
    }

    public void RemoveBones(double bonesAmount)
    {
        CurrentBones -= bonesAmount;
    }

    public void IncreaseCps(float amount)
    {
        CurrentCps = amount;
        TotalCps = amount;

        SaveData();
    }

    public void DecreaseTimeWrap()
    {
        TotalTimeWrap--;
    }

    public void LoadData()
    {
        StartCoroutine(FirebaseManager.Instance.ReadUser((data)=>
        {
            Nickname = data.Nickname;
            CurrentBones = data.CurrentBones;
            TotalBones = data.TotalBones;
            TotalBonesEarnedByClicking = data.TotalBoneEarnedByClicking;
            TotalClicks = data.TotalClicks;
            TotalGoldBonesEarned = data.TotalGoldBoneEarned;
            TotalUpgrades = data.TotalUpgrades;
            CurrentCps = data.CurrentCps;
            TotalCps = data.TotalCps;
            RemoveAds = data.RemoveAds;
            AutoEarningLevel = data.AutoEarningLevel;
            LastDateTime = data.LastDateTime;
            UpgradeButtonDictionary = data.UpgradeButtonDictionary;
            TotalTimeWrap = data.TotalTimeWrap;
            
            LoadLeaderBoard();
        }));
    }

    public void LoadLeaderBoard()
    {
        StartCoroutine(FirebaseManager.Instance.GetAllUsers((users)=>
        {
            foreach (var user in users)
            {
                LeaderboardUsers.Add(user);
            }

            IsDataLoaded = true;
            Debug.Log("LeaderBoard Yükleme başarili");
        }));
    }

    public void SaveData()
    {
        StartCoroutine(FirebaseManager.Instance.CreateUser(new Data
         {
            Nickname = this.Nickname,
            CurrentBones = this.CurrentBones,
            TotalBones = this.TotalBones,
            TotalBoneEarnedByClicking = this.TotalBonesEarnedByClicking,
            TotalClicks = this.TotalClicks,
            TotalGoldBoneEarned = this.TotalGoldBonesEarned,
            TotalUpgrades = this.TotalUpgrades,
            TotalCps = this.TotalCps,
            CurrentCps = this.CurrentCps,
            RemoveAds = this.RemoveAds,
            AutoEarningLevel = this.AutoEarningLevel,
            LastDateTime = DateTime.Now.ToString(),
            UpgradeButtonDictionary = this.UpgradeButtonDictionary,
            TotalTimeWrap = this.TotalTimeWrap
    }));
    }

}
