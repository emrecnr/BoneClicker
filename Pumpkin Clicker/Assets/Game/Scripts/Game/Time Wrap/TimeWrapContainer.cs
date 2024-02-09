using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeWrapContainer : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private TMP_Text amountText;
    [SerializeField] private GameObject disabledContainer;

    [SerializeField] private GameObject _clockObject;
    [SerializeField] private Image bgImage;

    [Header(" BG ")]
    private Color _initialColor;
    private float _tweenDuration = 2.0f;
    [SerializeField] private Color targetColor;

    [Header(" Clock ")]
    private float _rotationDuration = 10f;
    private int _rotationCount = 10;

    private double _calculatedAmount;

    public void Configure(double amount)
    {
        _initialColor = bgImage.color;

        _calculatedAmount = amount;
        amountText.text = DoubleUtilities.ToIdleNotation(_calculatedAmount);

    }

    public void UseButtonClickedCallback()
    {
        if (DataManager.Instance.TotalTimeWrap > 0)
        {
            BackgroundAnimate();
            ClockAnimate();
        }
        else
            Debug.Log("No have bones");

    }

    private void BackgroundAnimate()
    {
        disabledContainer.SetActive(false);
        LeanTween.value(gameObject, _initialColor.a, targetColor.a, _tweenDuration).setOnUpdate((float alpha) =>
        {
            // Her güncellemede Image'in rengini güncelle
            Color newColor = bgImage.color;
            newColor.a = alpha;
            bgImage.color = newColor;
        });
    }

    private void ClockAnimate()
    {
        _clockObject.SetActive(true);

        LeanTween.rotateAround(_clockObject, Vector3.forward, 360.0f * _rotationCount, _rotationDuration)
           .setEase(LeanTweenType.linear).setOnComplete(() =>
           {
               ResetAnimate();
           });
    }

    private void ResetAnimate()
    {
        DataManager.Instance.DecreaseTimeWrap();
        BoneManager.Instance.AddBones(_calculatedAmount);

        LeanTween.value(gameObject, targetColor.a, _initialColor.a, _tweenDuration).setOnUpdate((float alpha) =>
{
    // Her güncellemede Image'in rengini güncelle
    Color newColor = bgImage.color;
    newColor.a = alpha;
    bgImage.color = newColor;
}).setOnComplete(() => disabledContainer.SetActive(true));

        _clockObject.SetActive(false);
    }
}
