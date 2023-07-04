using System.Collections;
using System.Collections.Generic;
using Boat.New;
using UnityEngine;

public class CannonConfigurationParameters : MonoBehaviour
{
    [SerializeField] private string identifier;

    public string Identifier
    {
        get => identifier; 
        set => identifier = value;
    }

}
