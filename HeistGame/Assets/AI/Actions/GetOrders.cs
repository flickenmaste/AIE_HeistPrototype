using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class GetOrders : RAINAction
{
	public GameObject Commander;

    public override void Start(RAIN.Core.AI ai)
    {
		Commander = GameObject.FindGameObjectWithTag("Commisioner");

        base.Start(ai);
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
		if (Commander.GetComponent<RAIN.Core.AIRig> ().AI.WorkingMemory.GetItem ("varOrder") == "Attack")
		{

		}
		else
		{

		}

        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}