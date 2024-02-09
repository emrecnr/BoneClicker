using System;
using System.Collections;
using System.Collections.Generic;
using Clicker.Authentication;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RegisterUIManager : MonoBehaviour
{
    [Header(" Register Elements ")]
    [SerializeField] private GameObject registerPanel;

    [SerializeField] private TMP_InputField nicknameField;
    [SerializeField] private TMP_Text nicknameErrorCaption;

    [SerializeField] private TMP_InputField emailField;
    [SerializeField] private TMP_Text emailErrorCaption;

    [SerializeField] private TMP_InputField passwordField;
    [SerializeField] private TMP_Text passwordErrorCaption;

    [SerializeField] private TMP_InputField confirmPasswordField;
    [SerializeField] private TMP_Text confirmPasswordErrorCaption;

    [SerializeField] private TMP_Text errorMessage;

    [SerializeField] private Button registerButton;
    [SerializeField] private Button allreadyHaveAccountButton;

    [Header(" Actions ")]
    public static Action OnAllreadyHaveAccountButtonClicked;

    private void Start() 
    {
        InitializeUIElements();
    }

    private void OnEnable() 
    {
        LoginUIManager.OnCreateAnAccountButtonClicked += OnCreateAnAccountButtonClickedHandler;
    }

    private void OnDisable()
    {
        LoginUIManager.OnCreateAnAccountButtonClicked -= OnCreateAnAccountButtonClickedHandler;
    }

    private void InitializeUIElements()
    {
        allreadyHaveAccountButton.onClick.RemoveAllListeners();
        registerButton.onClick.RemoveAllListeners();
        
        nicknameField.text = null;
        emailField.text = null;
        passwordField.text = null;
        confirmPasswordField.text = null;

        nicknameErrorCaption.text = null;
        emailErrorCaption.text = null;
        passwordErrorCaption.text = null;
        confirmPasswordErrorCaption.text = null;
        errorMessage.text = null;

        registerButton.onClick.AddListener(()=>RegisterButtonClickedCallback());
        allreadyHaveAccountButton.onClick.AddListener(()=>{
            registerPanel.SetActive(false);
            OnAllreadyHaveAccountButtonClicked?.Invoke();
        });
    }

    private void RegisterButtonClickedCallback()
    {
        Debug.Log("Register Button");

        if(!NicknameCheck(nicknameField))
            return;
        
        if (!EmailCheck(emailField))        
            return;
        
        if(!PasswordCheck(passwordField))
            return;

        if(!ConfirmPasswordCheck(confirmPasswordField))
            return;

        StartCoroutine(FirebaseManager.Instance.RegisterAsync(emailField.text,passwordField.text,nicknameField.text,
        ()=>
        {
            // OnRegister;
            Debug.Log("Registration successfull");

        }, (message)=>
        {
            ErrorMessage(message);
        }));    
    }

    private void OnCreateAnAccountButtonClickedHandler()
    {
        InitializeUIElements();
        registerPanel.SetActive(true);
    }
    
    private bool NicknameCheck(TMP_InputField nicknameField)
    {
        if (string.IsNullOrEmpty(nicknameField.text))
        {
            nicknameErrorCaption.text = "Nickname field cannot be left empty.";
            return false;
        }

        if (nicknameField.text.Length < 8 || nicknameField.text.Length > 16)
        {
            nicknameErrorCaption.text = "Nickname must be at least 8 - 16 characters long.";
            return false;
        }

        return true;
    }

    private bool EmailCheck(TMP_InputField emailField)
    {
        if (string.IsNullOrEmpty(emailField.text))
        {
            emailField.text = "Email field cannot be left empty.";
            return false;
        }
        return true;
    }

    private bool PasswordCheck(TMP_InputField passwordField)
    {
        if (string.IsNullOrEmpty(passwordField.text))
        {
            passwordErrorCaption.text = "Password field cannot be left empty.";
            return false;
        }

        if (passwordField.text.Length < 8)
        {
            passwordErrorCaption.text = "Password must be at least 8 characters long.";
            return false;
        }

        return true;
    }

    private bool ConfirmPasswordCheck(TMP_InputField confirmPasswordField)
    {
        if (string.IsNullOrEmpty(confirmPasswordField.text))
        {
            confirmPasswordErrorCaption.text = "Confirm Password field cannot be left empty.";
            return false;
        }

        if(confirmPasswordField.text != passwordField.text)
        {
            confirmPasswordErrorCaption.text = "Passwords do not match.";
            return false;
        }
        return true;
    }

    private void ErrorMessage(string message)
    {
        errorMessage.text = message;
    }
}
