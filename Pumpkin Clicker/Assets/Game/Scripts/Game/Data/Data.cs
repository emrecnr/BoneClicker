using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Firestore;
using UnityEngine;

public class Data 
{
    private string userNickname; // Kullanici nick
    private double _currentBones = 0; // Mevcut Kemik miktari
    private double _totalBones = 0; // Tüm zamanlar Pizza
    private double _totalBoneEarnedByClicking = 0; // Tiklanarak kazanilan toplam kemik
    private double _totalClicks = 0; // Toplam tiklama sayisi
    private double _totalGoldBoneEarned = 0; // Toplam kazanilan altin kemik
    private double _totalUpgrades = 0;// Toplam gelistirme sayisi
    private double _autoEarningLevel = 0;
    private float _currentCps = 0f; // Mevcut cps
    private float _totalCps = 0f; // Toplam cps
    private string _lastDateTime = DateTime.Now.ToString();
    private double _removeAds = 0;
    private double _totalTimeWrap = 0;
    private Dictionary<string,double> _upgradeButtonDictionary = new Dictionary<string, double>();

    public string Nickname { get { return userNickname; } set { userNickname = value; } }
    public double CurrentBones { get { return _currentBones; } set { _currentBones = value; } }
    public double TotalBones { get { return _totalBones; } set { _totalBones = value; } }
    public double TotalBoneEarnedByClicking { get { return _totalBoneEarnedByClicking; } set { _totalBoneEarnedByClicking = value; } }
    public double TotalClicks { get { return _totalClicks; } set { _totalClicks = value; } }
    public double TotalGoldBoneEarned { get { return _totalGoldBoneEarned; } set { _totalGoldBoneEarned = value; } }
    public double TotalUpgrades { get { return _totalUpgrades; } set { _totalUpgrades = value; } }
    public double AutoEarningLevel { get { return _autoEarningLevel; } set { _autoEarningLevel = value; } }
    public float CurrentCps { get { return _currentCps; } set { _currentCps = value; } }
    public float TotalCps { get { return _totalCps; } set { _totalCps = value; } }
    public double RemoveAds { get { return _removeAds; } set { _removeAds = value; } }
    public string LastDateTime { get { return _lastDateTime; } set { _lastDateTime = value; } }
    public double TotalTimeWrap { get { return _totalTimeWrap; } set { _totalTimeWrap = value; } }
    public Dictionary<string,double> UpgradeButtonDictionary {get{return _upgradeButtonDictionary;} set{_upgradeButtonDictionary = value;}}



    public Data()
    {
        // Default const required for Firestore serialization
    }

    public Data(DocumentSnapshot snapshot)
    {
        // Constructor to create UserData object from a DocumentSnapshot

        Dictionary<string, object> data = snapshot.ToDictionary();
        Nickname = (string)data["Nickname"];
        CurrentBones = (double)data["CurrentBones"];
        TotalBones = (double)data["TotalBones"];
        TotalBoneEarnedByClicking = (double)data["TotalBoneEarnedByClicking"];
        TotalClicks = (double)data["TotalClicks"];
        TotalGoldBoneEarned = (double)data["TotalGoldBoneEarned"];
        TotalUpgrades = (double)data["TotalUpgrades"];
        CurrentCps = float.Parse(data["CurrentCps"].ToString());
        TotalCps = float.Parse(data["TotalCps"].ToString());
        RemoveAds = (double)data["RemoveAds"];
        AutoEarningLevel = (double)data["AutoEarningLevel"];
        LastDateTime = (string)data["LastDateTime"];
        TotalTimeWrap = (double)data["TotalTimeWrap"];

        Dictionary<string, object> upgradeDictObject = (Dictionary<string, object>)data["UpgradeButtonDictionary"];
        UpgradeButtonDictionary = new Dictionary<string, double>();

        foreach (var entry in upgradeDictObject)
        {
            if (entry.Value is double)
            {
                UpgradeButtonDictionary.Add(entry.Key, (double)entry.Value);
            }
            else
            {
                Debug.LogError("Error Dictionary");
            }
        }
    }

    public Dictionary<string, object> ToDictionary()
    {
        // Convert UserData object to a dictionary for Firestore

        Dictionary<string, object> data = new Dictionary<string, object>();
        data["Nickname"] = userNickname;
        data["CurrentBones"] = CurrentBones;
        data["TotalBones"] = TotalBones;
        data["TotalBoneEarnedByClicking"] = TotalBoneEarnedByClicking;
        data["TotalClicks"] = TotalClicks;
        data["TotalGoldBoneEarned"]= TotalGoldBoneEarned;
        data["TotalUpgrades"] = TotalUpgrades;
        data["CurrentCps"] = CurrentCps;
        data["TotalCps"] = TotalCps;
        data["RemoveAds"] = RemoveAds;
        data["AutoEarningLevel"] = AutoEarningLevel;
        data["LastDateTime"] = LastDateTime;
        data["UpgradeButtonDictionary"] = UpgradeButtonDictionary;
        data["TotalTimeWrap"] = TotalTimeWrap;

        return data;
    }
}
