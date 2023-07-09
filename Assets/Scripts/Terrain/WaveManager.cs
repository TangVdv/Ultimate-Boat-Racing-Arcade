using System;
using UnityEngine;

namespace Terrain
{
    public class WaveManager : MonoBehaviour
    {
        public static WaveManager instance;
        public Material waterMaterial;

        public float amplitude = 1f;
        public float length = 2f;
        public float speed = 1f;
        public float offset = 0f;
        public bool debug = false;
        private static readonly int Amplitude = Shader.PropertyToID("_amplitude");
        private static readonly int Length = Shader.PropertyToID("_length");
        private static readonly int Speed = Shader.PropertyToID("_speed");

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;

                
            }
            else if(instance != this)
            {
                if(debug) Debug.Log("Instance already exists, destroying object!");
                Destroy(this);
            }
        }

        private void Update()
        {
            offset += Time.deltaTime * speed;
            waterMaterial.SetFloat(Amplitude, amplitude);
            waterMaterial.SetFloat(Length, length);
            waterMaterial.SetFloat(Speed, speed);
        }

        public float GetWaveHeight(float _x)
        {
            return amplitude * Mathf.Sin(_x / length + offset);
        }
        
    }
}
