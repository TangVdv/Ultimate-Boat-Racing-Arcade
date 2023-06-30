using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "Config", menuName = "ConfigScript", order = 0)]
public class ConfigScript : ScriptableObject
{
    public List<string> BestTimePlayerName { get; set; } = new List<string>(5);

    public List<PlayerConfiguration> PlayerConfigurations { get; set; }
    
    //Dictionary<color_identifier, color>
    private Dictionary<string, Color> _colorsByIdentifier = new Dictionary<string, Color>()
    {
        { "fqisjb51n4", new Color(1f, .25f, .25f, 1f) },
        { "dsgjoh231c", new Color(.25f, .5f, 1f, 1f) }
    };

    public Dictionary<string, Color> ColorsByIdentifier => _colorsByIdentifier;

    //Dictionary<color_identifier, boat_identifier>
    public Dictionary<string, string> ColorIdentifierByBoat { get; set; }

    
    private List<Color> _colors = new List<Color>()
    {
        new Color(1f, .25f, .25f, 1f),
        new Color(.25f, .5f, 1f, 1f),
        new Color(.6f, .8f, .4f, 1f),
        new Color(1f, 1f, .25f, 1f),
        new Color(.25f, .14f, .18f, 1f),
        new Color(.9f, .9f, .9f, 1f),
        new Color(.13f, .13f, .13f, 1f)
    };

    public List<Color> Colors => _colors;
    public GameObject[] BoatTemplates { get; set; }
    
    public GameObject[] CannonTemplates { get; set; }
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
