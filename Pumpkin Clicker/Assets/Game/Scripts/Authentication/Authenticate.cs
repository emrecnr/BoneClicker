using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Authenticate : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private GameObject fieldArea;
    [SerializeField] private TMP_InputField nicknameField;
    [SerializeField] private Button nextButton;

    [SerializeField] private TMP_Text errorMessage;


    private void Start() 
    {
        errorMessage.text = null;
        fieldArea.SetActive(false);
    }

    private void OnEnable() 
    {
        nextButton.onClick.AddListener(()=> LoginButtonClickedCallback());
        FirebaseManager.OnLogged += OnLoggedCallback;
        FirebaseManager.OnFirst += OnFirstCallback;
    }

    private void OnDisable() 
    {
        nextButton.onClick.RemoveAllListeners();
        FirebaseManager.OnLogged -= OnLoggedCallback;
        FirebaseManager.OnFirst -= OnFirstCallback;
    }

    private void LoginButtonClickedCallback()
    {
        nextButton.interactable = false;
        errorMessage.text = null;
        if (string.IsNullOrEmpty(nicknameField.text))
        {
            errorMessage.text = "Username cannot be empty.";
            nextButton.interactable = true;
            return;
        }

        if (nicknameField.text.Length < 3 || nicknameField.text.Length > 16)
        {
            errorMessage.text = "Username must be between 3 and 16 characters long.";
            nextButton.interactable = true;
            return;    
        }

        StartCoroutine(FirebaseManager.Instance.AnonymouseLogin(nicknameField.text,(value)=>{
            errorMessage.text = value;
            nextButton.interactable = true;
        }));
    }

    private void OnLoggedCallback()
    {
        fieldArea.SetActive(false);
    }
    
    private void OnFirstCallback()
    {
        fieldArea.SetActive(true);
    }
}
