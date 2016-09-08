using UnityEngine;

/// <summary>
/// Abstract base class for all voxel data generators.
/// </summary>
public abstract class DistancefieldGenerator : MonoBehaviour {

    /// <summary>
    /// Generates the value at the specified position.
    /// </summary>
    /// <param name="x">The x coordinate.</param>
    /// <param name="y">The y coordinate.</param>
    /// <param name="z">The z coordinate.</param>
    /// <returns>The value at the specified position.</returns>
    public abstract float GenerateValue(float x, float y, float z);
	
}
