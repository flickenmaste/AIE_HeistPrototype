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

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void FormationWedge(int SquadPlace, RAIN.Core.AI ai)
	{
        //setting the position based on the position in the list

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

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //move the child object to the new local position
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		GameObject SquadSpot = GameObject.FindGameObjectWithTag (Positions [SquadPlace]);
		
		SquadSpot.transform.localPosition = FormationManager;
		
        //tell the cop to got to that object
		SquadCopList [SquadPlace].GetComponent<AIRig> ().AI.WorkingMemory.SetItem ("Obj_MovePoint", SquadSpot);

		SquadCopList [SquadPlace].GetComponent<AIRig> ().AI.Motor.CloseEnoughDistance = 0.1f;

        ai.Motor.DefaultSpeed = 0;
	}

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void FormationBox(int SquadPlace, RAIN.Core.AI ai)
	{

        //Setting the position based on its position in the list
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

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //move the cild object to the new local position
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		GameObject SquadSpot = GameObject.FindGameObjectWithTag (Positions [SquadPlace]);

		SquadSpot.transform.localPosition = FormationManager;

        //tell the cop to go there
		SquadCopList [SquadPlace].GetComponent<AIRig> ().AI.WorkingMemory.SetItem ("Obj_MovePoint", SquadSpot);


        //this part is still not working right, but nothing bad happens, so leaving it for now
        Vector3 DirectionFix = ai.Body.transform.position - SquadSpot.transform.position;

        DirectionFix.Normalize();

        DirectionFix *= -3;

        //make the cop look at the right place
        SquadCopList[SquadPlace].GetComponent<AIRig>().AI.WorkingMemory.SetItem("Vec3_Lookpoint", ai.Body.transform.position + DirectionFix);
        SquadCopList[SquadPlace].GetComponent<AIRig>().AI.WorkingMemory.SetItem("Obj_LookObject", (GameObject)null);

		SquadCopList [SquadPlace].GetComponent<AIRig> ().AI.Motor.CloseEnoughDistance = 0.2f;

        //tell the squad leader not to move, and make sure its not ready to do so until all the cops are

        ai.Motor.DefaultSpeed = 0;

        ai.WorkingMemory.SetItem("Int_ReadyToMove", 0);
	}

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void FormationLine(int SquadPlace, RAIN.Core.AI ai)
	{

        //get the child object and make its local position zero
		GameObject SquadSpot = GameObject.FindGameObjectWithTag (Positions [SquadPlace]);

		FormationManager.Set (0, 0, 0);

		SquadSpot.transform.localPosition = FormationManager;

        //tell the cops to follow the squad leader directly
		SquadCopList[SquadPlace].GetComponent<AIRig> ().AI.WorkingMemory.SetItem ("Obj_MovePoint", SquadSpot);
        SquadCopList[SquadPlace].GetComponent<AIRig>().AI.WorkingMemory.SetItem("Vec3_Lookpoint", Vector3.zero);
        SquadCopList[SquadPlace].GetComponent<AIRig>().AI.WorkingMemory.SetItem("Obj_LookObject", ai.Body);

        //this is so they don't all crowd the squad leader
		SquadCopList[SquadPlace].GetComponent<AIRig> ().AI.Motor.CloseEnoughDistance = 1.5f + 1.5f * SquadPlace;

        //the squad leader can move again
        ai.Motor.DefaultSpeed = 1.0f;
	}

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public override void Start(RAIN.Core.AI ai)
    {
        //just getting some variables that only need to be set once
        SquadName = ai.WorkingMemory.GetItem<string>("String_SquadName");

        SquadCopList = GameObject.FindGameObjectsWithTag(SquadName + "Cop");
		
		SquadSize = ai.WorkingMemory.GetItem<int>("Int_SquadSize");
		
        //get the childeren positions, they are used to make the formations
		Positions[0] = SquadName + "One";
		Positions[1] = SquadName + "Two";
		Positions[2] = SquadName + "Three";
		Positions[3] = SquadName + "Four";
		Positions[4] = SquadName + "Five";

        base.Start(ai);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public override ActionResult Execute(RAIN.Core.AI ai)
    {

        //The move location
        Vector3 MoveLoc = Vector3.zero;

        //this is for fixing the first point problem
        if(ai.WorkingMemory.GetItem<Vector3>("Vec3_Seek").y != -100)
		 MoveLoc = ai.WorkingMemory.GetItem<RAIN.Motion.MoveLookTarget>("Vec3_Seek").VectorTarget;

        //Get some variables from memory
		Vector3 TestLoc = ai.WorkingMemory.GetItem<Vector3>("Vec3_LastHeading");

        Formation = ai.WorkingMemory.GetItem<string>("String_Formation");

		int FormationSet = ai.WorkingMemory.GetItem<int>("Int_FormationSet");

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //make the ai stop at each point and go to box
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		if(Vector3.Distance(TestLoc, ai.Body.transform.position) < .5f)
        {
			ai.WorkingMemory.SetItem("Int_ReadyToMove", 0);
        }

		ChangeTestLoc = false;

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //if at point and there is no player go to box
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		if ((MoveLoc != TestLoc && ai.WorkingMemory.GetItem<GameObject> ("Obj_Player") == (GameObject)null)) 
		{
			if (Formation != "Box")
			{
				ai.WorkingMemory.SetItem ("String_Formation", "Box");
				ai.WorkingMemory.SetItem ("Int_FormationSet", 1);
			}


            //Check if all cops are ready to move
			foreach (var cop in SquadCopList) {

				if (cop.GetComponent<RAIN.Core.AIRig> ().AI.WorkingMemory.GetItem<int> ("Int_ReadyToMove") == 0)
				{
					ChangeTestLoc = false;
                    ai.WorkingMemory.SetItem("Int_ReadyToMove", 0);
					break;
				}

				ChangeTestLoc = true;
			}

            // if they are, then move to next point
			if(ChangeTestLoc)
			{
				ai.WorkingMemory.SetItem ("Vec3_LastHeading", MoveLoc);
				ai.WorkingMemory.SetItem("Int_ReadyToMove", 1);
			}

		}

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //if not at current point go to line formation and move to next point
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        if ((MoveLoc == TestLoc &&  ai.WorkingMemory.GetItem<GameObject>("Obj_Player") == (GameObject)null))
		{
            if (Formation != "Line")
            {
                ai.WorkingMemory.SetItem("String_Formation", "Line");
                ai.WorkingMemory.SetItem("Int_FormationSet", 1);
            }

            ai.WorkingMemory.SetItem("Int_ReadyToMove", 1);
		}

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Changing the formations happens here
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //Change formation to line
		if (Formation == "Line" && FormationSet == 1) 
		{
			for(int i = 0; i < 5; i++)
			{
				FormationLine(i, ai);
			}
			ai.WorkingMemory.SetItem ("Int_FormationSet", 0);
		}

        //Change formation to box
		if (Formation == "Box" && FormationSet == 1) 
		{
			for(int i = 0; i < 5; i++)
			{
				FormationBox(i, ai);
			}
			ai.WorkingMemory.SetItem ("Int_FormationSet", 0);
		}

        //This does the wedge
		if (Formation == "Wedge" && FormationSet == 1) 
		{
			for(int i = 0; i < 5; i++)
			{
				FormationWedge(i, ai);
			}
			ai.WorkingMemory.SetItem ("Int_FormationSet", 0);
		}


        return ActionResult.SUCCESS;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public override void Stop(RAIN.Core.AI ai)
    {
        //nothing here go away
        base.Stop(ai);
    }
}