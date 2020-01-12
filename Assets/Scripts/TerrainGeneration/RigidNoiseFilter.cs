using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidNoiseFilter : iNoiseFilter
{
    NoiseSettings.RigidNoiseSettings settings;
    Noise noise = new Noise();

    public RigidNoiseFilter(NoiseSettings.RigidNoiseSettings settings)
    {
        this.settings = settings;
    }

    public float evaluate(Vector3 point)
    {
        float noiseValue = 0;
        float frequency = settings.baseRoughness;
        float amplitude = 1;
        float weight = 1;

        for (int i = 0; i < settings.nLayers; i++)
        {
            float v = 1 - Mathf.Abs(noise.Evaluate(point * frequency + settings.centre));
            v *= v;
            v *= weight;
            weight = v * settings.weightMultiplier; //This makes layers that are low down remain relatively undetailed compared to higher up layers
            noiseValue += v * amplitude;
            frequency *= settings.roughness; //roughness > 1 -> frequency of noise increases with each layer
            amplitude *= settings.persistence; //persistence < 1 -> amplitude decreases with each layer
        }
        noiseValue = noiseValue - settings.minValue;
        return noiseValue * settings.strength;
    }
}
