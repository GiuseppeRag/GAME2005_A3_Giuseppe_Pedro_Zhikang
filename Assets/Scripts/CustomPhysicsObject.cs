using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomPhysicsObject : MonoBehaviour
{
    public float mass;
    public Vector3 velocity = Vector3.zero;
    public Vector3 acceleration = Vector3.zero;
    public bool useSystemGravity = true;

    public CollisionType collisionType;
    bool hasCollision = false;

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<CustomPhysicsSystem>().objectsList.Add(this);
    }

    public bool UseSystemGravity()
    {
        return useSystemGravity;
    }

    public bool HasCollision()
    {
        return hasCollision;
    }
}
