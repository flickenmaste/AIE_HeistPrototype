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
	private GameObject Squad;

	private string Formation;
	
	private int SquadSize;
	
	private string SquadName;

	private string[] Positions = new string[5];

	private Vector3[] FormationManager = new Vector3[5];

	public GameObject Cop;

	void FormationTriangle(int SquadPlace)
	{
	
	}

	void FormationBox(int SquadPlace, RAIN.Core.AI ai)
	{
		if (SquadPlace == 0) 
		{
			FormationManager [SquadPlace] = ai.Body.transform.position;
		}

		else if (SquadPlace == 1)
		{
			FormationManager [SquadPlace].Set(ai.Body.transform.position.x, ai.Body.transform.position.y, ai.Body.transform.position.z + 1.1f);
		}

		else if (SquadPlace == 2)
		{
			FormationManager [SquadPlace].Set(ai.Body.transform.position.x, ai.Body.transform.position.y, ai.Body.transform.position.z + 2.2f);
		}

		else if (SquadPlace == 3)
		{
			FormationManager [SquadPlace].Set(ai.Body.transform.position.x, ai.Body.transform.position.y, ai.Body.transform.position.z - 1.1f);
		}
		
		else if (SquadPlace == 4)
		{
			FormationManager [SquadPlace].Set(ai.Body.transform.position.x, ai.Body.transform.position.y, ai.Body.transform.position.z - 2.2f);
		}

		GameObject.FindGameObjectWithTag (Positions [SquadPlace]).GetComponent<Transform> ().position = FormationManager [SquadPlace];
	}

	void FormationLine(int SquadPlace, RAIN.Core.AI ai)
	{
		Vector3 NewPos = new Vector3();
		int FormationFix = 0;

		if (SquadPlace == 0) {
						NewPos = ai.Body.transform.position;
			NewPos.z -= 3;
				}
				else {

					FormationFix = ((-1 * (SquadPlace % 2)) - (-1 * (SquadPlace % 2)));

					if(FormationFix == 0)
						FormationFix += 1;

					NewPos.z = ai.Body.transform.position.z + -2 * SquadPlace;
					NewPos.y = ai.Body.transform.position.y;
					NewPos.x = ai.Body.transform.position.x;

				}

		FormationManager[SquadPlace] = NewPos;

		GameObject.FindGameObjectWithTag (Positions[SquadPlace]).GetComponent<Transform>().position = NewPos;// = NewPos;//FormationMaker [SquadPlace];

		//position.x + spacesize * (SquadPlace * (-1 * SquadPlace % 2) - (1 * SquadPlace % 2)) 

	}

    public override void Start(RAIN.Core.AI ai)
    {
		Formation = ai.WorkingMemory.GetItem<string>("varFormation");
		
		SquadSize = ai.WorkingMemory.GetItem<int>("varSquadSize");
		
		SquadName = ai.WorkingMemory.GetItem<string>("varSquadName");
		
		//Squad = GameObject.FindGameObjectWithTag(SquadName);
		
		Positions[0] = SquadName + "One";
		Positions[1] = SquadName + "Two";
		Positions[2] = SquadName + "Three";
		Positions[3] = SquadName + "Four";
		Positions[4] = SquadName + "Five";
		
		for(int i = 0; i < 5; i++)
		{
			//FormationLine(i, ai);
			FormationBox(i, ai);
		}

		base.Start(ai);

    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
		Cop = ai.WorkingMemory.GetItem<GameObject> ("varCopPrefab");

		GameObject FirstCop = (GameObject)GameObject.Instantiate(Cop, FormationManager[0], Quaternion.Euler(new Vector3(5, 0, 0)));

		FirstCop.tag = SquadName + "Cop";

		FirstCop.GetComponent<RAIN.Core.AIRig> ().AI.WorkingMemory.SetItem ("varMainMovePoint", GameObject.FindGameObjectWithTag(Positions [0]));

		GameObject SecondCop = (GameObject)GameObject.Instantiate(Cop, FormationManager[1], Quaternion.Euler(new Vector3(5, 0, 0)));
		
		SecondCop.tag = SquadName + "Cop";

		SecondCop.GetComponent<RAIN.Core.AIRig> ().AI.WorkingMemory.SetItem ("varMainMovePoint", GameObject.FindGameObjectWithTag(Positions [1]));

		GameObject ThirdCop = (GameObject)GameObject.Instantiate(Cop, FormationManager[2], Quaternion.Euler(new Vector3(5, 0, 0)));
		
		ThirdCop.tag = SquadName + "Cop";

		ThirdCop.GetComponent<RAIN.Core.AIRig> ().AI.WorkingMemory.SetItem ("varMainMovePoint", GameObject.FindGameObjectWithTag(Positions [2]));

		GameObject FourthCop = (GameObject)GameObject.Instantiate(Cop, FormationManager[3], Quaternion.Euler(new Vector3(5, 0, 0)));
		
		FourthCop.tag = SquadName + "Cop";

		FourthCop.GetComponent<RAIN.Core.AIRig> ().AI.WorkingMemory.SetItem ("varMainMovePoint", GameObject.FindGameObjectWithTag(Positions [3]));

		GameObject FifthCop = (GameObject)GameObject.Instantiate(Cop, FormationManager[4], Quaternion.Euler(new Vector3(5, 0, 0)));
		
		FifthCop.tag = SquadName + "Cop";

		FifthCop.GetComponent<RAIN.Core.AIRig> ().AI.WorkingMemory.SetItem ("varMainMovePoint", GameObject.FindGameObjectWithTag(Positions [4]));

		ai.WorkingMemory.SetItem ("varDoOnce", 1);

        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}