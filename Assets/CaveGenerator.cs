﻿using UnityEngine;

/// <summary>
/// Generates cave data for the <c>MarchingCuber</c>.
/// Have a look at http://iquilezles.org/www/articles/distfunctions/distfunctions.htm
/// for ideas.
/// </summary>
public class CaveGenerator : DistancefieldGenerator {

    /// <summary>
    /// Generates the value at the specified position.
    /// </summary>
    /// <param name="x">The x coordinate.</param>
    /// <param name="y">The y coordinate.</param>
    /// <param name="z">The z coordinate.</param>
    /// <returns>The value at the specified position.</returns>
    public override float GenerateValue(float x, float y, float z)
    {
        float origX = x;
        float origZ = z;

        x %= 15;
        z %= 15;

        x -= 5;
        z -= 5;

        return y
            * ((x * x + y * y + z * z) - 12)
            * (y * 0.2f + (x * x + z * z) - 3)
            * (Mathf.PerlinNoise(origX * 0.1f, origZ * 0.1f + y) - 0.2f)
            * (Mathf.PerlinNoise(origX * 0.1f, y * 0.1f) - 0.2f)
            * (Mathf.PerlinNoise(origZ * 0.1f, y * 0.1f) - 0.2f);
    }
}
