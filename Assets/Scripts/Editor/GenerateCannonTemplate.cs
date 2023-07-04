using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Boat.New.Canon;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public enum CannonColliders
{
    Mesh,
    Box
};
public class GenerateCannonTemplate : EditorWindow
{
    private GameObject _object;
    private CannonColliders _collider;
    private string _identifier;
    private int _initialVelocity;

    [MenuItem("Tools/Generate Cannon Template")]
    public static void ShowWindow()
    {
        GetWindow(typeof(GenerateCannonTemplate));
    }

    private void OnGUI()
    {
        GUILayout.BeginVertical("box");
        GUILayout.Label("Object", EditorStyles.boldLabel);
        _object = EditorGUILayout.ObjectField("", _object, typeof(GameObject), false) as GameObject;
        GUILayout.Space(20);
        GUILayout.Label("Components", EditorStyles.boldLabel);
        _collider = (CannonColliders)EditorGUILayout.EnumPopup("Collider", _collider);
        GUILayout.Space(20);
        GUILayout.Label("Parameters", EditorStyles.boldLabel);
        _initialVelocity = EditorGUILayout.IntField("Initial Velocity", _initialVelocity);
        _identifier = EditorGUILayout.TextField("Identifier", _identifier);
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Generate Template"))
        {
            GenerateTemplate();
        }
        GUILayout.EndVertical();
    }

    private void GenerateTemplate()
    {
        if (_object == null)
        {
            Debug.LogError("Error : Please assign an object");
        }
        
        // Create prefab template preview
        GameObject prefabInstancePreview = PrefabUtility.SaveAsPrefabAsset(_object,"Assets/Prefabs/Templates/Cannons/" + _object.name + ".prefab");
        prefabInstancePreview = PrefabUtility.InstantiatePrefab(prefabInstancePreview) as GameObject;
        
        Rotator rotator = prefabInstancePreview.AddComponent<Rotator>();
        rotator.rotation = prefabInstancePreview.transform.up; //TODO : Fix rotation value
        rotator.speed = 50;
        
        PrefabUtility.ApplyPrefabInstance(prefabInstancePreview, InteractionMode.UserAction);
        DestroyImmediate(prefabInstancePreview);
        
        // Create prefab template preview
        GameObject prefabInstanceTemplate = PrefabUtility.SaveAsPrefabAsset(_object, "Assets/Prefabs/Templates/Cannons/" + _object.name + "_Template.prefab");
        prefabInstanceTemplate = PrefabUtility.InstantiatePrefab(prefabInstanceTemplate) as GameObject;
        
        switch (_collider)
        {
            case CannonColliders.Box:
                prefabInstanceTemplate.AddComponent<BoxCollider>();
                break;
            case CannonColliders.Mesh:
                MeshCollider collider = prefabInstanceTemplate.AddComponent<MeshCollider>();
                collider.sharedMesh = _object.GetComponent<MeshFilter>().sharedMesh;
                break;
        }

        CannonConfigurationParameters cannonConfigurationParameters =
            prefabInstanceTemplate.AddComponent<CannonConfigurationParameters>();
        cannonConfigurationParameters.Identifier = _identifier;

        NewFiringManager newFiringManager = prefabInstanceTemplate.AddComponent<NewFiringManager>();
        
        GameObject body = prefabInstanceTemplate.transform.GetChild(0).gameObject;
        newFiringManager.body = body;
        if(body)
        {
            GameObject barrel;
            barrel = body.transform.GetChild(0).gameObject;
            newFiringManager.barrels = barrel;
            if (barrel)
            {
                if (barrel.transform.childCount > 0)
                {
                    Transform[] barrels = new Transform[barrel.transform.childCount];
                    int i = 0;
                    foreach (Transform child in barrel.transform)
                    {
                        GameObject barrelOut = new GameObject("barrel_out"); 
                        barrelOut.transform.SetParent(child.transform);
                        barrelOut.transform.position = child.transform.position;
                        barrels[i] = barrelOut.transform;
                        i++;
                    }

                    newFiringManager.barrelOutputs = barrels;
                }
                else
                {
                    GameObject barrelOut = new GameObject("barrel_out"); 
                    barrelOut.transform.SetParent(barrel.transform);
                    barrelOut.transform.position = barrel.transform.position;
                    newFiringManager.barrelOutputs = new Transform[] { barrelOut.transform };
                }
            }
            else
            {
                Debug.LogWarning(
                    "Barrel not found, you'll need to create the barrel output and assign it in the script");
            }

            newFiringManager.initialVelocity = _initialVelocity;
            GameObject bullet = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Entities/New/SubEntities/Bullet.prefab");
            if (bullet) newFiringManager.cannonBall = bullet;
            else
            {
                Debug.LogWarning("Bullet prefab not found, you'll need to add the prefab directly to the script");
            }
        }
        else
        {
            Debug.LogWarning("Body not found, can't properly generate the template");
        }
        PrefabUtility.ApplyPrefabInstance(prefabInstanceTemplate, InteractionMode.UserAction);
        DestroyImmediate(prefabInstanceTemplate);
        
        AssetDatabase.Refresh();
    }
}
