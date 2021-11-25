using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneCollisionType : CollisionType
{
    public float length;
    public float width;
    public override TypeEnum GetType()
    {
        return TypeEnum.PLANE;
    }

    public Vector3 GetPlaneNormal()
    {
        return transform.up;
    }
}
