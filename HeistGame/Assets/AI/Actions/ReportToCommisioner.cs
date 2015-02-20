using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class ReportToCommisioner : RAINAction
{
	public GameObject Commander;

    public override void Start(RAIN.Core.AI ai)
    {
		Commander = GameObject.FindGameObjectWithTag ("Commisioner");

        base.Start(ai);
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
		Commander.GetComponent<RAIN.Core.AIRig> ().AI.WorkingMemory.SetItem ("varAlert", "PlayerPosition");

        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}