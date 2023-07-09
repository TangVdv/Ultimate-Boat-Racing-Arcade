using System;
using System.Collections;
using System.Collections.Generic;
using Boat.New.Canon;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public ConfigScript config;
    [SerializeField] private Text currentFPSText;
    [SerializeField] private ChronoScript chronoScript;
    [SerializeField] private RaceModeScript raceModeScript;

    [SerializeField] private GameObject hotbarPanel;
    [SerializeField] private GameObject bulletInventoryTemplate;

    public ChronoScript ChronoScript
    {
        get => chronoScript;
    }
    public RaceModeScript RaceModeScript
    {
        get => raceModeScript;
    }

    private int[] _fpsArray = {30, 60, 120};
    private float _timer, _timelapse, _avgFramerate;
    private List<Text> _bulletsPanelText = new List<Text>();
    private List<Image> _bulletsPanelImage = new List<Image>();
    private int _selectedBullet = 0;

    private void Start()
    {
        Application.targetFrameRate = _fpsArray[config.FPSIndex];
    }

    private void Update()
    {
        if (config.ShowFPS)
        {
            // calcul current framerate
            _timelapse = Time.smoothDeltaTime;
            _timer = _timer <= 0 ? 0 : _timer -= _timelapse;
            if (_timer <= 0)
                _avgFramerate = (int)(1f / _timelapse);

            currentFPSText.text = _avgFramerate.ToString();
        }
        else
            currentFPSText.text = "";
    }

    public void HotbarManager(Dictionary<BulletType, int> bullets)
    {
        foreach (KeyValuePair<BulletType, int> bullet in bullets)
        {
            var currentTemplate = Instantiate(bulletInventoryTemplate, hotbarPanel.transform);
            string bulletAmount;
            if (bullet.Value > 999)
            {
                bulletAmount = "∞";
            }
            else
            {
                bulletAmount = bullet.Value.ToString();
            }
            var currentTemplateText = currentTemplate.transform.GetChild(0).GetComponent<Text>();
            currentTemplateText.text = bulletAmount;
            _bulletsPanelText.Add(currentTemplateText);
            _bulletsPanelImage.Add(currentTemplate.GetComponent<Image>());
        }
        BulletSelection(_selectedBullet);
    }

    public void BulletSelection(int bulletId)
    {
        _bulletsPanelImage[_selectedBullet].fillCenter = true;
        _bulletsPanelText[_selectedBullet].color = Color.black;
        _selectedBullet = bulletId;
        _bulletsPanelImage[_selectedBullet].fillCenter = false;
        _bulletsPanelText[_selectedBullet].color = Color.white;
    }

    public void IncreaseBulletAmount(int bullet, int bulletAmount)
    {
        _bulletsPanelText[bullet].text = bulletAmount.ToString();
    }

    public void DecreaseBulletAmount(int bulletAmount)
    {
        if (bulletAmount > 9999)
        {
            _bulletsPanelText[_selectedBullet].text = "∞";
        }
        else
        {
            _bulletsPanelText[_selectedBullet].text = bulletAmount.ToString();   
        }
    }
}
