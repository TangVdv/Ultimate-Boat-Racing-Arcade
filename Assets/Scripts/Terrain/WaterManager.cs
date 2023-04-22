using UnityEngine;

namespace Terrain
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]

    public class WaterManager : MonoBehaviour
    {
        private MeshFilter _meshFilter;

        private void Awake()
        {
            _meshFilter = GetComponent<MeshFilter>();
        }

        private void Update()
        {
            var vertices = _meshFilter.mesh.vertices;
            for (var i = 0; i < vertices.Length; ++i)
            {
                vertices[i].y = WaveManager.instance.GetWaveHeight(transform.position.x + vertices[i].x);
            }

            _meshFilter.mesh.vertices = vertices;
            _meshFilter.mesh.RecalculateNormals();
        }
    }
}
