using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class SquadFormationHandler : RAINAction
{
	private GameObject[] SquadCopList;

	private string Formation;
	
	private int SquadSize;
	
	private string SquadName;
	
	private string[] Positions = new string[5];
	
	private Vector3 FormationManager;

	void FormationWedge(int SquadPlace, RAIN.Core.AI ai)
	{
		FormationBox (SquadPlace, ai);
	}

	void FormationBox(int SquadPlace, RAIN.Core.AI ai)
	{
		if (SquadPlace == 0) 
		{
			FormationManager = ai.Body.transform.position;
		}
		
		else if (SquadPlace == 1)
		{
			FormationManager.Set(ai.Body.transform.position.x + 1.5f, ai.Body.transform.position.y, ai.Body.transform.position.z + 1.5f);
		}
		
		else if (SquadPlace == 3)
		{
			FormationManager.Set(ai.Body.transform.position.x - 1.5f, ai.Body.transform.position.y, ai.Body.transform.position.z + 1.5f);
		}
		
		else if (SquadPlace == 2)
		{
			FormationManager.Set(ai.Body.transform.position.x + 1.5f, ai.Body.transform.position.y, ai.Body.transform.position.z - 1.5f);
		}
		
		else if (SquadPlace == 4)
		{
			FormationManager.Set(ai.Body.transform.position.x - 1.5f, ai.Body.transform.position.y, ai.Body.transform.position.z - 1.5f);
		}
		
		GameObject SquadSpot = GameObject.FindGameObjectWithTag (Positions [SquadPlace]);

		SquadSpot.GetComponent<Transform> ().position = FormationManager;

		//SquadSpot.transform.localPosition = ai.Body.transform.position;

		SquadCopList [SquadPlace].GetComponent<RAIN.Core.AIRig> ().AI.WorkingMemory.SetItem ("varMainMovePoint", SquadSpot);

		SquadCopList [SquadPlace].GetComponent<RAIN.Core.AIRig> ().AI.Motor.DefaultCloseEnoughDistance = 0.1f;
	}

	void FormationLine(int SquadPlace, RAIN.Core.AI ai)
	{
		GameObject SquadSpot = GameObject.FindGameObjectWithTag (Positions [SquadPlace]);

		FormationManager.Set (ai.Body.transform.position.x, ai.Body.transform.position.y, ai.Body.transform.position.z);

		//SquadSpot.transform.localPosition = ai.Body.transform.position;

		SquadSpot.GetComponent<Transform> ().position = FormationManager;

		SquadCopList[SquadPlace].GetComponent<RAIN.Core.AIRig> ().AI.WorkingMemory.SetItem ("varMainMovePoint", SquadSpot);
		SquadCopList[SquadPlace].GetComponent<RAIN.Core.AIRig> ().AI.Motor.DefaultCloseEnoughDistance = 1.5f * SquadPlace;
	}

    public override void Start(RAIN.Core.AI ai)
    {
		SquadCopList = GameObject.FindGameObjectsWithTag(ai.WorkingMemory.GetItem<string>("varSquadName") + "Cop");

		Formation = ai.WorkingMemory.GetItem<string>("varFormation");
		
		SquadSize = ai.WorkingMemory.GetItem<int>("varSquadSize");
		
		SquadName = ai.WorkingMemory.GetItem<string>("varSquadName");
		
		Positions[0] = SquadName + "One";
		Positions[1] = SquadName + "Two";
		Positions[2] = SquadName + "Three";
		Positions[3] = SquadName + "Four";
		Positions[4] = SquadName + "Five";

        base.Start(ai);
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
		Vector3 MoveLoc = ai.WorkingMemory.GetItem<Vector3>("varSeek");

		Vector3 TestLoc = ai.WorkingMemory.GetItem<Vector3>("varLastHeading");

		int FormationSet = ai.WorkingMemory.GetItem<int>("varFormationSet");

		bool ChangeTestLoc = false;

		if (MoveLoc != TestLoc && ai.WorkingMemory.GetItem<GameObject> ("varPlayer") == (GameObject)null) 
		{
			if (Formation != "Box") 
			{
				ai.WorkingMemory.SetItem ("varFormation", "Box");
				ai.WorkingMemory.SetItem ("varFormationSet", 0);
			}

			foreach (var cop in SquadCopList) {

				if (cop.GetComponent<RAIN.Core.AIRig> ().AI.WorkingMemory.GetItem<int> ("varReadyToMove") == 0)
				{
					ChangeTestLoc = false;
					break;
				}

				ChangeTestLoc = true;
			}

			if(ChangeTestLoc)
				ai.WorkingMemory.SetItem ("varLastHeading", MoveLoc);

		}

		else 
		{
			ai.WorkingMemory.SetItem ("varFormation", "Line");
			ai.WorkingMemory.SetItem ("varFormationSet", 0);

			foreach (var cop in SquadCopList) {
				
				cop.GetComponent<RAIN.Core.AIRig> ().AI.WorkingMemory.SetItem("varReadyToMove", 1);
			}
		}

		if (Formation == "Line" && FormationSet == 0) 
		{
			for(int i = 0; i < 5; i++)
			{
				FormationLine(i, ai);
			}
			ai.WorkingMemory.SetItem ("varFormationSet", 1);
		}

		if (Formation == "Box" && FormationSet == 0) 
		{
			for(int i = 0; i < 5; i++)
			{
				FormationBox(i, ai);
			}
			ai.WorkingMemory.SetItem ("varFormationSet", 1);
		}

		if (Formation == "Wedge" && FormationSet == 0) 
		{
			for(int i = 0; i < 5; i++)
			{
				FormationBox(i, ai);
			}
			ai.WorkingMemory.SetItem ("varFormationSet", 1);
		}

        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}