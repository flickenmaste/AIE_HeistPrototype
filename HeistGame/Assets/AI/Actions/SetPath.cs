using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class SetPath : RAINAction
{
	public GameObject Enemy;

    public override void Start(RAIN.Core.AI ai)
    {
		Enemy.GetComponent<RAIN.Core.AIRig>().AI.WorkingMemory.SetItem("varPath", "Path");

        base.Start(ai);
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}