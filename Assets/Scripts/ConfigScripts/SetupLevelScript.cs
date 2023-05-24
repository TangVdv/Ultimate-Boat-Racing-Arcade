using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupLevelScript : MonoBehaviour
{
    [SerializeField] private ConfigScript config;
    [SerializeField] private TimerScript timerScript;
    public List<GameObject> mapTemplates;

    private SetupGameScript _setupGameScript;
    private GameObject _currentMap;
    void Start()
    {
        SetupLevel();
    }

    private void ClearLevel()
    {
        if (_currentMap != null)
        {
            Destroy(_currentMap);
        }
    }

    public void SetupLevel()
    {
        ClearLevel();
        _currentMap = Instantiate(mapTemplates[config.Level]);
        _setupGameScript = _currentMap.GetComponent<SetupGameScript>();
        _setupGameScript.SetupGame();
        timerScript.ResetTimer();
        timerScript.StartTimer();
    }

    public void ResetCurrentMap()
    {
        _setupGameScript.Reset();
        timerScript.ResetTimer();
        timerScript.StartTimer();
    }
}
