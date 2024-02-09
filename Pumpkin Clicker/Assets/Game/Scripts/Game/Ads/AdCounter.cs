using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AdCounter : MonoBehaviour
{
    private List<Button> allButtons = new List<Button>();
    private bool listenersAdded = false;

    private float timeThreshold = 30f;
    private float currentTime = 0f;

    void Start()
    {
        ConfigureButtons();
    }

    private void Update()
    {
        if (currentTime >= timeThreshold && !listenersAdded)
        {
            listenersAdded = true;
            AddListenersToButtons();
        }
        else
            currentTime += Time.deltaTime;


    }

    private void ConfigureButtons()
    {
        Button[] buttons = FindObjectsOfType<Button>();

        foreach (Button button in buttons)
        {
            allButtons.Add(button);
        }
    }


    private void AddListenersToButtons()
    {
        foreach (Button button in allButtons)
        {
            button.onClick.AddListener(() => OnButtonClick());
        }
    }

    void OnButtonClick()
    {
        currentTime = 0;
        foreach (Button button in allButtons)
        {
            button.onClick.RemoveAllListeners();
        }
        listenersAdded = false;

        AdManager.Instance.ShowInterstitialAd();

        
    }
}
