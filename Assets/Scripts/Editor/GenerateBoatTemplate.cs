using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Boat.New;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public enum BoatColliders
{
    Mesh, 
    Box
};
public class GenerateBoatTemplate : EditorWindow
{
    private int _cannonAmount;
    private int _floaterAmount;
    private GameObject _object;
    private BoatColliders _collider;
    
    private int _mass;
    private float _drag;
    private float _angularDrag;
    private int _forwardAcceleration;
    private int _backwardAcceleration;
    private float _slowModifier;
    private float _fastModifier;
    private int _rotationSpeed;
    private Transform[] _cannonPosition;
    private NewFloatersManager _newFloatersManager;
    private string _identifier;

    [MenuItem("Tools/Generate Boat Template")]
    public static void ShowWindow()
    {
        GetWindow(typeof(GenerateBoatTemplate));
    }

    private void OnGUI()
    {
        GUILayout.BeginVertical("box");
        GUILayout.Label("Object", EditorStyles.boldLabel);
        _object = EditorGUILayout.ObjectField("", _object, typeof(GameObject), false) as GameObject;
        GUILayout.Space(20);
        GUILayout.Label("Components", EditorStyles.boldLabel);
        _collider = (BoatColliders)EditorGUILayout.EnumPopup("Collider", _collider);
        GUILayout.Space(20);
        GUILayout.Label("Parameters", EditorStyles.boldLabel);
        _cannonAmount = EditorGUILayout.IntField("Cannon amount", _cannonAmount);
        _floaterAmount = EditorGUILayout.IntField("Floater amount", _floaterAmount);
        _mass = EditorGUILayout.IntField("Mass", _mass);
        _drag = EditorGUILayout.FloatField("Drag" , _drag);
        _angularDrag = EditorGUILayout.FloatField("Angular Drag" , _angularDrag);
        _forwardAcceleration = EditorGUILayout.IntField("Forward Acceleration" , _forwardAcceleration);
        _backwardAcceleration = EditorGUILayout.IntField("Backward Acceleration" , _backwardAcceleration);
        _slowModifier = EditorGUILayout.FloatField("Slow Modifier", _slowModifier);
        _fastModifier = EditorGUILayout.FloatField("Fast Modifier", _fastModifier);
        _rotationSpeed = EditorGUILayout.IntField("Rotation Speed" , _rotationSpeed);
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

        Transform[] cannonPos = new Transform[_cannonAmount];

        TemplatesDictionary templatesDictionaryConfig =
            AssetDatabase.LoadAssetAtPath<TemplatesDictionary>("Assets/Prefabs/Templates/TemplatesDictionaryConfig.asset");

        // Create prefab template preview
        GameObject prefabInstancePreview = PrefabUtility.SaveAsPrefabAsset(_object ,"Assets/Prefabs/Templates/Boats/" + _object.name + ".prefab");
        templatesDictionaryConfig.AddBoatPreview(prefabInstancePreview);
        prefabInstancePreview = PrefabUtility.InstantiatePrefab(prefabInstancePreview) as GameObject;
        
        BuildBoatPreview buildBoatPreview = prefabInstancePreview.AddComponent<BuildBoatPreview>();
        GameObject cannon = new GameObject("Cannons");
        cannon.transform.SetParent(prefabInstancePreview.transform);
        for (int i = 0; i < _cannonAmount; i++)
        {
            GameObject pos = new GameObject("Pos" + (i + 1));
            pos.transform.SetParent(cannon.transform);
            cannonPos[i] = pos.transform;
        }

        buildBoatPreview.cannonPos = cannonPos;
        Rotator rotator = prefabInstancePreview.AddComponent<Rotator>();
        rotator.rotation = prefabInstancePreview.transform.up; //TODO : Fix rotation value
        rotator.speed = 50;
        
        PrefabUtility.ApplyPrefabInstance(prefabInstancePreview, InteractionMode.UserAction);
        DestroyImmediate(prefabInstancePreview);
        
        // Create prefab template preview
        GameObject prefabInstanceTemplate = PrefabUtility.SaveAsPrefabAsset(_object, "Assets/Prefabs/Templates/Boats/" + _object.name + "_Template.prefab");
        templatesDictionaryConfig.AddBoatTemplate(prefabInstanceTemplate);
        prefabInstanceTemplate = PrefabUtility.InstantiatePrefab(prefabInstanceTemplate) as GameObject;
        
        switch (_collider)
        {
            case BoatColliders.Box:
                prefabInstanceTemplate.AddComponent<BoxCollider>();
                break;
            case BoatColliders.Mesh:
                MeshCollider collider = prefabInstanceTemplate.AddComponent<MeshCollider>();
                collider.sharedMesh = _object.GetComponent<MeshFilter>().sharedMesh;
                break;
        }
        
        cannon = new GameObject("Cannons");
        cannon.transform.SetParent(prefabInstanceTemplate.transform);
        for (int i = 0; i < _cannonAmount; i++)
        {
            GameObject pos = new GameObject("Pos" + (i + 1));
            pos.transform.SetParent(cannon.transform);
            cannonPos[i] = pos.transform;
        }
        
        GameObject floaters = new GameObject("Floaters");
        floaters.transform.SetParent(prefabInstanceTemplate.transform);
        NewFloatersManager newFloatersManager = floaters.AddComponent<NewFloatersManager>();
        for (int i = 0; i < _floaterAmount; i++)
        {
            GameObject floater = new GameObject("Floater" + (i + 1));
            floater.transform.SetParent(floaters.transform);
            NewFloater newFloater = floater.AddComponent<NewFloater>();
            newFloater.Manager = newFloatersManager;
        }

        BoatConfigurationParameters boatConfigurationParameters = prefabInstanceTemplate.AddComponent<BoatConfigurationParameters>();
        boatConfigurationParameters.Mass = _mass;
        boatConfigurationParameters.Drag = _drag;
        boatConfigurationParameters.AngularDrag = _angularDrag;
        boatConfigurationParameters.ForwardAcceleration = _forwardAcceleration;
        boatConfigurationParameters.BackwardAcceleration = _backwardAcceleration;
        boatConfigurationParameters.SlowModifier = _slowModifier;
        boatConfigurationParameters.FastModifier = _fastModifier;
        boatConfigurationParameters.RotationSpeed = _rotationSpeed;
        boatConfigurationParameters.CannonPos = cannonPos;
        boatConfigurationParameters.NewFloatersManager = newFloatersManager;
        boatConfigurationParameters.Identifier = _identifier;
        
        PrefabUtility.ApplyPrefabInstance(prefabInstanceTemplate, InteractionMode.UserAction);
        DestroyImmediate(prefabInstanceTemplate);
        
        AssetDatabase.Refresh();
    }
}
