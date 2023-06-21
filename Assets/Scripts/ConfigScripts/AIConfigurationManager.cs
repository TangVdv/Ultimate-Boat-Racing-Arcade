using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIConfigurationManager : MonoBehaviour
{
    [SerializeField] private ConfigScript config;
    public bool debug;
    private List<AIConfiguration> _AIConfigs;
    public List<AIConfiguration> AIConfigs => _AIConfigs;
    
    private AIConfigurationManager()
    {
        _AIConfigs = new List<AIConfiguration>();
    }

    public void SetupAI(int index, Material boatMat, Material cannonMat, GameObject boat, GameObject cannon)
    {
        _AIConfigs[index].AIBoatMaterial = boatMat;
        _AIConfigs[index].AICannonMaterial = cannonMat;
        _AIConfigs[index].AIBoat = boat;
        _AIConfigs[index].AICannon = cannon;
        _AIConfigs[index].Name = "AI " + (index+1);
    }

    public void HandleAIJoin(int index)
    {
        if (_AIConfigs.Count < config.AIAmount)
        {
            if(debug)Debug.Log("AI Joined "+index);

            if (_AIConfigs.All(p => p.AIIndex != index))
            {
                _AIConfigs.Add(new AIConfiguration(index));
            }   
        }
        else
        {
            if(debug)Debug.Log("AI capacity exceeded");
        }
    }
}

public class AIConfiguration
{
    public AIConfiguration(int index)
    {
        AIIndex = index;
    }
    
    public int AIIndex { get; set; }
    public string Name { get; set; }

    public Material AIBoatMaterial { get; set; }
    
    public Material AICannonMaterial { get; set; }
    
    public GameObject AIBoat { get; set; }
    
    public GameObject AICannon { get; set; }
}
