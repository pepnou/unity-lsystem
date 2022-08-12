using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    [SerializeField] Vector3 rot = new Vector3(0,10,0);
    public void RotateFromLeft()
    {
        transform.rotation = Quaternion.Euler(rot) * transform.rotation;
    }

    public void RotateFromRight()
    {
        transform.rotation =  transform.rotation * Quaternion.Euler(rot);
    }
}
