using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    //TODO: camera smoothing

    public Transform Target;
    public float DistanceFromTarget;

    void LateUpdate()
    {
        transform.position = Target.position + Vector3.up * DistanceFromTarget;
    }
}
