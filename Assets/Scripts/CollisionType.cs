using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeEnum
{
    SPHERE = 0,
    PLANE = 1
}
public abstract class CollisionType : MonoBehaviour
{
    public abstract TypeEnum GetType();
}
