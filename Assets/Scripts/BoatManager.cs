using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class BoatManager : MonoBehaviour
{
    [SerializeField] private Material material;
    
    private MeshRenderer _meshRenderer;

    private void Start()
    {
        _meshRenderer = gameObject.GetComponent<MeshRenderer>();
        _meshRenderer.material = material;
        
    }

    public Color CurrentMaterialColor
    {
        get => _meshRenderer.material.color;
        set => _meshRenderer.material.color = value;
    }
    
}
