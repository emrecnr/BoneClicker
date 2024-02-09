using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance {get; private set;}

    [Header(" Elements ")]
    [SerializeField] private UpgradeButton upgradeButton;
    [SerializeField] private Transform upgradeButtonsParent;

    [Header(" Data ")]
    [SerializeField] private UpgradeSO[] upgrades;

    [Header(" Actions ")]
    public static Action OnUpgradePurchased;

    private void Awake() 
    {
        if(Instance == null) 
            Instance = this;
        else
            Destroy(this);
    }
    
    private void Start()
    {
        SpawnButtons();
    }

    private void SpawnButtons()
    {
        for (int i = 0; i < upgrades.Length; i++)
        {
            SpawnButton(i);
        }
    }

    private void SpawnButton(int buttonIndex)
    {
        UpgradeButton upgradeButtonInstance = Instantiate(upgradeButton,upgradeButtonsParent);

        UpgradeSO upgrade = upgrades[buttonIndex];

        double upgradeLevel = GetButtonUpgradeLevel(buttonIndex);

        Sprite icon = upgrade.Icon;
        string title = upgrade.title;
        string subtitle = "lvl" + upgradeLevel + " (+)" + upgrade.cpsPerLevel + " Cps";
        string price = GetUpgradePriceString(buttonIndex);
        upgradeButtonInstance.Configure(icon,title,subtitle,price);

        upgradeButtonInstance.GetButton().onClick.AddListener(()=> UpgradeButtonClickedCallback(buttonIndex));
    }

    private void UpgradeButtonClickedCallback(int buttonIndex)
    {
        if(buttonIndex == 0)
            AutoEarningsManager.Instance.IncreaseUpgradeLevel();
            
        // TODO: para yeterli mi ?
        IncreaseUpgradeLevel(buttonIndex);
    }

    private void IncreaseUpgradeLevel(int buttonIndex)
    {
        double currentButtonUpgradeLevel = GetButtonUpgradeLevel(buttonIndex);
        currentButtonUpgradeLevel++;

        // Save the button upgrade level:
        SaveButtonUpgradeLevel(buttonIndex,currentButtonUpgradeLevel);

        UpdateVisuals(buttonIndex);

        OnUpgradePurchased?.Invoke(); 
    }

    private void UpdateVisuals(int buttonIndex)
    {
        UpgradeButton upgradeButton = upgradeButtonsParent.GetChild(buttonIndex).GetComponent<UpgradeButton>();

        UpgradeSO upgrade = upgrades[buttonIndex];

        double upgradeLevel = GetButtonUpgradeLevel(buttonIndex);
         string subtitle = "lvl" + upgradeLevel + " (+)" + upgrade.cpsPerLevel + " Cps";
        string price = GetUpgradePriceString(buttonIndex);


        upgradeButton.UpdateVisuals(subtitle,price);
    }

    private string GetUpgradePriceString(int buttonIndex)
    {
        return DoubleUtilities.ToIdleNotation(GetUpgradePrice(buttonIndex));
    }

    private double GetUpgradePrice(int buttonIndex)
    {
        return upgrades[buttonIndex].GetPrice(GetButtonUpgradeLevel(buttonIndex));
    }

    public double GetButtonUpgradeLevel(int buttonIndex)
    {
        string key = "Upgrade" + buttonIndex;

        // Anahtar var mı kontrol et
        if (DataManager.Instance.UpgradeButtonDictionary.ContainsKey(key))
        {
            // Anahtar varsa değeri döndür
            return DataManager.Instance.UpgradeButtonDictionary[key];
        }
        else
        {
            // Anahtar bulunamazsa varsayılan bir değer döndür
            return 0; 
        }
    }

    public UpgradeSO[] GetUpgrades()
    {
        return upgrades;
    }

    private void SaveButtonUpgradeLevel(int buttonIndex, double upgradeLevel)
    {
        if (DataManager.Instance.UpgradeButtonDictionary.ContainsKey("Upgrade" + buttonIndex))
        {
            // Eğer zaten böyle bir anahtar varsa, değeri güncelle
            DataManager.Instance.UpgradeButtonDictionary["Upgrade" + buttonIndex] = upgradeLevel;
        }
        else
        {
            // Anahtar yoksa, yeni bir anahtar-değer çifti ekle
            DataManager.Instance.UpgradeButtonDictionary.Add("Upgrade" + buttonIndex, upgradeLevel);
        }

        DataManager.Instance.SaveData();
    }
}
