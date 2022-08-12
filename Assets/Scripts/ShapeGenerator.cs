using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class ShapeGenerator : MonoBehaviour
{
    [SerializeField] private Vector3Int sample;

    private ComplexShape shape = new ComplexShape();

    private Vector3[,,] position;
    private float[,,] field;

    private Vector2 xInterval = new Vector2(-1f,1f);
    private Vector2 yInterval = new Vector2(-1f, 1f);
    private Vector2 zInterval = new Vector2(-1f, 1f);

    [SerializeField] private Material defaultMat;



    public void ResetShape()
    {
        shape.resetShape();
    }
    public void AddShape(JoinType type, Shape _shape, float smoothing)
    {
        switch(type)
        {
            case JoinType.INTERSECTION:
                shape.add_IntersectionShape(_shape, smoothing);
                break;
            case JoinType.SUBTRACTION:
                shape.add_SubtractionShape(_shape, smoothing);
                break;
            case JoinType.UNION:
                shape.add_UnionShape(_shape, smoothing);
                break;
        }
    }
    public void GenerateShape()
    {
        Debug.Log("Num shapes: " + shape.get_num_shapes());

        position = new Vector3[sample.x + 1, sample.y + 1, sample.z + 1];
        field = new float[sample.x + 1, sample.y + 1, sample.z + 1];

        shape.get_interval(ref xInterval, ref yInterval, ref zInterval);

        for (int i = 0; i <= sample.x; i++)
        {
            for (int j = 0; j <= sample.y; j++)
            {
                for (int k = 0; k <= sample.z; k++)
                {
                    position[i, j, k] = new Vector3(xInterval[0] + (float)i * (xInterval[1] - xInterval[0]) / sample.x, yInterval[0] + (float)j * (yInterval[1] - yInterval[0]) / sample.y, zInterval[0] + (float)k * (zInterval[1] - zInterval[0]) / sample.z);
                    field[i, j, k] = shape.get_distance(position[i, j, k]);
                }
            }
        }

        GetComponent<MeshFilter>().mesh = MeshGenerator.GenerateMesh(position, field, 0f);
        GetComponent<MeshRenderer>().material = defaultMat;
    }

    public Vector3 getSize()
    {
        return new Vector3(xInterval.y - xInterval.x, yInterval.y - yInterval.x, zInterval.y - zInterval.x);
    }

    public Tuple<Vector2, Vector2, Vector2> getInterval()
    {
        return new Tuple<Vector2, Vector2, Vector2>(xInterval, yInterval, zInterval);
    }
}
