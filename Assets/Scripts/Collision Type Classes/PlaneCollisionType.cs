using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneCollisionType : CollisionType
{
    //Will be used for Plane-Sphere collision
    public float length;
    public float width;
    public override TypeEnum GetType()
    {
        return TypeEnum.PLANE;
    }

    //Return the normal of the plane
    public Vector3 GetPlaneNormal()
    {
        return transform.up;
    }
}
