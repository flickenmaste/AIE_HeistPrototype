using UnityEngine;
using System.Collections;

public class HingeScriptClamp : MonoBehaviour
{

    private float speed = 25;
    private float currentRot;

    // Update is called once per frame
    void Update()
    {

        //Obtain mouse wheel input
        float mouseWheel = Input.GetAxis("Mouse ScrollWheel") * speed;

        if (mouseWheel != 0)
        {
            //Increment rotation by mouseWheel value
            currentRot = currentRot - mouseWheel;

            //Clamp it to between 0 and 45 degs (Thanks whydoidoit!!!)
            currentRot = ClampAngle(currentRot, 0, 45);

            //Apply rotation
            transform.localEulerAngles = new Vector3(currentRot, 0, 0);
        }

    }

    float ClampAngle(float angle, float min, float max)
    {
        bool inverse = false;
        var tmin = min;
        var tangle = angle;
        if (min > 180)
        {
            inverse = !inverse;
            tmin -= 180;
        }
        if (angle > 180)
        {
            inverse = !inverse;
            tangle -= 180;
        }
        var result = !inverse ? tangle > tmin : tangle < tmin;
        if (!result)
            angle = min;

        inverse = false;
        tangle = angle;
        var tmax = max;
        if (angle > 180)
        {
            inverse = !inverse;
            tangle -= 180;
        }
        if (max > 180)
        {
            inverse = !inverse;
            tmax -= 180;
        }

        result = !inverse ? tangle < tmax : tangle > tmax;
        if (!result)
            angle = max;
        return angle;
    }
}