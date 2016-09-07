using UnityEngine;
using System.Collections;

/// <summary>
/// Generates cave data for the <c>MarchingCuber</c>.
/// </summary>
[RequireComponent(typeof (MarchingCuber))]
public class CaveGenerator : MonoBehaviour {

    public int Seed = 0;

    [Range(5, 25)]
    public int Width = 10;

    [Range(5, 25)]
    public int Height = 10;

    [Range(5, 25)]
    public int Length = 10;

    // Use this for initialization
    void Start () {
        int[,,] voxeldata = new int[Width, Height, Length];
        for(int x = 0; x < Width; x++)
        {
            for(int y = 0; y < Height; y++)
            {
                for(int z = 0; z < Length; z++)
                {
                    float thresh = Mathf.PerlinNoise(x * 2 / (float)Width, y * 2 / (float)Height)
                        + Mathf.PerlinNoise(-x * 3 / (float)Width, z * 3 / (float)Height);
                    voxeldata[x, y, z] = thresh > 1f ? 1 : 0;
                }
            }
        }

        MarchingCuber cuber = GetComponent<MarchingCuber>();
        cuber.Voxeldata = voxeldata;
        cuber.GenerateMesh();
	}
}
