using System.Collections;
using System.Collections.Generic;
using Boat.New;
using UnityEngine;

public class BoatConfigurationParameters : MonoBehaviour
{
    [SerializeField] private int mass;
    [SerializeField] private float drag;
    [SerializeField] private float angularDrag;
    [SerializeField] private int forwardAcceleration;
    [SerializeField] private int backwardAcceleration;
    [SerializeField] private float slowModifier;
    [SerializeField] private float fastModifier;
    [SerializeField] private int rotationSpeed;
    [SerializeField] private Transform[] cannonPosition;
    [SerializeField] private NewFloatersManager newFloatersManager;
    [SerializeField] private float boostModifier;
    [SerializeField] private string identifier;

    public float BoostModifier
    {
        get => boostModifier;
        set => boostModifier = value;
    }
    
    public int Mass
    {
        get => mass;
        set => mass = value;
    }

    public float Drag
    {
        get => drag;
        set => drag = value;
    }

    public float AngularDrag
    {
        get => angularDrag;
        set => angularDrag = value;
    }

    public int ForwardAcceleration
    {
        get => forwardAcceleration;
        set => forwardAcceleration = value;
    }

    public int BackwardAcceleration
    {
        get => backwardAcceleration;
        set => backwardAcceleration = value;
    }

    public float SlowModifier
    {
        get => slowModifier;
        set => slowModifier = value;
    }

    public float FastModifier
    {
        get => fastModifier;
        set => fastModifier = value;
    }

    public int RotationSpeed
    {
        get => rotationSpeed;
        set => rotationSpeed = value;
    }

    public Transform[] CannonPos
    {
        get => cannonPosition;
        set => cannonPosition = value;
    }

    public NewFloatersManager NewFloatersManager
    {
        get => newFloatersManager;
        set => newFloatersManager = value;
    }

    public string Identifier
    {
        get => identifier;
        set => identifier = value;
    }
}
