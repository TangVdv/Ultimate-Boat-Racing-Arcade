using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatManager : MonoBehaviour
{
    [SerializeField] private Material material;
    
    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        _meshRenderer = gameObject.GetComponent<MeshRenderer>();
        _meshRenderer.material = material;
        
    }

    public Color CurrentMaterialColor
    {
        get => _meshRenderer.material.color;
        set
        {
            if (_meshRenderer)
            {
                _meshRenderer.material.color = value;
            }
        }
    }
    
}
