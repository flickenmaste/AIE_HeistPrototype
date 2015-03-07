using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class C_Shoot : MonoBehaviour
{
    public float range = 1000.0f;
    public float ShotSpread = 0.03f;
    public GameObject defaultHolePrefab;

    // Shooting
    float fireRate = 1.0f;
    private float lastShot = 0.0f;

    public int MaxShots = 15;

    // Managers
    public GameObject EscaMan;
    public GameObject PhaseMan;

    // Crosshair
    public Texture2D cross1;
    public Texture2D cross2;
    public Texture2D cross3;
    public Texture2D cross4;

    // Audio
    public AudioClip GunShot;
    public AudioClip Reload;

    // Use this for initialization
    void Start()
    {
        EscaMan = GameObject.FindGameObjectWithTag("EscalationManager");
        PhaseMan = GameObject.FindGameObjectWithTag("PhaseManager");
    }

    // Update is called once per frame
    void Update()
    {

        if (PhaseMan.gameObject.GetComponent<PhaseManager>().PhaseQueue.Peek().ToString() == "Execution")
        {
            if (Input.GetMouseButtonDown(0) && MaxShots >= 1)
                Shoot();

            if (Input.GetKeyUp(KeyCode.R) && MaxShots < 15)
            {
                AudioSource.PlayClipAtPoint(Reload, this.gameObject.transform.position);
                MaxShots = 15;
            }
        }
    }

    void Shoot()
    {
        RaycastHit Hit;	// Store raycast info

        Vector3 DirectionRay = BulletSpread();	// Set bullet spread

        Debug.DrawRay(this.transform.position, DirectionRay * range, Color.green);	// Ray so we can see

        if (Time.time > fireRate + lastShot)
        {
            if (Physics.Raycast(transform.position, DirectionRay, out Hit, range))
            {	// Check for ray hit
                Quaternion hitRotation = Quaternion.FromToRotation(Vector3.up, Hit.normal);
                if (Hit.transform.tag == "Untagged")
                {
                    GameObject defaultHole = Instantiate(defaultHolePrefab, Hit.point, hitRotation) as GameObject;
                    defaultHole.transform.parent = Hit.transform;
                    defaultHole.transform.position = new Vector3(defaultHole.transform.position.x, defaultHole.transform.position.y + 0.01f, defaultHole.transform.position.z);
                    Destroy(defaultHole, 3);
                }

                if (Hit.transform.gameObject.layer == 8)
                {
                    if (Hit.transform.gameObject.tag == "Cop")
                        Hit.transform.gameObject.GetComponent<BasicCop>().TakeDamage(50);
                    if (Hit.transform.gameObject.tag == "Civilian")
                        Hit.transform.gameObject.GetComponent<BasicCivilian>().TakeDamage(50);
                    //Debug.Log("yay");
                    if (EscaMan.GetComponent<EscalationManager>().PhaseQueue.Count >= 1)
                        EscaMan.GetComponent<EscalationManager>().PhaseQueue.Dequeue(); // This is how you can escalate the phase
                }
            }
        }
        AudioSource.PlayClipAtPoint(GunShot, this.gameObject.transform.position);
        MaxShots += -1;
    }

    Vector3 BulletSpread()
    {
        // AIM and WALK - fuck shit - overrides the other shit
        // Check if mouse is moving and player is walking, fuck up spread even more
        if ((Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0) &&		// aim
            (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0))	// movement
        {
            //Debug.Log("AIM AND WALK");
            ShotSpread = 0.4f;
        }

            // AIM or WALK - fuck shit
        // Check if mouse is moving or player is walking, fuck up spread
        else if ((Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0) ||	// aim
                 (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0))	// movement
        {
            //Debug.Log("AIM or WALK");
            ShotSpread = 0.2f;
        }

        // NOTHING NOTHING - leave shit
        else // jack shit
        {
            //Debug.Log("STILL");
            ShotSpread = 0.03f;
        }



        float sprayX = (1 - 2 * Random.value) * ShotSpread;
        float sprayY = (1 - 2 * Random.value) * ShotSpread;
        float sprayZ = 1.0f;
        return this.transform.TransformDirection(sprayX, sprayY, sprayZ);
    }

    // Crosshair
    void OnGUI()
    {
        if (PhaseMan.gameObject.GetComponent<PhaseManager>().PhaseQueue.Peek().ToString() == "Execution")
        {
            if ((Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0) || // aim
                (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)) // movement
            {
                // Left
                GUI.DrawTexture(new Rect(0 + (Screen.width / 2) - 120, 0 + (Screen.height / 2) - 3, 64, 32), cross1, ScaleMode.StretchToFill, true, 10.0F);
                // Right
                GUI.DrawTexture(new Rect(0 + (Screen.width / 2) + 55, 0 + (Screen.height / 2) - 3, 64, 32), cross2, ScaleMode.StretchToFill, true, 10.0F);
                // Up
                GUI.DrawTexture(new Rect(0 + (Screen.width / 2) - 16, 0 + (Screen.height / 2) - 105, 32, 64), cross3, ScaleMode.StretchToFill, true, 10.0F);
                // Down
                GUI.DrawTexture(new Rect(0 + (Screen.width / 2) - 16, 0 + (Screen.height / 2) + 65, 32, 64), cross4, ScaleMode.StretchToFill, true, 10.0F);
            }

            else
            {
                // Left
                GUI.DrawTexture(new Rect(0 + (Screen.width / 2) - 100, 0 + (Screen.height / 2) - 3, 64, 32), cross1, ScaleMode.StretchToFill, true, 10.0F);
                // Right
                GUI.DrawTexture(new Rect(0 + (Screen.width / 2) + 35, 0 + (Screen.height / 2) - 3, 64, 32), cross2, ScaleMode.StretchToFill, true, 10.0F);
                // Up
                GUI.DrawTexture(new Rect(0 + (Screen.width / 2) - 16, 0 + (Screen.height / 2) - 85, 32, 64), cross3, ScaleMode.StretchToFill, true, 10.0F);
                // Down
                GUI.DrawTexture(new Rect(0 + (Screen.width / 2) - 16, 0 + (Screen.height / 2) + 45, 32, 64), cross4, ScaleMode.StretchToFill, true, 10.0F);
            }
        }
    }
}

