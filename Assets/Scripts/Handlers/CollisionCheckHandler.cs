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

    // Used for collision checking between A Sphere and an AABB object
    public static bool Sphere_AABBCollision(SphereCollisionType sphere, AABBCollisionType aabb)
    {
        //Clamp the sphere to the closest point on the box
        float xClamp = Mathf.Max(aabb.GetMinPoint().x, Mathf.Min(sphere.transform.position.x, aabb.GetMaxPoint().x));
        float yClamp = Mathf.Max(aabb.GetMinPoint().y, Mathf.Min(sphere.transform.position.y, aabb.GetMaxPoint().y));
        float zClamp = Mathf.Max(aabb.GetMinPoint().z, Mathf.Min(sphere.transform.position.z, aabb.GetMaxPoint().z));

        //get the distance between that point and the sphere
        float magnitude = Mathf.Sqrt(Mathf.Pow(xClamp - sphere.transform.position.x, 2) +
                                     Mathf.Pow(yClamp - sphere.transform.position.y, 2) +
                                     Mathf.Pow(zClamp - sphere.transform.position.z, 2));

        //If the distance is less than the radius, the sphere is intersecting
        return magnitude < sphere.radius;
    }

    // Used for collision checking between a Plane and an AABB object
    public static bool Plane_AABBCollision(PlaneCollisionType plane, AABBCollisionType aabb)
    {
        //Get a vector that represents how far out the box extends from the center
        Vector3 extents = aabb.GetMaxPoint() - aabb.transform.position;

        //Get the projection of the closest point on the box with the plane's normal
        float projection = (extents.x * Mathf.Abs(plane.GetPlaneNormal().x)) +
                           (extents.y * Mathf.Abs(plane.GetPlaneNormal().y)) +
                           (extents.z * Mathf.Abs(plane.GetPlaneNormal().z));

        //perform the dot product on the box and the plane
        float dot = Vector3.Dot(plane.GetPlaneNormal(), aabb.transform.position) + plane.GetConstant();

        //check if the resultant value is less than the projection value. if so, it means there's a collision
        return Mathf.Abs(dot) <= projection;
    }

    // Used for collision checking between 2 AABB objects
    public static bool AABB_AABBCollision(AABBCollisionType aabb1, AABBCollisionType aabb2)
    {
        return ((aabb1.GetMinPoint().x <= aabb2.GetMaxPoint().x && aabb1.GetMaxPoint().x >= aabb2.GetMinPoint().x) &&
                (aabb1.GetMinPoint().y <= aabb2.GetMaxPoint().y && aabb1.GetMaxPoint().y >= aabb2.GetMinPoint().y) &&
                (aabb1.GetMinPoint().z <= aabb2.GetMaxPoint().z && aabb1.GetMaxPoint().z >= aabb2.GetMinPoint().z));
    }


    // These Functions exists for less confusion with names
    public static bool Plane_SphereCollision(PlaneCollisionType plane, SphereCollisionType sphere) { return Sphere_PlaneCollision(sphere, plane); }

    public static bool AABB_SphereCollision(AABBCollisionType aabb, SphereCollisionType sphere) { return Sphere_AABBCollision(sphere, aabb); }

    public static bool AABB_PlaneCollision(AABBCollisionType aabb, PlaneCollisionType plane) { return Plane_AABBCollision(plane, aabb); }
}
