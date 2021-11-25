using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollisionHandler : MonoBehaviour
{
    public static void OnSphere_SphereCollision(CustomPhysicsObject sphere1, CustomPhysicsObject sphere2)
    {
        //Cast spheres.
        SphereCollisionType sphereCollision1 = (SphereCollisionType)sphere1.collisionType;
        SphereCollisionType sphereCollision2 = (SphereCollisionType)sphere2.collisionType;

        Vector3 displacementBetweenSpheres = sphere2.transform.position - sphere1.transform.position;
        float distanceBetween = displacementBetweenSpheres.magnitude;
        float sumRad = sphereCollision1.radius + sphereCollision2.radius;
        float penetration = sumRad - distanceBetween;
        bool isOverlapping = penetration > 0.0f;

        if (!isOverlapping)
            return; // No collision.

        // Normal for the collision.
        Vector3 collisionNormal = displacementBetweenSpheres / distanceBetween;
        
        // Move spheres to stop overlapping.
        Vector3 minimumTranslationVector = collisionNormal * penetration;
        Vector3 translationA = minimumTranslationVector * -0.5f;
        Vector3 translationB = minimumTranslationVector * 0.5f;

        sphere1.transform.position += translationA;
        sphere2.transform.position += translationB;
    }

    public static void OnSphere_PlaneCollision(CustomPhysicsObject sphere, CustomPhysicsObject plane)
    {
        // Due to reason I can't just put them on a single line
        // So I have to cast them then use them
        PlaneCollisionType planeCollision = (PlaneCollisionType)plane.collisionType;
        SphereCollisionType sphereCollision = (SphereCollisionType)sphere.collisionType;

        Vector3 planeNormal = planeCollision.GetPlaneNormal();

        // Stop the velocity on the appropriate axis depending on the plane's normal
        if (planeNormal.x != 0)
            sphere.velocity.x = 0.0f;
        else if (planeNormal.z != 0)
            sphere.velocity.z = 0.0f;
        else
            sphere.velocity.y = 0.0f;

        // Perform the displacement of the sphere so it does not penetrate the plane
        Vector3 displacement = sphere.transform.position - plane.transform.position;
        float dotProduct = Vector3.Dot(planeNormal, displacement);
        float distance = Mathf.Abs(dotProduct);
        float penetration = sphereCollision.radius - distance;

        sphere.transform.position += planeNormal * penetration;
    }

    public static void OnPlane_SphereCollision(CustomPhysicsObject plane, CustomPhysicsObject sphere) { OnSphere_PlaneCollision(sphere, plane); }
}
