using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class UseCop : RAINAction
{

	RAIN.Core.AI Tim;
    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
		Tim = ai.WorkingMemory.GetItem<RAIN.Core.AI> ("Cop");
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
		Tim.WorkingMemory.SetItem("varPath", "PatrolPath");

        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}