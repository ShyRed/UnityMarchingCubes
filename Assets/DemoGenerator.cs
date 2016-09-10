using UnityEngine;

/// <summary>
/// Generates cave data for the <c>MarchingCuber</c>.
/// Have a look at http://iquilezles.org/www/articles/distfunctions/distfunctions.htm
/// for ideas.
/// </summary>
public class DemoGenerator : DistancefieldGenerator {

    /// <summary>
    /// Generates the value at the specified position.
    /// </summary>
    /// <param name="position">The position in the distance field to sample.</param>
    /// <returns>The value at the specified position.</returns>
    public override float GenerateValue(Vector3 position)
    {
        float distance = 1f;

        int cellX = (int)(position.x / 6f);
        int cellZ = (int)(position.z / 6f);

        // Add floor
        distance = Union(distance, position.y);

        // Add sphere
        if (cellX == 0 && cellZ == 0)
        {
            Vector3 pos = Translate(position, new Vector3(4f, 4f, 4f));
            distance = Union(distance, SignedSphere(pos, 2f));
        }

        // Add box
        else if (cellX == 1 && cellZ == 0)
        {
            Vector3 pos = Translate(position, new Vector3(10f, 4f, 4f));
            distance = Union(distance, UnsignedBox(pos, new Vector3(2f, 1f, 1.5f)));
        }

        // Add rounded box
        else if (cellX == 2 && cellZ == 0)
        {
            Vector3 pos = Translate(position, new Vector3(16f, 4f, 4f));
            distance = Union(distance, UnsignedRoundBox(pos, new Vector3(2f, 1f, 1.5f), 0.5f));
        }

        // Add SignedBox
        else if (cellX == 3 && cellZ == 0)
        {
            Vector3 pos = Translate(position, new Vector3(22f, 4f, 4f));
            distance = Union(distance, SignedBox(pos, new Vector3(2f, 1f, 1.5f)));
        }

        // Add SignedTorus
        else if (cellX == 4 && cellZ == 0)
        {
            Vector3 pos = Translate(position, new Vector3(28f, 4f, 4f));
            distance = Union(distance, SignedTorus(pos, 2f, 0.5f));
        }

        // Add SignedCylinder.
        else if (cellX == 0 && cellZ == 1)
        {
            Vector3 pos = Translate(position, new Vector3(4f, 4f, 10f));
            distance = Union(distance, SignedCylinder(pos, new Vector3(0.5f, 0.5f, 2f)));
        }

        // Add SignedCone. Cone is a bit difficult to handle, as it grows infinitely.
        // So we do an intersection with a box to limit the area it consumes.
        else if (cellX == 1 && cellZ == 1)
        {
            Vector3 pos = Translate(position, new Vector3(10f, 4f, 10f));
            distance = Union(distance, Intersection(UnsignedBox(pos, new Vector3(2f, 2f, 2f)),
                SignedCone(pos, new Vector2(0.60f, 0.30f).normalized)));
        }

        // Add SignedHexagonalPrism
        else if (cellX == 2 && cellZ == 1)
        {
            Vector3 pos = Translate(position, new Vector3(16f, 4f, 10f));
            distance = Union(distance, SignedHexagonalPrism(pos, new Vector2(2f, 1f)));
        }

        // Add SignedTriangularPrism
        else if (cellX == 3 && cellZ == 1)
        {
            Vector3 pos = Translate(position, new Vector3(22f, 4f, 10f));
            distance = Union(distance, SignedTriangularPrism(pos, new Vector2(2f, 1f)));
        }

        // Add SignedCappedCylinder
        else if (cellX == 4 && cellZ == 1)
        {
            Vector3 pos = Translate(position, new Vector3(28f, 4f, 10f));
            distance = Union(distance, SignedCappedCylinder(pos, new Vector2(2f, 3f)));
        }

        // Add SignedEllipsoid
        else if (cellX == 0 && cellZ == 2)
        {
            Vector3 pos = Translate(position, new Vector3(4f, 4f, 16f));
            distance = Union(distance, SignedEllipsoid(pos, new Vector3(3f, 2f, 1f)));
        }

        // Add union between box and sphere
        else if (cellX == 1 && cellZ == 2)
        {
            Vector3 pos = Translate(position, new Vector3(10f, 4f, 16f));
            distance = Union(distance, Union(
                SignedSphere(pos, 2f),
                UnsignedBox(pos, new Vector3(2f, 1f, 1.5f))));
        }

        // Add subtraction between box and sphere
        else if (cellX == 2 && cellZ == 2)
        {
            Vector3 pos = Translate(position, new Vector3(16f, 4f, 16f));
            distance = Union(distance, Subtraction(
                UnsignedBox(pos, new Vector3(2f, 1f, 1.5f)),
                SignedSphere(pos, 2f)));
        }

        // Add intersection between box and sphere
        else if (cellX == 3 && cellZ == 2)
        {
            Vector3 pos = Translate(position, new Vector3(22f, 4f, 16f));
            distance = Union(distance, Intersection(
                SignedSphere(pos, 1.75f),
                UnsignedBox(pos, new Vector3(2f, 1f, 1.5f))));
        }

        // Add twisted torus
        else if (cellX == 4 && cellZ == 2)
        {
            Vector3 pos = Translate(position, new Vector3(28f, 4f, 16f));
            Vector3 twistedPos = Twist(pos, 45f);
            twistedPos = RotateAndTranslate(twistedPos, Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(90f, 0f, 0f), Vector3.one));
            distance = Union(distance, SignedTorus(Twist(twistedPos, 20f), 2f, 0.75f));
        }

        return distance;
    }
}
