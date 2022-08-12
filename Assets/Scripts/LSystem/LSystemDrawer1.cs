using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSystemDrawer1
{
    private Vector3 position;
    private Quaternion rotation;

    Stack<(Vector3 position, Quaternion rotation)> positionStack;

    private float drawDistance = 1f;

    public LSystemDrawer1()
    {
        position = Vector3.zero;
        rotation = Quaternion.identity;

        positionStack = new Stack<(Vector3 position, Quaternion rotation)>();
    }

    public void Draw_F(Dictionary<string, double> ctx, Dictionary<string, double> globalCtx)
    {
        Vector3 dest = position + rotation * Vector3.up * drawDistance;
        Debug.DrawLine(position, dest, Color.white, 10f);
        position = dest;
    }
    public void Draw_F1(Dictionary<string, double> ctx, Dictionary<string, double> globalCtx)
    {
        Vector3 dest = position + rotation * Vector3.up * drawDistance;
        Debug.DrawLine(position, dest, Color.white, 10f);
        position = dest;
    }
    public void Draw_F2(Dictionary<string, double> ctx, Dictionary<string, double> globalCtx)
    {
        Vector3 dest = position + rotation * Vector3.up * drawDistance;
        Debug.DrawLine(position, dest, Color.white, 10f);
        position = dest;
    }
    public void Draw_Push(Dictionary<string, double> ctx, Dictionary<string, double> globalCtx)
    {
        positionStack.Push((position, rotation));
    }
    public void Draw_Pop(Dictionary<string, double> ctx, Dictionary<string, double> globalCtx)
    {
        (Vector3 position, Quaternion rotation) x = positionStack.Pop();
        position = x.position;
        rotation = x.rotation;
    }
    public void Draw_Lrot(Dictionary<string, double> ctx, Dictionary<string, double> globalCtx)
    {
        rotation *= Quaternion.Euler((float)globalCtx["turn_angle"], 0f, 0f);
    }

    public void Draw_Rrot(Dictionary<string, double> ctx, Dictionary<string, double> globalCtx)
    {
        rotation *= Quaternion.Euler(-(float)globalCtx["turn_angle"], 0f, 0f);
    }
}
