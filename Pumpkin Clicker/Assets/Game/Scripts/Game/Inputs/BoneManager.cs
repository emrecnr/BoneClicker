using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneManager : MonoBehaviour
{
    public static BoneManager Instance { get; private set; }

    [Header(" ## Data ## ")]
    [SerializeField] private double currentBones;
    [SerializeField] private float boneIncrement;

    private int frenzyModeMultiplier = 2;
    private int pawModeMultiplier = 50; 


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
    }

    private void Start()
    {
        boneIncrement = 1;
        LoadData();
    }
    
    private void OnEnable()
    {
        // Click:
        InputManager.OnBoneClicked += BoneClickedHandler;

        // Frenzy mode:
        Bone.OnFrenzyModeStarted += OnFrenzyModeStartedHandler;
        Bone.OnFrenzyModeStopped += OnFrenzyModeStoppedHandler;

        // Paw mode:
        PawModeManager.OnPawModeStarted += OnPawModeStartedHandler;
        PawModeManager.OnPawModeStopped += OnPawModeStoppedHandler;
    }

    private void OnDisable()
    {
        InputManager.OnBoneClicked -= BoneClickedHandler;

        Bone.OnFrenzyModeStarted -= OnFrenzyModeStartedHandler;
        Bone.OnFrenzyModeStopped -= OnFrenzyModeStoppedHandler;

        PawModeManager.OnPawModeStarted -= OnPawModeStartedHandler;
        PawModeManager.OnPawModeStopped -= OnPawModeStoppedHandler;
    }

    public void AddBones(double value)
    {
        DataManager.Instance.CurrentBones += value;
        DataManager.Instance.TotalBones += value;

        currentBones += value;
        GameUIManager.Instance.UpdateBonesText(currentBones);
    }

    private void BoneClickedHandler()
    {
        currentBones += boneIncrement;
        DataManager.Instance.CurrentBones += boneIncrement;
        DataManager.Instance.TotalBones += boneIncrement;
        
        GameUIManager.Instance.UpdateBonesText(currentBones);
    }
    #region Frenzy Mode
    private void OnFrenzyModeStartedHandler()
    {
        boneIncrement *= frenzyModeMultiplier;
    }

    private void OnFrenzyModeStoppedHandler()
    {
        boneIncrement /= frenzyModeMultiplier;
    }
    #endregion

    #region  Paw Mode Handlers
    private void OnPawModeStartedHandler()
    {
       boneIncrement *= pawModeMultiplier;
    }

    private void OnPawModeStoppedHandler()
    {
       boneIncrement /= pawModeMultiplier;
    }
    #endregion

    public float GetBoneIncrement()
    {
        return boneIncrement;
    }

    private void LoadData()
    {
        currentBones = DataManager.Instance.CurrentBones;
    }
}
