using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoEarningsManager : MonoBehaviour
{
    public static AutoEarningsManager Instance {get; private set;}

    [Header(" Settings ")]
    [Tooltip("Value in hertz")]
    [SerializeField] private int addBonesFrequency;

    [Header(" Actions ")]
    public static Action<int> OnAutoEarningLevelIncreased;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
    }

    private void Start() 
    {
        GameUIManager.Instance.UpdatePerSecondText(GetBonesPerSecond());
        InvokeRepeating("AddBones",1,1f / addBonesFrequency);
    }

    private void OnEnable() {
        UpgradeManager.OnUpgradePurchased += OnUpgradePurchasedHandler;
    }

    private void OnDisable()
    {
        UpgradeManager.OnUpgradePurchased -= OnUpgradePurchasedHandler;
    }

    private void OnUpgradePurchasedHandler()
    {
        GameUIManager.Instance.UpdatePerSecondText(GetBonesPerSecond());
        DataManager.Instance.IncreaseCps((float)GetBonesPerSecond());
    }

    private void AddBones()
    {
        double totalBones = GetBonesPerSecond();

        // At this point we have the amount of bones we need to add every second
        BoneManager.Instance.AddBones(totalBones / addBonesFrequency);
    }

    public void IncreaseUpgradeLevel()
    {
        int autoEarningLevel = (int)DataManager.Instance.AutoEarningLevel++;
        OnAutoEarningLevelIncreased?.Invoke(autoEarningLevel);

        DataManager.Instance.SaveData();
    }

    public double GetBonesPerSecond()
    {
        UpgradeSO[] upgrades = UpgradeManager.Instance.GetUpgrades();

        if (upgrades.Length <= 0) return 0;

        double totalBones = 0;

        for (int i = 0; i < upgrades.Length; i++)
        {
            // Grab the amount of bones for the upgrade
            double upgradeBones = upgrades[i].cpsPerLevel * UpgradeManager.Instance.GetButtonUpgradeLevel(i);

            totalBones += upgradeBones;

            Debug.Log(totalBones);
        }
        return totalBones;
    }
}
