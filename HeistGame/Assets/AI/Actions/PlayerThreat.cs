using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class PlayerThreat : RAINAction
{
	//used to access the PhaseManager
	public GameObject Phase;

	//used to determine the threat level of the player
	public string PlayerState;

    public override void Start(RAIN.Core.AI ai)
    {
		Phase = GameObject.FindGameObjectWithTag("PhaseManager");

        base.Start(ai);
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
		PlayerState = Phase.GetComponent<PhaseManager>().PhaseQueue.Peek().ToString();

		if (PlayerState == "Execution")
		{
			return ActionResult.SUCCESS;
		}
		else
		{
			return ActionResult.FAILURE;
		}
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}