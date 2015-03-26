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

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public override void Start(RAIN.Core.AI ai)
    {
        //get the list of cops for this squad
		SquadName = ai.WorkingMemory.GetItem<string> ("String_SquadName");

		CopList = GameObject.FindGameObjectsWithTag (SquadName + "Cop");

        base.Start(ai);
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {

        //Ths squad leaders Y is set by the first cop in the list (this is because the squad leader does not have collision)
		Vector3 AiFix = new Vector3();

		bool IsPlayerFound = false;

		AiFix.Set (ai.Body.transform.position.x, CopList [0].transform.position.y, ai.Body.transform.position.z);
		ai.Kinematic.Position = AiFix;

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Check if any cop has spotted the player, then tell all the other cops about it
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		foreach (var cop in CopList)
		{
			GameObject IsPlayer = cop.GetComponent<AIRig>().AI.WorkingMemory.GetItem<GameObject>("Obj_Player");

			if(IsPlayer != (GameObject)null)
			{
				IsPlayerFound = true;
				ai.WorkingMemory.SetItem("Obj_Player", IsPlayer);
				ai.WorkingMemory.SetItem("Vec3_LastPlayerPos", IsPlayer.transform.position);
				ai.WorkingMemory.SetItem("Int_Break", 1);
				ai.WorkingMemory.SetItem ("String_Formation", "Wedge");
				ai.WorkingMemory.SetItem ("Int_FormationSet", 1);
				break;
			}


		}

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //if there is a player in sight do somthing
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		if (IsPlayerFound)
		{
			foreach (var cop in CopList)
			{
				cop.GetComponent<AIRig>().AI.WorkingMemory.SetItem("Obj_SquadPlayer", ai.WorkingMemory.GetItem<GameObject>("Obj_Player"));
			}
		}

        //otherwise make sure everyone sees nothing
		else
        {
			foreach (var cop in CopList)
			{
				cop.GetComponent<AIRig>().AI.WorkingMemory.SetItem("Obj_SquadPlayer", (GameObject)null);
			}

			ai.WorkingMemory.SetItem("Obj_Player", (GameObject)null);
		}


        return ActionResult.SUCCESS;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public override void Stop(RAIN.Core.AI ai)
    {
        //Is good place, only 19.99
        base.Stop(ai);
    }
}