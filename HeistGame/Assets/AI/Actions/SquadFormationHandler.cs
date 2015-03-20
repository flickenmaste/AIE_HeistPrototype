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

    private bool ChangeTestLoc;

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

        Vector3 DirectionFix = ai.Body.transform.position - SquadSpot.transform.position;

        DirectionFix.Normalize();

        DirectionFix *= -3;

        SquadCopList[SquadPlace].GetComponent<AIRig>().AI.WorkingMemory.SetItem("varLookpoint", ai.Body.transform.position + DirectionFix);
        SquadCopList[SquadPlace].GetComponent<AIRig>().AI.WorkingMemory.SetItem("varLookObject", (GameObject)null);

		SquadCopList [SquadPlace].GetComponent<AIRig> ().AI.Motor.CloseEnoughDistance = 0.2f;

        ai.Motor.DefaultSpeed = 0;

        ai.WorkingMemory.SetItem("varReadyToMove", 0);
	}

	void FormationLine(int SquadPlace, RAIN.Core.AI ai)
	{
		GameObject SquadSpot = GameObject.FindGameObjectWithTag (Positions [SquadPlace]);

		FormationManager.Set (0, 0, 0);

		SquadSpot.transform.localPosition = FormationManager;

		SquadCopList[SquadPlace].GetComponent<AIRig> ().AI.WorkingMemory.SetItem ("varMainMovePoint", SquadSpot);
        SquadCopList[SquadPlace].GetComponent<AIRig>().AI.WorkingMemory.SetItem("varLookpoint", Vector3.zero);
        SquadCopList[SquadPlace].GetComponent<AIRig>().AI.WorkingMemory.SetItem("varLookObject", ai.Body);
		SquadCopList[SquadPlace].GetComponent<AIRig> ().AI.Motor.CloseEnoughDistance = 1.5f + 1.5f * SquadPlace;

        ai.Motor.DefaultSpeed = 1.0f;
	}

    public override void Start(RAIN.Core.AI ai)
    {
        SquadName = ai.WorkingMemory.GetItem<string>("varSquadName");

        SquadCopList = GameObject.FindGameObjectsWithTag(SquadName + "Cop");

		//Formation = ai.WorkingMemory.GetItem<string>("varFormation");
		
		SquadSize = ai.WorkingMemory.GetItem<int>("varSquadSize");
		
		Positions[0] = SquadName + "One";
		Positions[1] = SquadName + "Two";
		Positions[2] = SquadName + "Three";
		Positions[3] = SquadName + "Four";
		Positions[4] = SquadName + "Five";

        base.Start(ai);
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        Vector3 MoveLoc = Vector3.zero;

        if(ai.WorkingMemory.GetItem<Vector3>("varSeek").y != -100)
		 MoveLoc = ai.WorkingMemory.GetItem<RAIN.Motion.MoveLookTarget>("varSeek").VectorTarget;

		Vector3 TestLoc = ai.WorkingMemory.GetItem<Vector3>("varLastHeading");

       Formation = ai.WorkingMemory.GetItem<string>("varFormation");

		int FormationSet = ai.WorkingMemory.GetItem<int>("varFormationSet");

		if(Vector3.Distance(TestLoc, ai.Body.transform.position) < .5f)
        {
			ai.WorkingMemory.SetItem("varReadyToMove", 0);
        }

		ChangeTestLoc = false;

		if (MoveLoc != TestLoc && ai.WorkingMemory.GetItem<GameObject> ("varPlayer") == (GameObject)null) 
		{
			if (Formation != "Box")
			{
				ai.WorkingMemory.SetItem ("varFormation", "Box");
				ai.WorkingMemory.SetItem ("varFormationSet", 1);
			}

            ai.WorkingMemory.SetItem("varReadyToMove", 0);

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
				ai.WorkingMemory.SetItem("varReadyToMove", 1);
			}

		}

        if ((MoveLoc == TestLoc &&  ai.WorkingMemory.GetItem<GameObject>("varPlayer") == (GameObject)null) || ai.WorkingMemory.GetItem<int>("varReadyToMove") == 1)
		{
            if (Formation != "Line")
            {
                ai.WorkingMemory.SetItem("varFormation", "Line");
                ai.WorkingMemory.SetItem("varFormationSet", 1);
            }
            ai.WorkingMemory.SetItem("varReadyToMove", 1);
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