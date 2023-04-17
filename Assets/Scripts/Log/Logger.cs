using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;

public class Logger : MonoBehaviour
{
    private string _myLog = "------ BEGIN LOG ------";
    private string _filename = "";
    private bool _doShow = false;
    private int kChars = 700;

    private void OnEnable()
    {
        Application.logMessageReceived += Log;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= Log;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            _doShow = !_doShow;
        }
    }
    public void Log(string logString, string stackTrace, LogType type)
    {
        // for onscreen...
        _myLog = _myLog + "\n" + logString;
        if (_myLog.Length > kChars)
        {
            _myLog = _myLog.Substring(_myLog.Length - kChars);
        }
 
        // for the file ...
        if (_filename == "")
        {
            string dir = Application.dataPath + "/Logs";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);   
            }
            string time = DateTime.Now.ToString("yyyyMMdd_HHmmssfff");
            _filename = dir + "/log-" + time + ".txt";
        }

        try
        {
            string localTime = System.DateTime.UtcNow.ToLocalTime().ToString("HH:mm:ss");
            File.AppendAllText(_filename, localTime + " : " + logString + "\n");
        }
        catch
        {
            
        }
    }
 
    private void OnGUI()
    {
        if (!_doShow) { return; }
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity,
            new Vector3(Screen.width / 1200.0f, Screen.height / 800.0f, 1.0f));
        GUI.TextArea(new Rect(10, 10, 540, 370), _myLog);
    }
}