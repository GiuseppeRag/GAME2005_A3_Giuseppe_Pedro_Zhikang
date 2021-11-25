using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollisionHandler : MonoBehaviour
{
    public static void OnSphere_SphereCollision(CustomPhysicsObject sphere1, CustomPhysicsObject sphere2)
    {
        
    }

    public static void OnSphere_PlaneCollision(CustomPhysicsObject sphere, CustomPhysicsObject plane)
    {
        // Due to reason I can't just put them on a single line
        // So I have to cast them then use them
        PlaneCollisionType planeCollision = (PlaneCollisionType)plane.collisionType;
        SphereCollisionType sphereCollision = (SphereCollisionType)sphere.collisionType;

        Vector3 planeNormal = planeCollision.GetPlaneNormal();

        if (planeNormal.x != 0)
        {
            sphere.velocity.x = 0.0f;
        }
        else if (planeNormal.z != 0)
        {
            sphere.velocity.z = 0.0f;
        }
        else
        {
            sphere.velocity.y = 0.0f;
        }

        // "It just works" -Tod Howard
        // For some reason when the velocity is more than 150 it doesn't work
        Vector3 displacement = sphere.transform.position - plane.transform.position;
        float dotProduct = Vector3.Dot(planeNormal, displacement);
        float distance = Mathf.Abs(dotProduct);
        float penetration = sphereCollision.radius - distance;

        sphere.transform.position += planeNormal * penetration;
    }

    public static void OnPlane_SphereCollision(CustomPhysicsObject plane, CustomPhysicsObject sphere) { OnSphere_PlaneCollision(sphere, plane); }
}
