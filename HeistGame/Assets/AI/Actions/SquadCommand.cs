using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;
using RAIN.Navigation;
using RAIN.Navigation.Graph;
using RAIN.Navigation.Waypoints;

[RAINAction]
public class SquadCommand : RAINAction
{
	private GameObject[] Squad;

	private string Formation;
	
	private int SquadSize;
	
	private string SquadName;

	private Vector3[] FormationMaker;

	private string[] Positions = new string[5];

	void FormationTriangle(int SquadPlace)
	{
	
	}

	void FormationBox(int SquadPlace)
	{
		
	}

	void FormationLine(int SquadPlace, RAIN.Core.AI ai)
	{
		if (SquadPlace == 0)
						FormationMaker.SetValue (ai.Body.rigidbody.position, SquadPlace);
				else {
			Vector3 NewPos;
			NewPos.x = ai.Body.transform.position.x + 5 * (SquadPlace * (-1 * (SquadPlace % 2)) - (-1 * (SquadPlace % 2)));
			NewPos.y = ai.Body.transform.position.y;
			NewPos.z = ai.Body.transform.position.z;
						FormationMaker.SetValue (NewPos, SquadPlace);
				}

		GameObject PlaceSetter = GameObject.FindGameObjectWithTag (Positions [SquadPlace]);

		PlaceSetter.transform.position = FormationMaker [SquadPlace];
		//position.x + spacesize * (SquadPlace * (-1 * SquadPlace % 2) - (1 * SquadPlace % 2)) 

	}

    public override void Start(RAIN.Core.AI ai)
    {
		base.Start(ai);

		Formation = ai.WorkingMemory.GetItem<string>("varFormation");

		SquadSize = ai.WorkingMemory.GetItem<int>("varSquadSize");

		SquadName = ai.WorkingMemory.GetItem<string>("varSquadName");

		FormationMaker = new Vector3[SquadSize];

		Squad = GameObject.FindGameObjectsWithTag(SquadName);

		Positions[0] = SquadName + "One";
		Positions[1] = "SquadAlphaTwo";
		Positions[2] = SquadName + "Three";
		Positions[3] = SquadName + "Four";
		Positions[4] = SquadName + "Five";

		for(int i = 0; i < SquadSize; i++)
		{
			FormationLine(i, ai);
		}

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