using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    [SerializeField] private ConfigAPI configAPI;
    [SerializeField] private ConfigScript config; 
    [SerializeField] private InputField codeInput;
    [SerializeField] private GameObject playerPanel;
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private Text playerNameText;

    private void Start()
    {
        if (configAPI.IsConnected == true)
        {
            ConnectionHandler();
        }
    }

    public void Login()
    {
        StartCoroutine(LoginHandler());
    }

    private IEnumerator LoginHandler()
    {
        if (configAPI.IsConnected == false)
        {
            string idCode = codeInput.text;
            if (string.IsNullOrEmpty(idCode))
            {
                Debug.LogError("Code input is empty");
                yield return null;
            }
            else
            {
                var coroutine =  StartCoroutine(configAPI.GetAuth(idCode, "user"));
                if (coroutine != null) yield return null;
                yield return coroutine;
                configAPI.UserData.points += 100;
                StartCoroutine(configAPI.PostAuth());
                ConnectionHandler();
            }   
        }
    }

    private void ConnectionHandler()
    {
        codeInput.text = "";
        configAPI.IsConnected = true;
                
        playerNameText.text = configAPI.UserData.username;
        playerPanel.SetActive(true);
        loginPanel.SetActive(false);

        StartCoroutine(GetUserData());
    }

    private IEnumerator GetUserData()
    {
        yield return StartCoroutine(configAPI.GetAuth(configAPI.UserData.id_code,"skins"));
        config.ColorIdentifierByBoat = new Dictionary<string, string>();
        foreach (var skin in configAPI.SkinsArray.skins)
        {
            config.ColorIdentifierByBoat[skin.identifier] = skin.boat_identifier;
        }
    }

    public void Logout()
    {
        if (configAPI.IsConnected == true)
        {
            configAPI.ClearData();
            configAPI.IsConnected = false;
            playerPanel.SetActive(false);
            loginPanel.SetActive(true);
        }
    }
}
