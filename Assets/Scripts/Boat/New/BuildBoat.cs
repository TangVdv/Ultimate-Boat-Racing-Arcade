using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Boat.New;
using Boat.New.Canon;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildBoat : MonoBehaviour
{
    [SerializeField] private NewBoatMovementManager newBoatMovementManager;
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private NewAimingManager newAimingManager;
    [SerializeField] private PlayerInput playerInput;
    
    private GameObject _boatTemplate;
    private GameObject _cannonTemplate;
    private BoatConfigurationParameters _boatConfigurationParameters;
    private NewFloatersManager _newFloatersManager;

    public void Initiate(PlayerConfiguration playerConfiguration)
    {
        CreateBoat(playerConfiguration.PlayerBoat, playerConfiguration.PlayerBoatMaterial);
        if (_boatConfigurationParameters)
        {
            if (_boatConfigurationParameters.CannonPos.Length > 0)
            {
                foreach (Transform pos in _boatConfigurationParameters.CannonPos)
                {
                    CreateCannon(playerConfiguration.PlayerCannon, pos, playerConfiguration.PlayerCannonMaterial);   
                }
            }   
        }
        ConnectParameters();
    }

    private void CreateBoat(GameObject template, Material color)
    {
        _boatTemplate = Instantiate(template, transform);
        _boatConfigurationParameters = _boatTemplate.GetComponent<BoatConfigurationParameters>();
        ApplyColor(color, _boatTemplate.GetComponent<MeshRenderer>());
    }

    private void CreateCannon(GameObject template, Transform pos, Material color)
    {
        _cannonTemplate = Instantiate(template, pos);
        NewFiringManager newFiringManager = _cannonTemplate.GetComponent<NewFiringManager>();
        newFiringManager.boat = _boatTemplate;
        newFiringManager.boatRigidbody = rigidbody;
        newFiringManager.aimingManager = newAimingManager;
        newAimingManager.canons.Append(newFiringManager);
        ApplyColor(color, _cannonTemplate.GetComponent<MeshRenderer>());
        ApplyMaterialRecursively(_cannonTemplate.transform, color);
    }
    
    private void ApplyMaterialRecursively(Transform parent, Material color)
    {
        foreach (Transform child in parent)
        {
            MeshRenderer renderer = child.GetComponent<MeshRenderer>();

            if (renderer != null)
            {
                ApplyColor(color, renderer);
            }

            if (child.childCount > 0)
            {
                ApplyMaterialRecursively(child, color);
            }
        }
    }

    private void ApplyColor(Material mat, MeshRenderer mesh) 
    {
        mesh.material = mat;
    }

    private void ConnectParameters()
    {
        Debug.Log("ConnectParameters");
        rigidbody.mass = _boatConfigurationParameters.Mass;
        rigidbody.drag = _boatConfigurationParameters.Drag;
        rigidbody.angularDrag = _boatConfigurationParameters.AngularDrag;

        newBoatMovementManager.forwardAcceleration = _boatConfigurationParameters.ForwardAcceleration;
        newBoatMovementManager.backwardAcceleration = _boatConfigurationParameters.BackwardAcceleration;
        newBoatMovementManager.slowModifier = _boatConfigurationParameters.SlowModifier;
        newBoatMovementManager.fastModifier = _boatConfigurationParameters.FastModifier;
        newBoatMovementManager.rotationSpeed = _boatConfigurationParameters.RotationSpeed;

        _newFloatersManager = _boatConfigurationParameters.NewFloatersManager;
        _newFloatersManager.rigidBody = rigidbody;
        _newFloatersManager.movementManager = newBoatMovementManager;
    }
}
