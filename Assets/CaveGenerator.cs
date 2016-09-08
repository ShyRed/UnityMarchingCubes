using UnityEngine;

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
        x %= 10;
        z %= 10;

        x -= 5;
        z -= 5;

        return y
            * ((x * x + y * y + z * z) - 12)
            * (y * 0.2f + (x * x + z * z) - 3);
    }
}
