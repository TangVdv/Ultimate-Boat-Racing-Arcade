using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Color = System.Drawing.Color;

public class BoatSelection : MonoBehaviour
{
    [SerializeField] private ConfigScript config;
    [SerializeField] private GameObject[] locations;
    [SerializeField] private GameObject[] boats;
    [SerializeField] private Text PlayboatNameText;
    [SerializeField] private Text CustomboatNameText;

    public float travelDuration = 1;

    private BoatManager _boatManager;
    
    private GameObject _currentBoat;
    private int _indexSelection = 0;
    private bool _isMoving = false;

    private void Start()
    {
        if (boats.Length > 0)
        {
            _currentBoat = Instantiate( boats[_indexSelection], locations[1].transform.position, locations[1].transform.rotation);
            PlayboatNameText.text = _currentBoat.name;
            CustomboatNameText.text = _currentBoat.name;
            _boatManager = _currentBoat.GetComponent<BoatManager>();
        }
    }
    private IEnumerator MoveToPosition(int start, int final)
    {
        float elapsedTime = 0f;
        Vector3 startPosition = locations[start].transform.position;
        Quaternion startRotation = locations[start].transform.rotation;
        
        Vector3 finalPosition = locations[final].transform.position;
        Quaternion finalRotation = locations[final].transform.rotation;

        while (elapsedTime < travelDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / travelDuration);
            _currentBoat.transform.position = Vector3.Lerp(startPosition, finalPosition, t);
            _currentBoat.transform.rotation = Quaternion.Lerp(startRotation, finalRotation, t);
            yield return null;
        }
        
        _currentBoat.transform.position = finalPosition;
        _currentBoat.transform.rotation = finalRotation;
    }

    public IEnumerator SwitchBoat(int start, int final)
    {
        StartCoroutine(MoveToPosition(1, final));
        yield return StartCoroutine(MoveToPosition(1, final));
        Destroy(_currentBoat);
        _currentBoat = Instantiate(boats[_indexSelection], locations[start].transform.position, locations[start].transform.rotation);
        Debug.Log("Current Boat : "+boats[_indexSelection]);
        _boatManager = _currentBoat.GetComponent<BoatManager>();
        PlayboatNameText.text = _currentBoat.name;
        CustomboatNameText.text = _currentBoat.name;
        StartCoroutine(MoveToPosition(start, 1));
        _isMoving = false;
    }

    public void LeftSelection()
    {
        if (_indexSelection > 0 && _isMoving == false)
        {
            _isMoving = true;
            _indexSelection = Mathf.Clamp(_indexSelection - 1, 0, boats.Length - 1);
            StartCoroutine(SwitchBoat(2, 0));   
        }
    }

    public void RightSelection()
    {
        if (_indexSelection < boats.Length - 1 && _isMoving == false)
        {
            _isMoving = true;
            _indexSelection = Mathf.Clamp(_indexSelection + 1, 0, boats.Length - 1);
            StartCoroutine(SwitchBoat(0, 2));   
        }
    }

    public void Select()
    {
        config.Boat = boats[_indexSelection];
    }

    public void ColorSelect(Button button)
    {
        var color = button.GetComponent<Image>().color;
        _boatManager.CurrentMaterialColor = color;
    }
    
}
