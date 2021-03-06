using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCheckHandler : MonoBehaviour
{

    //Used for Collision between 2 Spheres
    public static bool Sphere_SphereCollision(SphereCollisionType sphere1, SphereCollisionType sphere2)
    {
        //Get the positions of the two spheres being compared
        Vector3 pos1 = sphere1.transform.position;
        Vector3 pos2 = sphere2.transform.position;

        //Determine the distance between them as a vector
        Vector3 distanceVector = pos2 - pos1;

        //Get the magnitude of this distance. This will determine the total distance between the middle of the two spheres
        float magnitude = Mathf.Sqrt(Mathf.Pow(distanceVector.x, 2) + Mathf.Pow(distanceVector.y, 2) + Mathf.Pow(distanceVector.z, 2));

        //Get the sum of the radiuses of the two spheres in question. This sum represents the distance if the two spheres were directly beside each other
        float radiusSum = sphere1.radius + sphere2.radius;

        //If the determined distance is less than or equal to the radius sum (aka the distance when both spheres are directly beside each other), a collision is occuring
        return magnitude <= radiusSum;
    }

    //Used for Collision between a Sphere and a Plane
    public static bool Sphere_PlaneCollision(SphereCollisionType sphere, PlaneCollisionType plane)
    {
        Vector3 planeNormal = plane.GetPlaneNormal();
        float widthFromCenter = (plane.width / 2) + sphere.radius;
        float lengthFromCenter = (plane.length / 2) + sphere.radius;


        //Get the distance between any plane point and the center of the sphere
        Vector3 distanceVec = sphere.transform.position - plane.transform.position;

        //Check if the ball is within the plane's boundaries. If not, collision won't happen regardless of the dot product
        //Is it a YZ plane?
        if (planeNormal.x != 0)
        {
            if (Mathf.Abs(distanceVec.y) > widthFromCenter || Mathf.Abs(distanceVec.z) > lengthFromCenter)
                    return false;
        }
        //Is it an XY plane?
        else if (planeNormal.z != 0)
        {
            if (Mathf.Abs(distanceVec.y) > widthFromCenter || Mathf.Abs(distanceVec.x) > lengthFromCenter)
                return false;
        }
        //Is it an XZ plane?
        else
        {
            if (Mathf.Abs(distanceVec.x) > widthFromCenter || Mathf.Abs(distanceVec.z) > lengthFromCenter)
                return false;
        }

        //Use the dot product to determine the distance of the "shadow" of the sphere, and the sphere itself
        //We get the absolute value of it, since it does not matter when determining the overlap
        float magnitude = Mathf.Abs(Vector3.Dot(distanceVec, plane.GetPlaneNormal()));

        //if the distance between them is less than the radius, it means there is an overlap
        return (sphere.radius - magnitude > 0);
    }


    // This Functions exists for less confusion with names
    public static bool Plane_SphereCollision(PlaneCollisionType plane, SphereCollisionType sphere) { return Sphere_PlaneCollision(sphere, plane); }
}
