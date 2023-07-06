using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[CreateAssetMenu(fileName = "TerrainDictionaryConfig", menuName = "TerrainDictionary", order = 0)]
public class TerrainDictionary : ScriptableObject
{
    [SerializeField] private List<GameObject> terrain = new List<GameObject>();
    [SerializeField] private List<GameObject> terrainPreview = new List<GameObject>();
    [SerializeField] private List<NavMeshData> terrainNavmesh = new List<NavMeshData>();

    public List<GameObject> TerrainPrefab => terrain;
    public List<GameObject> TerrainPreview => terrainPreview;
    public List<NavMeshData> TerrainNavmesh => terrainNavmesh;
    
    public void AddTerrainPrefab(GameObject terrain)
    {
        if(!TerrainAlreadyExist(this.terrain, terrain))
            this.terrain.Add(terrain);
    }
    
    public void AddTerrainPreview(GameObject terrain)
    {
        if(!TerrainAlreadyExist(terrainPreview, terrain))
            terrainPreview.Add(terrain);
    }

    private bool TerrainAlreadyExist(List<GameObject> list, GameObject terrain)
    {
        return list.Contains(terrain);
    }
}
