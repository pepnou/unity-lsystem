using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Shape
{
    protected Vector3 position;
    protected Quaternion rotation;

    public virtual float get_distance(Vector3 point)
    {
        return distance(rotation * (point - position));
    }
    public abstract float distance(Vector3 point);

    public void get_interval(ref Vector2 xInterval, ref Vector2 yInterval, ref Vector2 zInterval)
    {
        float dist = 10000;

        xInterval[0] = -dist + get_distance(new Vector3(-dist, 0, 0));
        xInterval[1] = dist - get_distance(new Vector3(dist, 0, 0));
        float xint = xInterval.y - xInterval.x;
        xInterval[0] -= xint / 10f;
        xInterval[1] += xint / 10f;

        yInterval[0] = -dist + get_distance(new Vector3(0, -dist, 0));
        yInterval[1] = dist - get_distance(new Vector3(0, dist, 0));
        float yint = yInterval.y - yInterval.x;
        yInterval[0] -= yint / 10f;
        yInterval[1] += yint / 10f;

        zInterval[0] = -dist + get_distance(new Vector3(0, 0, -dist));
        zInterval[1] = dist - get_distance(new Vector3(0, 0, dist));
        float zint = zInterval.y - zInterval.x;
        zInterval[0] -= zint / 10f;
        zInterval[1] += zint / 10f;
    }

    protected Vector3 Abs(Vector3 input)
    {
        return new Vector3(Mathf.Abs(input.x), Mathf.Abs(input.y), Mathf.Abs(input.z));
    }
}

public class Sphere : Shape
{
    public float radius;

    public Sphere(Vector3 position, float radius)
    {
        this.position = position;
        this.rotation = Quaternion.identity;
        this.radius = radius;
    }

    public override float distance(Vector3 point)
    {
        return Vector3.Magnitude(point) - radius;
        //Mathf.Sqrt(Mathf.Pow(originCentered.x,2)+ Mathf.Pow(originCentered.y, 2)+ Mathf.Pow(originCentered.z, 2)) - radius;
    }
}

/*public class InfCylinder : Shape
{
    public Vector3 rotation;

    public InfCylinder(Vector3 position, Vector3 rotation)
    {
        this.position = position;
        this.rotation = rotation;
    }

    public override float get_distance(Vector3 origin)
    {
        Vector3 originCentered = position - origin;
        return Mathf.Sqrt(Mathf.Pow(originCentered.x - rotation.x, 2) + Mathf.Pow(originCentered.z - rotation.y, 2)) - rotation.z;
    }
}*/

public class Cube : Shape
{
    private Vector3 size;

    public Cube(Vector3 position, Quaternion rotation, Vector3 size)
    {
        this.position = position;
        this.rotation = Quaternion.Inverse(rotation);
        this.size = size/2;
    }

    public override float distance(Vector3 point)
    {
        Vector3 q = Abs(point) - size;
        return Vector3.Magnitude(Vector3.Max(q, Vector3.zero)) + Mathf.Min(Mathf.Max(q.x, Mathf.Max(q.y, q.z)), 0f);
    }
}

public class RoundCone : Shape
{
    private float r1, r2, height;

    public RoundCone(Vector3 position, Quaternion rotation, float r1, float r2, float height)
    {
        this.position = position;
        this.rotation = Quaternion.Inverse(rotation);
        this.r1 = r1;
        this.r2 = r2;
        this.height = height;
    }

    public override float distance(Vector3 point)
    {
        /*vec2 q = vec2(length(p.xz), p.y);

        float b = (r1 - r2) / h;
        float a = sqrt(1.0 - b * b);
        float k = dot(q, vec2(-b, a));

        if (k < 0.0) return length(q) - r1;
        if (k > a * h) return length(q - vec2(0.0, h)) - r2;

        return dot(q, vec2(a, b)) - r1;*/

        Vector2 q = new Vector2(Mathf.Sqrt(point.x* point.x + point.z* point.z), point.y);
        
        float b = (r1 - r2) / height;
        float a = Mathf.Sqrt(1f - b * b);
        float k = Vector2.Dot(q, new Vector2(-b, a));

        if (k < 0f) return q.magnitude - r1;
        if (k > a * height) return Mathf.Sqrt(q.x * q.x + (q.y - height) * (q.y - height)) - r2;

        return Vector2.Dot(q, new Vector2(a, b)) - r1;
    }
}

public enum JoinType { UNION, SUBTRACTION, INTERSECTION };
public class ComplexShape : Shape
{
    private List<Tuple<Shape, float, JoinType>> shapes;

    public void resetShape()
    {
        shapes.Clear();
    }
    public ComplexShape()
    {
        shapes = new List<Tuple<Shape, float, JoinType>>();
    }

    float opSmoothUnion(float d1, float d2, float k)
    {
        float h = Mathf.Clamp(0.5f + 0.5f * (d2 - d1) / k, 0.0f, 1.0f);
        return Mathf.Lerp(d2, d1, h) - k * h * (1.0f - h);
    }

    float opSmoothSubtraction(float d1, float d2, float k)
    {
        float h = Mathf.Clamp(0.5f - 0.5f * (d2 + d1) / k, 0.0f, 1.0f);
        return Mathf.Lerp(d2, -d1, h) + k * h * (1.0f - h);
    }

    float opSmoothIntersection(float d1, float d2, float k)
    {
        float h = Mathf.Clamp(0.5f - 0.5f * (d2 - d1) / k, 0.0f, 1.0f);
        return Mathf.Lerp(d2, d1, h) + k * h * (1.0f - h);
    }

    public void add_UnionShape(Shape s, float k)
    {
        shapes.Add(new Tuple<Shape, float, JoinType>(s, Mathf.Max(k, 0.0001f), JoinType.UNION));
    }

    public void add_SubtractionShape(Shape s, float k)
    {
        shapes.Add(new Tuple<Shape, float, JoinType>(s, Mathf.Max(k, 0.0001f), JoinType.SUBTRACTION));
    }

    public void add_IntersectionShape(Shape s, float k)
    {
        shapes.Add(new Tuple<Shape, float, JoinType>(s, Mathf.Max(k, 0.0001f), JoinType.INTERSECTION));
    }

    public override float get_distance(Vector3 point)
    {
        return distance(point);
    }

    public override float distance(Vector3 point)
    {
        float distance = shapes[0].Item1.get_distance(point);

        for (int index = 1; index < shapes.Count; index++)
        {
            switch (shapes[index].Item3)
            {
                case JoinType.UNION:
                    {
                        distance = opSmoothUnion(distance, shapes[index].Item1.get_distance(point), shapes[index].Item2);
                        break;
                    }
                case JoinType.SUBTRACTION:
                    {
                        distance = opSmoothSubtraction(shapes[index].Item1.get_distance(point), distance, shapes[index].Item2);
                        break;
                    }
                case JoinType.INTERSECTION:
                    {
                        distance = opSmoothIntersection(distance, shapes[index].Item1.get_distance(point), shapes[index].Item2);
                        break;
                    }
            }

        }

        return distance;
    }

    public int get_num_shapes()
    {
        return shapes.Count;
    }
}
