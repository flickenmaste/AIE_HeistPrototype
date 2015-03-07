using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class SquadPlayerDetection : RAINAction
{
	private GameObject[] CopList;

	private string SquadName;

    public override void Start(RAIN.Core.AI ai)
    {
		SquadName = ai.WorkingMemory.GetItem<string> ("varSquadName");

		CopList = GameObject.FindGameObjectsWithTag (SquadName + "Cop");

        base.Start(ai);
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
		Vector3 AiFix = new Vector3();

		bool IsPlayerFound = false;

		AiFix.Set (ai.Body.transform.position.x, CopList [0].transform.position.y, ai.Body.transform.position.z);
		ai.Kinematic.Position = AiFix;

		//ai.WorkingMemory.SetItem("varPlayer", (GameObject)null);
		
		foreach (var cop in CopList)
		{
			GameObject IsPlayer = cop.GetComponent<RAIN.Core.AIRig>().AI.WorkingMemory.GetItem<GameObject>("varPlayer");

			if(IsPlayer != (GameObject)null)
			{
				IsPlayerFound = true;
				ai.WorkingMemory.SetItem("varPlayer", IsPlayer);
				ai.WorkingMemory.SetItem("varLastPlayerPos", IsPlayer.transform.position);
				ai.WorkingMemory.SetItem("varBreak", 1);
				ai.WorkingMemory.SetItem ("varFormation", "Wedge");
				ai.Motor.Speed = 0.0f;
				ai.WorkingMemory.SetItem ("varFormationSet", 0);
				break;
			}


		}
		
		if (IsPlayerFound)
		{
			foreach (var cop in CopList)
			{
				cop.GetComponent<RAIN.Core.AIRig>().AI.WorkingMemory.SetItem("varPlayer", ai.WorkingMemory.GetItem<GameObject>("varPlayer"));
			}
		}

        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}