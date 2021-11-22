using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomPhysicsObject : MonoBehaviour
{
    public float mass;
    public Vector3 velocity = Vector3.zero;
    public Vector3 acceleration = Vector3.zero;
    public bool useSystemGravity = true;
    public bool autoDetermineMass = false;

    public CollisionType collisionType;
    bool hasCollision = false;

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<CustomPhysicsSystem>().objectsList.Add(this);

        //No use right now, perhaps later
        if (autoDetermineMass)
        {
            switch (collisionType.GetType())
            {
                case TypeEnum.SPHERE:
                    SphereCollisionType sphere = (SphereCollisionType)collisionType;

                    mass = (float)((4 / 3) * 3.14 * Mathf.Pow(sphere.radius, 3));
                    break;
            }
        }
    }

    public bool UseSystemGravity()
    {
        return useSystemGravity;
    }

    public bool HasCollision()
    {
        return hasCollision;
    }

    public void SetHasCollision(bool flag)
    {
        hasCollision = flag;
    }
}
