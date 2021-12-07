using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum TypeEnum
{
    SPHERE = 0,
    PLANE = 1,
    AABB = 2
}

[RequireComponent(typeof(CustomPhysicsObject))]
public abstract class CollisionType : MonoBehaviour
{
    public abstract TypeEnum GetType();
}
