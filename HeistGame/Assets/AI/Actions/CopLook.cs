using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class CopLook : RAINAction
{
    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
		Vector3 LookTarget = ai.WorkingMemory.GetItem<Vector3>("varLookPoint");

		ai.Body.transform.rotation = Quaternion.Slerp(ai.Body.transform.rotation, Quaternion.LookRotation(LookTarget - ai.Body.transform.position), 5*Time.deltaTime);

        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}