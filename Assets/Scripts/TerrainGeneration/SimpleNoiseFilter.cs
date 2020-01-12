using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleNoiseFilter : iNoiseFilter
{
    NoiseSettings.SimpleNoiseSettings settings;
    Noise noise = new Noise();


    public SimpleNoiseFilter(NoiseSettings.SimpleNoiseSettings settings)
    {
        this.settings = settings;
    }

    public float evaluate(Vector3 point)
    {
        float noiseValue = 0;
        float amplitude = 1;
        float frequency = settings.baseRoughness;

        for (int i=0; i < settings.nLayers; i++)
        {
            float v = noise.Evaluate(point * frequency);
            noiseValue += (v + 1) * .5f * amplitude;
            frequency *= settings.roughness;
            amplitude *= settings.persistence;
        }

        return noiseValue * settings.strength;
    }
}
