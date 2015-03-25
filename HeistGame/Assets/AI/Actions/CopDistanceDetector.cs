using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class CopDistanceDetector : RAINAction
{
    public override void Start(RAIN.Core.AI ai)
    {
        //yes it is
        base.Start(ai);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public override ActionResult Execute(RAIN.Core.AI ai)
    {

        //get the animator, the target, and the distance
        Animator CopAnim = ai.Body.GetComponentInChildren<Animator>();

        Vector3 Target = ai.WorkingMemory.GetItem<GameObject>("Obj_MovePoint").transform.position;

        float Distance = Vector3.Distance(Target, ai.Body.transform.position);


        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //this checks the distance, sets the animator speed accordingly, 
        //and tells the squad leader (indirectly) that the cop is ready to move
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        if (Distance >= 10.0f + ai.Motor.CloseEnoughDistance)
        {
            ai.WorkingMemory.SetItem("Int_ReadyToMove", 0);
            ai.Motor.DefaultSpeed = 10;
        }

        else
        {
            ai.WorkingMemory.SetItem("Int_ReadyToMove", 0);

            //use the distance and the "max speed" to get a percentage between 1 and zero
            ai.Motor.DefaultSpeed = ((Distance) / (10.0f + ai.Motor.CloseEnoughDistance)) * 10;

            //if that percentage is small enough, just tell the cop to stop
            if (ai.Motor.DefaultSpeed < 0.5f || Distance <= ai.Motor.CloseEnoughDistance - 0.1f)
                ai.Motor.DefaultSpeed = 0;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //set the cops animator walk speed to "match" the cops speed
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        if (ai.Motor.DefaultSpeed == 0)
        {
            ai.WorkingMemory.SetItem("Int_ReadyToMove", 1);
            CopAnim.SetFloat("Vertical", 0);
        }

        else
        {
            ai.WorkingMemory.SetItem("Int_ReadyToMove", 0);
            CopAnim.SetFloat("Vertical", (ai.Motor.DefaultSpeed) / 10);
        }
            
        return ActionResult.SUCCESS;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public override void Stop(RAIN.Core.AI ai)
    {
        //this is not a phone
        base.Stop(ai);
    }
}