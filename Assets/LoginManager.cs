using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    [SerializeField] private ConfigAPI configAPI;
    [SerializeField] private InputField codeInput;
    [SerializeField] private GameObject playerPanel;
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private Text playerNameText;

    private bool _isConnected;
    
    public void Login()
    {
        StartCoroutine(LoginHandler());
    }

    private IEnumerator LoginHandler()
    {
        if (_isConnected == false)
        {
            string code = codeInput.text;
            if (string.IsNullOrEmpty(code))
            {
                Debug.LogError("Code input is empty");
            }
            else
            {
                codeInput.text = "";
                configAPI.IdCode = code;
                _isConnected = true;

                yield return StartCoroutine(configAPI.GetAuth("user"));
                playerNameText.text = configAPI.UserData.username;
                playerPanel.SetActive(true);
                loginPanel.SetActive(false);
            }   
        }
    }

    public void Logout()
    {
        if (_isConnected == true)
        {
            configAPI.ClearData();
            configAPI.IdCode = null;
            _isConnected = false;
            playerPanel.SetActive(false);
            loginPanel.SetActive(true);
        }
    }
}
