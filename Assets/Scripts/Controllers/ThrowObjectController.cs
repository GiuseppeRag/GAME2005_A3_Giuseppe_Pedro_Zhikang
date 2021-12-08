using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowObjectController : MonoBehaviour
{
    public GameObject SpawnPrefab;
    private Vector3 Dir;
    private float StartingVel;

    // Start is called before the first frame update
    void Start()
    {
        Dir = transform.forward;
        StartingVel = 100;
    }

    // Update is called once per frame
    void Update()
    {
        Dir = transform.forward;

        if (Input.GetKeyDown("space"))
        {
            Debug.DrawRay(transform.localPosition, Dir, Color.red, 10000);
            GameObject temp = Instantiate(SpawnPrefab, transform.localPosition, transform.rotation);
            temp.GetComponent<CustomPhysicsObject>().velocity = new Vector3 (Dir.x * StartingVel, Dir.y * StartingVel, StartingVel );

            //print("space key was pressed");

        }
    }
}
