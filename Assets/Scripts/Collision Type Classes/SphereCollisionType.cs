using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereCollisionType : CollisionType
{
    public bool autoSetRadius = true;
    public float radius;

    private void Start()
    {
        //auto sets the radius
        if (autoSetRadius)
            radius = transform.localScale.x / 2;
    }

    public override TypeEnum GetType()
    {
        return TypeEnum.SPHERE;
    }
}
