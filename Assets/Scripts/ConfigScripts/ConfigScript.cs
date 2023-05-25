using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Config", menuName = "ConfigScript", order = 0)]
public class ConfigScript : ScriptableObject
{
    public List<string> BestTimePlayerName { get; set; } = new List<string>(5);

    public List<PlayerConfiguration> PlayerConfigurations { get; set; }

    public List<float>[] CheckpointTimes { get; set; } = new List<float>[5];

    public int Level { get; set; }
    
    public int LastLevelIndex { get; set; }

    public int GameMode { get; set; }

    public int AIAmount { get; set; }

    public int PlayerAmount { get; set; }

    public int Difficulty { get; set; }

    public bool ShowFPS { get; set; }

    public int FPSIndex { get; set; }
}
