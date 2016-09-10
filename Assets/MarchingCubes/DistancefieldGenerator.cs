using UnityEngine;

/// <summary>
/// Abstract base class for all voxel data generators with some primitive
/// distance field functions based on Inigo Quilez' research
/// ( see http://iquilezles.org/www/articles/distfunctions/distfunctions.htm ).
/// </summary>
public abstract class DistancefieldGenerator : MonoBehaviour {

    /// <summary>
    /// Vector with zero magnitude.
    /// </summary>
    public static readonly Vector2 Zero2 = new Vector2(0f, 0f);

    /// <summary>
    /// Vector with zero magnitude.
    /// </summary>
    public static readonly Vector3 Zero3 = new Vector3(0f, 0f, 0f);

    /// <summary>
    /// Generates the value at the specified position.
    /// </summary>
    /// <param name="position">The position in the distance field to sample.</param>
    /// <returns>The value at the specified position.</returns>
    public abstract float GenerateValue(Vector3 position);

    /// <summary>
    /// Signed sphere.
    /// </summary>
    /// <param name="position">Sample position.</param>
    /// <param name="radius">Radius of the sphere.</param>
    /// <returns>Distance to sphere.</returns>
    public static float SignedSphere(Vector3 position, float radius)
    {
        // return length(p)-s;
        return position.magnitude - radius;
    }

    /// <summary>
    /// Unsigned box.
    /// </summary>
    /// <param name="position">Sample position.</param>
    /// <param name="radius">Box radius.</param>
    /// <returns>Distance to box.</returns>
    public static float UnsignedBox(Vector3 position, Vector3 radius)
    {
        // return length(max(abs(p)-b,0.0));
        return Vector3.Max(Abs(position) - radius, Zero3).magnitude;
    }

    /// <summary>
    /// Unsigned box with round borders.
    /// </summary>
    /// <param name="position">Sample position.</param>
    /// <param name="radius">Box radius.</param>
    /// <param name="borderRadius">Border radius.</param>
    /// <returns>Distance to box.</returns>
    public static float UnsignedRoundBox(Vector3 position, Vector3 radius, float borderRadius)
    {
        // length(max(abs(p) - b, 0.0)) - r
        return Vector3.Max(Abs(position) - radius, Zero3).magnitude - borderRadius;
    }

    /// <summary>
    /// Signed box.
    /// </summary>
    /// <param name="position">Sample position.</param>
    /// <param name="radius">Box radius.</param>
    /// <returns>Distance to box.</returns>
    public static float SignedBox(Vector3 position, Vector3 radius)
    {
        // vec3 d = abs(p) - b;
        // return min(max(d.x, max(d.y, d.z)), 0.0) + length(max(d, 0.0));
        Vector3 d = Abs(position) - radius;
        return Mathf.Min(Mathf.Max(d.x, Mathf.Max(d.y, d.z)), 0f) + Vector3.Max(d, Zero3).magnitude;
    }

    /// <summary>
    /// Signed torus.
    /// </summary>
    /// <param name="position">Sample position.</param>
    /// <param name="innerRadius">Inner radius.</param>
    /// <param name="outerradius">Outer radius.</param>
    /// <returns>Distance to torus.</returns>
    public static float SignedTorus(Vector3 position, float innerRadius, float outerradius)
    {
        // vec2 q = vec2(length(p.xz)-t.x,p.y);
        // return length(q) - t.y;
        Vector2 q = new Vector2(new Vector2(position.x, position.z).magnitude - innerRadius, position.y);
        return q.magnitude - outerradius;
    }

    /// <summary>
    /// Signed cylinder.
    /// </summary>
    /// <param name="position">Sample position.</param>
    /// <param name="radius">Cylinder radius.</param>
    /// <returns>Distance to cylinder.</returns>
    public static float SignedCylinder(Vector3 position, Vector3 radius)
    {
        // return length(p.xz-c.xy)-c.z;
        return (new Vector2(position.x, position.z) - new Vector2(radius.x, radius.y)).magnitude - radius.z;
    }

