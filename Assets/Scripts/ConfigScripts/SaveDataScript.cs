using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SaveData", menuName = "SaveDataConfig", order = 0)]
public class SaveDataScript : ScriptableObject
{
    [SerializeField] private ConfigScript config;
    
    public void LoadData()
    {
        Debug.Log("Load data");
        int i = 0;
        while (PlayerPrefs.HasKey(i + "_0"))
        {
            List<float> mapTimes = new List<float>();

            int j = 0;
            while (PlayerPrefs.HasKey(i + "_" + j))
            {
                float checkpointTime = PlayerPrefs.GetFloat(i + "_" + j);
                mapTimes.Add(checkpointTime);
                j++;
            }

            config.CheckpointTimes[i] = mapTimes;
            i++;
        }
    }

    public void SaveAllData()
    {
        Debug.Log("Save all data");
        int i = 0;
        foreach (var mapTime in config.CheckpointTimes)
        {
            int j = 0;
            foreach (var checkpointTime in mapTime)
            {
                PlayerPrefs.SetFloat(i+"_"+j, checkpointTime);
            }   
        }
    }

    public void SaveMapData(int mapId)
    {
        Debug.Log("Save map"+mapId+" data");
        int j = 0;
        foreach (var checkpointTime in config.CheckpointTimes[mapId])
        {
            PlayerPrefs.SetFloat(mapId+"_"+j, checkpointTime);
        } 
    }
}
