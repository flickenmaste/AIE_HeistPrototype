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

    public override void Start(RAIN.Core.AI ai)
    {
		Formation = ai.WorkingMemory.GetItem<string>("varFormation");
		
		SquadSize = ai.WorkingMemory.GetItem<int>("varSquadSize");
		
		SquadName = ai.WorkingMemory.GetItem<string>("varSquadName");

		base.Start(ai);

    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
		Cop = ai.WorkingMemory.GetItem<GameObject> ("varCopPrefab");
		GameObject AIObj = ai.Body.gameObject;

		Vector3 Spacing = ai.Body.transform.position;

		Spacing.z += 0.5f;

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

		float CopSpacing = 0.5f;

		foreach (var cop in SquadCop)
		{
			cop.GetComponent<RAIN.Core.AIRig> ().AI.WorkingMemory.SetItem ("varMainMovePoint", AIObj);
			
			cop.GetComponent<RAIN.Core.AIRig> ().AI.Motor.CloseEnoughDistance = CopSpacing;

			CopSpacing += 1.5f;
		}

		ai.WorkingMemory.SetItem ("varFormationSet", 0);
		ai.WorkingMemory.SetItem ("varDoOnce", 1);

        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}