    /// <summary>
    /// Signed cone.
    /// </summary>
    /// <param name="position">Sample position.</param>
    /// <param name="radius">Normalized radius.</param>
    /// <returns>Distance to cone.</returns>
    public static float SignedCone(Vector3 position, Vector2 radius)
    {
        // c must be normalized
        // float q = length(p.xy);
        // return dot(c, vec2(q, p.z));
        float q = new Vector2(position.x, position.y).magnitude;
        return Vector2.Dot(radius, new Vector2(q, position.z));
    }

    /// <summary>
    /// Signed plane.
    /// </summary>
    /// <param name="position">Sample position.</param>
    /// <param name="normal">Normalized plane normal vector.</param>
    /// <returns>Distance to plane.</returns>
    public static float SignedPlane(Vector3 position, Vector4 normal)
    {
        // n must be normalized
        // return dot(p, n.xyz) + n.w;
        return Vector3.Dot(position, new Vector3(normal.x, normal.y, normal.z)) + normal.w;
    }

    /// <summary>
    /// Signed hexagonal prism.
    /// </summary>
    /// <param name="position">Sample position.</param>
    /// <param name="radius">Prism radius.</param>
    /// <returns>Distance to prism.</returns>
    public static float SignedHexagonalPrism(Vector3 position, Vector2 radius)
    {
        // vec3 q = abs(p);
        // return max(q.z - h.y, max((q.x * 0.866025 + q.y * 0.5), q.y) - h.x);
        Vector3 q = Abs(position);
        return Mathf.Max(q.z - radius.y, Mathf.Max((q.x * 0.866025f + q.y * 0.5f), q.y) - radius.x);
    }

    /// <summary>
    /// Signed triangular prism.
    /// </summary>
    /// <param name="position">Sample position.</param>
    /// <param name="radius">Prism radius.</param>
    /// <returns>Distance to prism.</returns>
    public static float SignedTriangularPrism(Vector3 position, Vector2 radius)
    {
        // vec3 q = abs(p);
        // return max(q.z - h.y, max(q.x * 0.866025 + p.y * 0.5, -p.y) - h.x * 0.5);
        Vector3 q = Abs(position);
        return Mathf.Max(q.z - radius.y, Mathf.Max(q.x * 0.866025f + position.y * 0.5f, -position.y) - radius.x * 0.5f);
    }

    /// <summary>
    /// Signed capped cylinder.
    /// </summary>
    /// <param name="position">Sample position.</param>
    /// <param name="radius">Cylinder radius. X is radius and Y is length.</param>
    /// <returns>Distance to cylinder.</returns>
    public static float SignedCappedCylinder(Vector3 position, Vector2 radius)
    {
        // vec2 d = abs(vec2(length(p.xz), p.y)) - h;
        // return min(max(d.x, d.y), 0.0) + length(max(d, 0.0));
        Vector2 d = Abs(new Vector2(new Vector2(position.x, position.z).magnitude, position.y)) - radius;
        return Mathf.Min(Mathf.Max(d.x, d.y), 0.0f + Vector2.Max(d, Zero2).magnitude);
    }

    /// <summary>
    /// Signed ellipsoid.
    /// </summary>
    /// <param name="position">Sample position.</param>
    /// <param name="radius">Ellipsoid radius.</param>
    /// <returns>Distance to ellipsoid.</returns>
    public static float SignedEllipsoid(Vector3 position, Vector3 radius)
    {
        // return (length( p/r ) - 1.0) * min(min(r.x,r.y),r.z);
        return (Divide(position, radius).magnitude - 1f) * Mathf.Min(Mathf.Min(radius.x, radius.y), radius.z);
    }

    /// <summary>
    /// Union of two distance fields.
    /// </summary>
    /// <param name="a">First distance field.</param>
    /// <param name="b">Second distance field.</param>
    /// <returns>Union between distance field a and b.</returns>
    public static float Union(float a, float b)
    {
        return Mathf.Min(a, b);
    }

