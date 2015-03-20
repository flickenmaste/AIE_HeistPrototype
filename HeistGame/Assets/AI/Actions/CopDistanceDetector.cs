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
        base.Start(ai);
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {

        Animator CopAnim = ai.Body.GetComponentInChildren<Animator>();

        Vector3 Target = ai.WorkingMemory.GetItem<GameObject>("varMainMovePoint").transform.position;

        float Distance = Vector3.Distance(Target, ai.Body.transform.position);

        //if (Distance < ai.Motor.CloseEnoughDistance - 0.1f)
        //{
        //    ai.WorkingMemory.SetItem("varReadyToMove", 1);
        //    ai.Motor.Speed = 0;
        //}
        if (Distance >= 10.0f + ai.Motor.CloseEnoughDistance)
        {
            ai.WorkingMemory.SetItem("varReadyToMove", 0);
            ai.Motor.DefaultSpeed = 10;
        }
        else
        {
            ai.WorkingMemory.SetItem("varReadyToMove", 0);
            ai.Motor.DefaultSpeed = ((Distance) / (10.0f + ai.Motor.CloseEnoughDistance)) * 10;

            if (ai.Motor.DefaultSpeed < 1.2f && Distance >= ai.Motor.CloseEnoughDistance)
                ai.Motor.DefaultSpeed = 1.2f;
        }

        if (Distance < ai.Motor.CloseEnoughDistance)
        {
            ai.WorkingMemory.SetItem("varReadyToMove", 1);
            CopAnim.SetFloat("Vertical", 0);
        }
        else
        {
            CopAnim.SetFloat("Vertical", (ai.Motor.DefaultSpeed) / 10);
            ai.WorkingMemory.SetItem("varReadyToMove", 0);
        }
            
        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}