using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSystemDrawer2
{
    private Vector3 position;
    private Quaternion rotation;

    private Transform parent;

    private Transform branch;
    private Transform leaf;

    Stack<(Vector3 position, Quaternion rotation)> positionStack;

    //private float drawDistance = 1f;

    public LSystemDrawer2(object p1, object p2)
    {
        position = Vector3.zero;
        rotation = Quaternion.identity;

        parent = new GameObject().transform;

        branch = (Transform)p1;
        leaf = (Transform)p2;

        positionStack = new Stack<(Vector3 position, Quaternion rotation)>();
    }

    public void Draw_F(Dictionary<string, double> ctx, Dictionary<string, double> globalCtx)
    {
        Transform obj = Object.Instantiate(branch, position, rotation, parent);

        Vector3 scale = obj.transform.localScale;
        scale.Scale(new Vector3(1f, (float)ctx["x"], 1f));
        obj.transform.localScale = scale;

        position += rotation * Vector3.up * (float)ctx["x"];
        Object.Instantiate(leaf, position, rotation, parent);
    }
    public void Draw_F1(Dictionary<string, double> ctx, Dictionary<string, double> globalCtx)
    {
        Transform obj = Object.Instantiate(branch, position, rotation, parent);

        Vector3 scale = obj.transform.localScale;
        scale.Scale(new Vector3(1f, (float)ctx["p0"], 1f));
        obj.transform.localScale = scale;

        position += rotation * Vector3.up * (float)ctx["p0"];
    }

    public void Draw_Pu(Dictionary<string, double> ctx, Dictionary<string, double> globalCtx)
    {
        positionStack.Push((position, rotation));
    }
    public void Draw_Pup(Dictionary<string, double> ctx, Dictionary<string, double> globalCtx)
    {
        positionStack.Push((position, default(Quaternion)));
    }
    public void Draw_Pur(Dictionary<string, double> ctx, Dictionary<string, double> globalCtx)
    {
        positionStack.Push((default(Vector3), rotation));
    }
    public void Draw_Po(Dictionary<string, double> ctx, Dictionary<string, double> globalCtx)
    {
        (Vector3 position, Quaternion rotation) x = positionStack.Pop();
        position = x.position;
        rotation = x.rotation;
    }
    public void Draw_Pop(Dictionary<string, double> ctx, Dictionary<string, double> globalCtx)
    {
        (Vector3 position, Quaternion rotation) x = positionStack.Pop();
        position = x.position;
    }
    public void Draw_Por(Dictionary<string, double> ctx, Dictionary<string, double> globalCtx)
    {
        (Vector3 position, Quaternion rotation) x = positionStack.Pop();
        rotation = x.rotation;
    }

    public void Draw_R(Dictionary<string, double> ctx, Dictionary<string, double> globalCtx)
    {
        rotation *= Quaternion.Euler((float)ctx["p0"], (float)ctx["p1"], (float)ctx["p2"]);
    }
}
