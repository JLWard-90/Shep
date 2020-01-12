using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinMax
{
    public float Min { get; private set; }
    public float Max { get; private set; }

    public MinMax()
    {
        Min = float.MaxValue;
        Max = float.MinValue;
    }

    public void AddValue(float v)
    {
        if (v > Max && v < 1e10)
        {
            Max = v;
        }
        if (v < Min && v > -1e10)
        {
            Min = v;
        }
    }
}
