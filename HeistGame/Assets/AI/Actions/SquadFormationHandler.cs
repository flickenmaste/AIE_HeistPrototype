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
		if (SquadPlace == 0) 
		{
			FormationManager = Vector3.zero;
		}
		
		else if (SquadPlace == 1)
		{
			FormationManager.Set(1.5f, 0, -1.5f);
		}
		
		else if (SquadPlace == 3)
		{
			FormationManager.Set(-1.5f, 0, -1.5f);
		}
		
		else if (SquadPlace == 2)
		{
			FormationManager.Set(3.0f, 0, -3.0f);
		}
		
		else if (SquadPlace == 4)
		{
			FormationManager.Set(-3.0f, 0, -3.0f);
		}
		
		GameObject SquadSpot = GameObject.FindGameObjectWithTag (Positions [SquadPlace]);
		
		SquadSpot.transform.localPosition = FormationManager;
		
		SquadCopList [SquadPlace].GetComponent<AIRig> ().AI.WorkingMemory.SetItem ("varMainMovePoint", SquadSpot);
		
		SquadCopList [SquadPlace].GetComponent<AIRig> ().AI.Motor.CloseEnoughDistance = 0.1f;
	}

	void FormationBox(int SquadPlace, RAIN.Core.AI ai)
	{
		if (SquadPlace == 0) 
		{
			FormationManager = Vector3.zero;
		}
		
		else if (SquadPlace == 1)
		{
			FormationManager.Set(1.5f, 0, 1.5f);
		}
		
		else if (SquadPlace == 3)
		{
			FormationManager.Set(-1.5f, 0, 1.5f);
		}
		
		else if (SquadPlace == 2)
		{
			FormationManager.Set(1.5f, 0, -1.5f);
		}
		
		else if (SquadPlace == 4)
		{
			FormationManager.Set(-1.5f, 0, -1.5f);
		}

		GameObject SquadSpot = GameObject.FindGameObjectWithTag (Positions [SquadPlace]);

		SquadSpot.transform.localPosition = FormationManager;

		SquadCopList [SquadPlace].GetComponent<AIRig> ().AI.WorkingMemory.SetItem ("varMainMovePoint", SquadSpot);

		SquadCopList [SquadPlace].GetComponent<AIRig> ().AI.Motor.CloseEnoughDistance = 0.1f;
	}

	void FormationLine(int SquadPlace, RAIN.Core.AI ai)
	{
		GameObject SquadSpot = GameObject.FindGameObjectWithTag (Positions [SquadPlace]);

		FormationManager.Set (0, 0, 0);

		SquadSpot.transform.localPosition = FormationManager;

		SquadCopList[SquadPlace].GetComponent<AIRig> ().AI.WorkingMemory.SetItem ("varMainMovePoint", SquadSpot);
		SquadCopList[SquadPlace].GetComponent<AIRig>().AI.WorkingMemory.SetItem("varLookPoint", ai.Body.transform.position);
		SquadCopList[SquadPlace].GetComponent<AIRig> ().AI.Motor.CloseEnoughDistance = 1.5f * SquadPlace;
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

		Vector3 Heading = ai.WorkingMemory.GetItem<Vector3>("varLastHeading");

		if(Vector3.Distance(Heading, ai.Body.transform.position) < .5f)
			ai.WorkingMemory.SetItem("varReadyToMove", 1);

		bool ChangeTestLoc = false;

		if (MoveLoc != TestLoc && ai.WorkingMemory.GetItem<GameObject> ("varPlayer") == (GameObject)null) 
		{
			if (Formation != "Box") 
			{
				ai.WorkingMemory.SetItem ("varFormation", "Box");
				ai.WorkingMemory.SetItem ("varFormationSet", 1);
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
			{
				ai.WorkingMemory.SetItem ("varLastHeading", MoveLoc);
				ai.WorkingMemory.SetItem("varReadyToMove", 0);
			}

		}

		else 
		{
			ai.WorkingMemory.SetItem ("varFormation", "Line");
			ai.WorkingMemory.SetItem ("varFormationSet", 0);

			foreach (var cop in SquadCopList) {
				
				cop.GetComponent<RAIN.Core.AIRig> ().AI.WorkingMemory.SetItem("varReadyToMove", 1);
			}
		}

		if (Formation == "Line" && FormationSet == 1) 
		{
			for(int i = 0; i < 5; i++)
			{
				FormationLine(i, ai);
			}
			ai.WorkingMemory.SetItem ("varFormationSet", 0);
		}

		if (Formation == "Box" && FormationSet == 1) 
		{
			for(int i = 0; i < 5; i++)
			{
				FormationBox(i, ai);
			}
			ai.WorkingMemory.SetItem ("varFormationSet", 0);
		}

		if (Formation == "Wedge" && FormationSet == 1) 
		{
			for(int i = 0; i < 5; i++)
			{
				FormationWedge(i, ai);
			}
			ai.WorkingMemory.SetItem ("varFormationSet", 0);
		}

        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}