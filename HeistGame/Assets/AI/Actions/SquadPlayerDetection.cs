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

	private bool PLayerSpotted;

    public override void Start(RAIN.Core.AI ai)
    {
		SquadName = ai.WorkingMemory.GetItem<string> ("varSquadName");

		CopList = GameObject.FindGameObjectsWithTag (SquadName + "Cop");

        base.Start(ai);
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
		Vector3 AiFix = new Vector3();

		AiFix.Set (ai.Body.transform.position.x, CopList [0].transform.position.y, ai.Body.transform.position.z);
		ai.Kinematic.Position = AiFix;

		ai.WorkingMemory.SetItem("varPlayer", (GameObject)null);
		
		foreach (var cop in CopList)
		{
			GameObject IsPlayer = cop.GetComponent<RAIN.Core.AIRig>().AI.WorkingMemory.GetItem<GameObject>("varPlayer");

			if(IsPlayer != null)
			{
				ai.WorkingMemory.SetItem("varPlayer", IsPlayer);
				ai.WorkingMemory.SetItem("varLastPlayerPos", IsPlayer.transform.position);
				ai.WorkingMemory.SetItem("varLastPlayerDirection", IsPlayer.transform.position);
				ai.WorkingMemory.SetItem("varBreak", 1);
				break;
			}

			ai.WorkingMemory.SetItem("varPlayer", IsPlayer);
		}

        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}