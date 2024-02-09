using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Scriptable Objects",menuName = "Create New Upgrades/Upgrade Data", order = 0)]
public class UpgradeSO : ScriptableObject
{
    [Header(" General ")]
    public Sprite Icon;
    public string title;

    [Header(" Settings ")]
    public double cpsPerLevel;
    public double basePrice;
    public float coefficient;

    public double GetPrice(double upgradeLevel)
    {
        return basePrice * Mathf.Pow(coefficient,(float)upgradeLevel);
    }
}
