using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class ShootPlayer : RAINAction
{
    // Shooting
    float fireRate = 1.0f;
    private float lastShot = 0.0f;

    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        RaycastHit Hit;	// Store raycast info

        Vector3 DirectionRay = BulletSpread(ai);
        
        if (Time.time > fireRate + lastShot)
        {
            Debug.DrawRay(ai.Body.transform.position, DirectionRay * 1000.0f, Color.red);	// Ray so we can see

            if (Physics.Raycast(ai.Body.transform.position, DirectionRay, out Hit, 1000.0f))
            {	// Check for ray hit
                //Quaternion hitRotation = Quaternion.FromToRotation(Vector3.up, Hit.normal);
                if (Hit.transform.tag == "Player")
                {

                }

                if (Hit.transform.gameObject.layer == 10)
                {
                    Hit.transform.gameObject.GetComponent<Player>().TakeDamage(50); // Damage Player
                }
            }
            lastShot = Time.time + fireRate;
        }

        return ActionResult.SUCCESS;
    }

    Vector3 BulletSpread(RAIN.Core.AI ai)
    {
        // Spread for shooting
        float ShotSpread = 0.1f;

        float sprayX = (1 - 2 * Random.value) * ShotSpread;
        float sprayY = (1 - 2 * Random.value) * ShotSpread;
        float sprayZ = 1.0f;
        return ai.Body.transform.TransformDirection(sprayX, sprayY, sprayZ);
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}