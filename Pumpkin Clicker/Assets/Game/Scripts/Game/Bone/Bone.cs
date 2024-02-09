using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;
using UnityEditor.Rendering;

public class Bone : MonoBehaviour
{
    [Header(" ## Elements ## ")]
    [SerializeField] Transform boneRendererTransform;
    [SerializeField] private Image fillImage;

    [Header(" ## Settings ## ")]
    [SerializeField] private float fillRate; // 0.04 default
    private bool isFrenzyModeActive = false;

    private LTDescr _fillTween;

    [Header(" ## Actions ## ")]
    public static Action OnFrenzyModeStarted;
    public static Action OnFrenzyModeStopped;

    private void OnEnable() 
    {
        InputManager.OnBoneClicked += BoneClickedHandler;

        PawModeManager.OnPawModeStarted += OnPawModeStartedHandler;
    }

    private void OnDisable()
    {
        InputManager.OnBoneClicked -= BoneClickedHandler;

        PawModeManager.OnPawModeStarted -= OnPawModeStartedHandler;
    }


    private void OnPawModeStartedHandler()
    {
        LeanTween.rotateAround(gameObject, Vector3.forward,360f, 10f);
    }

    private void BoneClickedHandler()
    {
        BoneAnimate();

        if(!isFrenzyModeActive)
                FillAnimate();
    }

    
    private void FillAnimate()
    {
        if (_fillTween != null)
        {
            return;
        }

        float targetAmount = fillImage.fillAmount += fillRate;

       _fillTween = LeanTween.value(gameObject, fillImage.fillAmount, targetAmount, .25f)
            .setEase(LeanTweenType.clamp).
            setOnUpdate((float val) =>
            {
                fillImage.fillAmount = val;                
            }).
            setOnComplete(()=>{
                _fillTween = null;

                if(fillImage.fillAmount >= 1)
                {
                    fillImage.fillAmount = 1;
                    StartFrenzyMode();
                }
            });
    }

    private void BoneAnimate()
    {
        boneRendererTransform.localScale = Vector3.one * .8f;
        LeanTween.cancel(boneRendererTransform.gameObject);
        LeanTween.scale(boneRendererTransform.gameObject, Vector3.one * 0.7f, .15f).setLoopPingPong(1);
    }

    private void StartFrenzyMode()
    {
        isFrenzyModeActive = true;
        _fillTween = LeanTween.value(1,0,10).setOnUpdate((value) => fillImage.fillAmount = value).
        setOnComplete(()=>{
            StopFrenzyMode();
            fillImage.fillAmount = 0;
            _fillTween = null;
        });
        OnFrenzyModeStarted?.Invoke();
    }


    private void StopFrenzyMode()
    {
        isFrenzyModeActive = false;
        OnFrenzyModeStopped?.Invoke();
    }
}

