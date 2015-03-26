using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;
using RAIN.Navigation;
using RAIN.Navigation.Graph;
using RAIN.Navigation.Waypoints;

[RAINAction]
public class MakeSquad : RAINAction
{
	private string Formation;
	
	private int SquadSize;
	
	private string SquadName;

	public GameObject Cop;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public override void Start(RAIN.Core.AI ai)
    {

        //Just some preliminary setting stuff
		Formation = ai.WorkingMemory.GetItem<string>("String_Formation");
		
		SquadSize = ai.WorkingMemory.GetItem<int>("Int_SquadSize");
		
		SquadName = ai.WorkingMemory.GetItem<string>("String_SquadName");

		base.Start(ai);

    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public override ActionResult Execute(RAIN.Core.AI ai)
    {

        //Get the Cop prefab for clonning
		Cop = ai.WorkingMemory.GetItem<GameObject> ("Obj_CopPrefab");
		GameObject AIObj = ai.Body.gameObject;

        //spawn them spaced out (no flying cop circus)
		Vector3 Spacing = ai.Body.transform.position;

		Spacing.z += 0.5f;

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //make the clones (different names because there was problems)
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		GameObject FirstCop = (GameObject)GameObject.Instantiate(Cop, Spacing, Quaternion.Euler(new Vector3(5, 0, 0)));

		FirstCop.tag = SquadName + "Cop";

		Spacing.z += 1.5f;

		GameObject SecondCop = (GameObject)GameObject.Instantiate(Cop, Spacing, Quaternion.Euler(new Vector3(5, 0, 0)));
		
		SecondCop.tag = SquadName + "Cop";

		Spacing.z += 1.5f;

		GameObject ThirdCop = (GameObject)GameObject.Instantiate(Cop, Spacing, Quaternion.Euler(new Vector3(5, 0, 0)));
		
		ThirdCop.tag = SquadName + "Cop";

		Spacing.z += 1.5f;

		GameObject FourthCop = (GameObject)GameObject.Instantiate(Cop, Spacing, Quaternion.Euler(new Vector3(5, 0, 0)));
		
		FourthCop.tag = SquadName + "Cop";

		Spacing.z += 1.5f;

		GameObject FifthCop = (GameObject)GameObject.Instantiate(Cop, Spacing, Quaternion.Euler(new Vector3(5, 0, 0)));
		
		FifthCop.tag = SquadName + "Cop";

		GameObject[] SquadCop = GameObject.FindGameObjectsWithTag(SquadName + "Cop");


        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //now we need to do some setting for the line formation (because problems)
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		float CopSpacing = 0.5f;

		foreach (var cop in SquadCop)
		{
			cop.GetComponent<RAIN.Core.AIRig> ().AI.WorkingMemory.SetItem ("Obj_MovePoint", AIObj);
			
			cop.GetComponent<RAIN.Core.AIRig> ().AI.Motor.CloseEnoughDistance = CopSpacing;

			CopSpacing += 1.5f;
		}

        //making sure things are set and making sure this only happens once
		ai.WorkingMemory.SetItem ("Int_FormationSet", 0);
		ai.WorkingMemory.SetItem ("Int_DoOnce", 1);

        return ActionResult.SUCCESS;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public override void Stop(RAIN.Core.AI ai)
    {
        //found nothing, congrats
        base.Stop(ai);
    }
}