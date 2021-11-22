using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereCollisionType : CollisionType
{
    public float radius;
    public override TypeEnum GetType()
    {
        return TypeEnum.SPHERE;
    }
}
