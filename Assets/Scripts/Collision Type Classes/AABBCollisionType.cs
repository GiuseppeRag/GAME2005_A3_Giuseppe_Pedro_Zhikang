using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AABBCollisionType : CollisionType
{
    public bool autoSetWidth = true;
    public float width;
    public float height;
    public float length;

    // Start is called before the first frame update
    void Start()
    {
        if (autoSetWidth)
        {
            width = transform.localScale.x;
            height = transform.localScale.y;
            length = transform.localScale.z;
        }
    }

    // Get the dimensions of the AABB box
    public Vector3 GetSize()
    {
        return new Vector3(width, height, length);
    }

    public override TypeEnum GetType()
    {
        return TypeEnum.AABB;
    }

    // Get the vertex on the AABB box with the lowest values
    public Vector3 GetMinPoint()
    {
        return new Vector3(transform.position.x - (width / 2), transform.position.y - (height / 2), transform.position.z - (length / 2));
    }

    // Get the vertex on the AABB box with the highest values
    public Vector3 GetMaxPoint()
    {
        return new Vector3(transform.position.x + (width / 2), transform.position.y + (height / 2), transform.position.z + (length / 2));
    }
}
