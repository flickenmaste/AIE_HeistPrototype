using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class SetOrders : RAINAction
{
	public GameObject[] Cops;
	public GameObject Commander;

    public override void Start(RAIN.Core.AI ai)
    {
		Cops = GameObject.FindGameObjectsWithTag("Cop");
		Commander = GameObject.FindGameObjectWithTag("Commiosioner");

        base.Start(ai);
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
		for (int i = 0; i < Cops.Length; i++)
		{
			if (Cops[i].GetComponent<RAIN.Core.AIRig>().AI.WorkingMemory.GetItem("varPlayer") == "1")
			{
				Commander.GetComponent<RAIN.Core.AIRig>().AI.WorkingMemory.SetItem("varOder", "AttackOrder");
			}
			else
			{
				Commander.GetComponent<RAIN.Core.AIRig>().AI.WorkingMemory.SetItem("varOder", "PatrolOrder");
			}
		}

        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}