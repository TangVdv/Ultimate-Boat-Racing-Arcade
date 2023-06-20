using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPrefabManager : MonoBehaviour
{
    [SerializeField] private Text nameText;
    [SerializeField] private Button button;
    [SerializeField] private Text priceText;
    [SerializeField] private GameObject buyButton;

    public bool isLocked;
    public string prefabName;
    public int price;

    private void Start()
    {
        SetButtonState();
        SetNameText();
    }

    public void SetButtonState()
    {
        if (isLocked)
        {
            button.interactable = false;
            buyButton.SetActive(true);
            priceText.text = price.ToString();
        }
        else
        {
            buyButton.SetActive(false);
            button.interactable = true;
        }
    }

    private void SetNameText()
    {
        nameText.text = prefabName;
    }
}
