using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PawModeUI : MonoBehaviour
{
    [SerializeField] private Slider pawModeSlider;

    public void InitializeSlider(float maxValue, float value)
    {
        pawModeSlider.maxValue = maxValue;
        pawModeSlider.value = value;
    }

    public void IncreaseSlider()
    {
        pawModeSlider.value++;
    }

    public void DecreaseSlider()
    {
        pawModeSlider.value--;
    }
}
