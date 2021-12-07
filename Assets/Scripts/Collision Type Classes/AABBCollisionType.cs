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

    public override TypeEnum GetType()
    {
        return TypeEnum.AABB;
    }

    public Vector3 GetMinPoint()
    {
        return new Vector3(transform.position.x - (width / 2), transform.position.y - (height / 2), transform.position.z - (length / 2));
    }

    public Vector3 GetMaxPoint()
    {
        return new Vector3(transform.position.x + (width / 2), transform.position.y + (height / 2), transform.position.z + (length / 2));
    }

    public Vector3 GetUpNormal()
    {
        return transform.up;
    }

    public Vector3 GetRightNormal()
    {
        return transform.right;
    }

    public Vector3 GetFrontNormal()
    {
        return transform.forward;
    }

    public Vector3 GetDownNormal()
    {
        return new Vector3(transform.up.x * -1, transform.up.y * -1, transform.up.z * -1);
    }

    public Vector3 GetLeftNormal()
    {
        return new Vector3(transform.right.x * -1, transform.right.y * -1, transform.right.z * -1);
    }

    public Vector3 GetBackNormal()
    {
        return new Vector3(transform.forward.x * -1, transform.forward.y * -1, transform.forward.z * -1);
    }

}
