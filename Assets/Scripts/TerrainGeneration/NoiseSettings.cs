using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NoiseSettings
{
    public SimpleNoiseSettings simpleNoiseSettings;
    public RigidNoiseSettings rigidNoiseSettings;

    public enum FilterType { Simple, Rigid };
    public FilterType filterType;

    [System.Serializable]
    public class SimpleNoiseSettings
    {
        [Range(1, 8)]
        public int nLayers = 1;
        public float persistence = 0.5f;
        public float strength = 1;
        public float baseRoughness = 1;
        public float roughness = 2;
        public Vector3 centre;
        public float minValue = 0;
    }

    [System.Serializable]
    public class RigidNoiseSettings : SimpleNoiseSettings
    {
        public float weightMultiplier = 0.8f;
    }

}