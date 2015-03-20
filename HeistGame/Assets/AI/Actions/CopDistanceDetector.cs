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

        Vector3 Target = ai.WorkingMemory.GetItem<GameObject>("varMainMovePoint").transform.position;

        if (Vector3.Distance(Target, ai.Body.transform.position) < 0.3f)
            ai.WorkingMemory.SetItem("varReadyToMove", 1);
        else
            ai.WorkingMemory.SetItem("varReadyToMove", 0);
            

        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}