using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class SetFocus : RAINAction
{
	public GameObject[] Cops;
	public GameObject Player;

    public override void Start(RAIN.Core.AI ai)
    {
		Cops = GameObject.FindGameObjectsWithTag ("Cop");
		Player = GameObject.FindGameObjectWithTag("Player");

		for (int i = 0; i < Cops.GetLength; i++)
		{
			Cops[i].GetComponent<RAIN.Core.AIRig>().AI.WorkingMemory.SetItem("varObjective", "Patrol");
		}

        base.Start(ai);
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
		for (int i = 0; i < Cops.GetLength; i++)
		{
			if (Cops[i].GetComponent<RAIN.Core.AIRig>().AI.WorkingMemory.GetItem("varPlayer") != null)
			{
				Cops[i].GetComponent<RAIN.Core.AIRig>().AI.WorkingMemory.SetItem("varObjective", "Attack");
			}

			if (Cops[i].GetComponent<RAIN.Core.AIRig>().AI.WorkingMemory.GetItem("varPlayer") == null)
			{
				Cops[i].GetComponent<RAIN.Core.AIRig>().AI.WorkingMemory.SetItem("varObjective", "Patrol");
			}
		}

        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}