    /// <summary>
    /// Subtraction of two distance fields.
    /// </summary>
    /// <param name="a">First distance field.</param>
    /// <param name="b">Second distance field.</param>
    /// <returns>Subtraction of a by b.</returns>
    public static float Subtraction(float a, float b)
    {
        return Mathf.Max(-a, b);
    }

    /// <summary>
    /// Intersection of two distance fields.
    /// </summary>
    /// <param name="a">First distance field.</param>
    /// <param name="b">Second distance field.</param>
    /// <returns>Intersection of distance field a and b.</returns>
    public static float Intersection(float a, float b)
    {
        return Mathf.Max(a, b);
    }

    /// <summary>
    /// Repeats the position within the repitition.
    /// </summary>
    /// <param name="position">Sample position.</param>
    /// <param name="repetition">Reptition.</param>
    /// <returns>Repeated position.</returns>
    public static Vector3 Repeat(Vector3 position, Vector3 repetition)
    {
        // vec3 q = mod(p,c)-0.5*c;
        return Mod(position, repetition) - 0.5f * repetition;
    }

    /// <summary>
    /// Translates the position.
    /// </summary>
    /// <param name="position">Sample position.</param>
    /// <param name="translation">Translation.</param>
    /// <returns>Translated position.</returns>
    public static Vector3 Translate(Vector3 position, Vector3 translation)
    {
        return position - translation;
    }

    /// <summary>
    /// Rotates and translates the position by the specified matrix.
    /// </summary>
    /// <param name="position">Sample position.</param>
    /// <param name="matrix">Rotation and translation matrix.</param>
    /// <returns>The rotated and translated vector.</returns>
    public static Vector3 RotateAndTranslate(Vector3 position, Matrix4x4 matrix)
    {
        // vec3 q = invert(m)*p;
        return Matrix4x4.Inverse(matrix) * position;
    }

    /// <summary>
    /// Twists the position-
    /// </summary>
    /// <param name="position">Sample position.</param>
    /// <param name="strength">Twist strength.</param>
    /// <returns>The twisted vector.</returns>
    public static Vector3 Twist(Vector3 position, float strength)
    {
        // float c = cos(20.0*p.y);
        // float s = sin(20.0 * p.y);
        // mat2 m = mat2(c, -s, s, c);
        // vec3 q = vec3(m * p.xz, p.y);
        // return primitive(q);
        Quaternion q = Quaternion.Euler(0f, position.y * strength, 0f);
        return q * position;
    }

    /// <summary>
    /// Calculates modulo between Vector a and b.
    /// </summary>
    /// <param name="a">First vector.</param>
    /// <param name="b">Second vector.</param>
    /// <returns>Modulo of vector a b b.</returns>
    public static Vector3 Mod(Vector3 a, Vector3 b)
    {
        return new Vector3(
            a.x % b.x,
            a.y % b.y,
            a.z % b.z);
    }

    /// <summary>
    /// Returns absolute version of the vector.
    /// </summary>
    /// <param name="vector">The vector.</param>
    /// <returns>Vector with absolute values.</returns>
    public static Vector2 Abs(Vector2 vector)
    {
        return new Vector2(
            vector.x > 0 ? vector.x : -vector.x,
            vector.y > 0 ? vector.y : -vector.y);
    }

    /// <summary>
    /// Returns absolute version of the vector.
    /// </summary>
    /// <param name="vector">The vector.</param>
    /// <returns>Vector with absolute values.</returns>
    public static Vector3 Abs(Vector3 vector)
    {
        return new Vector3(
            vector.x > 0 ? vector.x : -vector.x,
            vector.y > 0 ? vector.y : -vector.y,
            vector.z > 0 ? vector.z : -vector.z);
    }

    /// <summary>
    /// "Divides" a by b.
    /// </summary>
    /// <param name="a">First vector.</param>
    /// <param name="b">Second vector.</param>
    /// <returns>a / b.</returns>
    public static Vector3 Divide(Vector3 a, Vector3 b)
    {
        return new Vector3(a.x / b.x, a.y / b.y, a.z / b.z);
    }

}
