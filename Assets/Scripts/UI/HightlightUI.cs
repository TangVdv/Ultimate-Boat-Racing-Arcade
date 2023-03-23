using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HightlightUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private GameObject _currentPanel;
    private GameObject _currentText;
    private Image _image;
    private Text _text;
    private Toggle _toggle;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject.CompareTag("Hightlight"))
        {
            _currentPanel = eventData.pointerCurrentRaycast.gameObject; // Panel GameObject
            _currentText = _currentPanel.transform.GetChild(0).gameObject; // Text GameObject
            if (_currentPanel.transform.childCount > 1)
            {
                var child = _currentPanel.transform.GetChild(1).gameObject;
                if (child.name == "Toggle")
                {
                    _toggle = child.GetComponent<Toggle>();
                    var toggleColors = _toggle.colors;
                    toggleColors.normalColor = Color.black;
                    toggleColors.highlightedColor = Color.black;
                    toggleColors.pressedColor = Color.black;
                    toggleColors.selectedColor = Color.black;
                    _toggle.colors = toggleColors;
                }
            }
            
            _image = _currentPanel.GetComponent<Image>();
            _text = _currentText.GetComponent<Text>();

            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 230f); 
            _text.color = Color.black;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_currentText && _currentPanel)
        {
            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 0f); 
            _text.color = Color.white;   
        }

        if (_toggle)
        {
            var toggleColors = _toggle.colors;
            toggleColors.normalColor = Color.white;
            toggleColors.highlightedColor = Color.white;
            toggleColors.pressedColor = Color.white;
            toggleColors.selectedColor = Color.white;
            _toggle.colors = toggleColors;
        }
    }
}
