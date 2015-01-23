using UnityEngine;
using System.Collections;

public class C_Shoot : MonoBehaviour
{

    public float range = 1000.0f;
    public float ShotSpread = 0.03f;
    public GameObject defaultHolePrefab;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            Shoot();
    }

    void Shoot()
    {

        RaycastHit Hit;	// Store raycast info

        Vector3 DirectionRay = BulletSpread();	// Set bullet spread

        Debug.DrawRay(this.transform.position, DirectionRay * range, Color.green);	// Ray so we can see

        if (Physics.Raycast(transform.position, DirectionRay, out Hit, range))
        {	// Check for ray hit
            Quaternion hitRotation = Quaternion.FromToRotation(Vector3.up, Hit.normal);
            if (Hit.transform.tag == "Untagged")
            {
                GameObject defaultHole = Instantiate(defaultHolePrefab, Hit.point, hitRotation) as GameObject;
                defaultHole.transform.parent = Hit.transform;
                defaultHole.transform.position = new Vector3(defaultHole.transform.position.x, defaultHole.transform.position.y + 0.01f, defaultHole.transform.position.z + 0.01f);
                Destroy(defaultHole, 3);
            }
        }
    }

    Vector3 BulletSpread()
    {
        // AIM and WALK - fuck shit - overrides the other shit
        // Check if mouse is moving and player is walking, fuck up spread even more
        if ((Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0) &&		// aim
            (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0))	// movement
        {
            Debug.Log("AIM AND WALK");
            ShotSpread = 0.4f;
        }

            // AIM or WALK - fuck shit
        // Check if mouse is moving or player is walking, fuck up spread
        else if ((Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0) ||	// aim
                 (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0))	// movement
        {
            Debug.Log("AIM or WALK");
            ShotSpread = 0.2f;
        }

        // NOTHING NOTHING - leave shit
        else // jack shit
        {
            Debug.Log("STILL");
            ShotSpread = 0.03f;
        }



        float sprayX = (1 - 2 * Random.value) * ShotSpread;
        float sprayY = (1 - 2 * Random.value) * ShotSpread;
        float sprayZ = 1.0f;
        return this.transform.TransformDirection(sprayX, sprayY, sprayZ);
    }
}

