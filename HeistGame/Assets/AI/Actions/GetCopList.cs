using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class GetCopList : RAINAction
{
	//private List<GameObject> Cops = new List<GameObject>();
	public GameObject[] Cops;	

    public override void Start(RAIN.Core.AI ai)
    {
		Cops = GameObject.FindGameObjectsWithTag("Cop");

		foreach (var cop in Cops)
		{
			//if(cop != null)
			//cop.GetComponent<RAIN.Core.AIRig>().AI.WorkingMemory.SetItem("varPath", "PatrolPath");
		}

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