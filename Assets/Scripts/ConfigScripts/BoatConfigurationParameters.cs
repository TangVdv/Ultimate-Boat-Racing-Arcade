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

    public int Mass => mass;
    public float Drag => drag;
    public float AngularDrag => angularDrag;
    public int ForwardAcceleration => forwardAcceleration;
    public int BackwardAcceleration => backwardAcceleration;
    public float SlowModifier => slowModifier;
    public float FastModifier => fastModifier;
    public int RotationSpeed => rotationSpeed;
    public Transform[] CannonPos => cannonPosition;
    public NewFloatersManager NewFloatersManager => newFloatersManager;

}
