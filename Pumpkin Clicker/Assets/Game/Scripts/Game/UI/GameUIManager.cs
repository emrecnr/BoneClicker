using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance{get;private set;}

    [Header(" Elements ")]
    [SerializeField] private TMP_Text boneText;
    [SerializeField] private TMP_Text perSecondText;

    private void Awake() 
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    public void UpdateBonesText(double boneCount)
    {
        //boneText.text = boneCount + " Bones!";
        boneText.text = DoubleUtilities.ToIdleNotation(boneCount) + " Bones!";
    }

    public void UpdatePerSecondText(double perSecond)
    {
        perSecondText.text = perSecond.ToString("F2") + " per second";
    }
}
