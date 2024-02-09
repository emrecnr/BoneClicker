using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Clicker.Authentication
{
    public class LoginUIManager : MonoBehaviour
    {
        [Header(" Login Elements ")]
        [SerializeField] private GameObject loginPanel;

        [SerializeField] private TMP_InputField emailField;
        [SerializeField] private TMP_Text emailErrorCaption;

        [SerializeField] private TMP_InputField passwordField;
        [SerializeField] private TMP_Text passwordErrorCaption;

        [SerializeField] private TMP_Text errorCaption;


        [SerializeField] private Button loginButton;
        [SerializeField] private Button createAnAccountButton;

        [Header(" Actions ")]
        public static Action OnCreateAnAccountButtonClicked;

        private void Awake()
        {
            InitializeUIElements();
        }

        private void OnEnable()
        {
            RegisterUIManager.OnAllreadyHaveAccountButtonClicked += OnAllreadyHaveAccountButtonClickedHandler;
        }

        private void OnDisable()
        {
            RegisterUIManager.OnAllreadyHaveAccountButtonClicked -= OnAllreadyHaveAccountButtonClickedHandler;
        }

        private void InitializeUIElements()
        {
            loginButton.onClick.RemoveAllListeners();
            createAnAccountButton.onClick.RemoveAllListeners();

            emailField.text = null;
            passwordField.text = null;

            emailErrorCaption.text = null;
            passwordErrorCaption.text = null;
            errorCaption.text = null;

            loginButton.onClick.AddListener(() => LoginButtonClickedCallback());
            createAnAccountButton.onClick.AddListener(() =>
            {
                loginPanel.SetActive(false);
                OnCreateAnAccountButtonClicked?.Invoke();
            });
        }

        private void LoginButtonClickedCallback()
        {
            Debug.Log("Login Button");
            if (!EmailCheck(emailField))
                return;
            if (!PasswordCheck(passwordField))
                return;

            StartCoroutine(FirebaseManager.Instance.LoginAsync(emailField.text, passwordField.text, (message) =>
            {
                ErrorMessage(message);
            }));
        }

        private void OnAllreadyHaveAccountButtonClickedHandler()
        {
            InitializeUIElements();
            loginPanel.SetActive(true);
        }

        private bool EmailCheck(TMP_InputField emailField)
        {
            if (string.IsNullOrEmpty(emailField.text))
            {
                emailErrorCaption.text = "Email field cannot be left empty.";
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

        private void ErrorMessage(string message)
        {
            errorCaption.text = message;
        }
    }
}
