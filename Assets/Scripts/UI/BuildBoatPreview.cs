using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildBoatPreview : MonoBehaviour
{
    public Transform[] cannonPos;

    private GameObject _boatTemplate;
    private GameObject _cannonTemplate;

    public void CreateCannon(GameObject cannon, Material color)
    {
        ClearCannon();
        if (cannon)
        {
            if (cannonPos.Length > 0)
            {
                foreach (Transform pos in cannonPos)
                {
                    _cannonTemplate = Instantiate(cannon, pos);
                    _cannonTemplate.GetComponent<Rotator>().rotation = new Vector3(0f, 0f, 0f);
                    if (color)
                    {
                        ApplyColor(color, _cannonTemplate.GetComponent<MeshRenderer>());
                        ApplyMaterialRecursively(_cannonTemplate.transform, color);   
                    }
                }
            }   
        }
    }

    private void ClearCannon()
    {
        foreach (Transform pos in cannonPos)
        {
            if (pos.childCount > 0)
            {
                var cannon = pos.GetChild(0);
                Destroy(cannon.gameObject);   
            }
        }
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

    public void ApplyColor(Material mat, MeshRenderer mesh) 
    {
        mesh.material = mat;
    }
}
