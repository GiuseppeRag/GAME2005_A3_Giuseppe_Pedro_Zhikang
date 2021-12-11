using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowObjectController : MonoBehaviour
{
    public GameObject SpawnPrefab;
    public float StartingVelocity = 10;
    //private List<GameObject> CloneList = new List<GameObject>();
    private Vector3 Dir;

    // Start is called before the first frame update
    void Start()
    {
        Dir = transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        Dir = transform.forward;

        if (Input.GetKeyDown("space"))
        {
            //Debug.DrawRay(transform.localPosition, Dir, Color.red, 10000);
            GameObject temp = Instantiate(SpawnPrefab, transform.localPosition, transform.rotation);
            temp.tag = "DeleteWhenReset";
            temp.GetComponent<CustomPhysicsObject>().velocity = new Vector3 (Dir.x * StartingVelocity, Dir.y * StartingVelocity,  Dir.z * StartingVelocity);
        }
    }
}
