using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

////////TO DO LIST
/// add a timer to the civilian seeing the player
/// have civilians spend time at their goal to perform a task
/// get civilians to hear the player

////////STRETCH GOALS
/// possibly make a list of goals for the civilians
/// get a list of possible goals once the level is fleshed out

//uses states to trigger behaviors in the civilians?
public enum CivState
{
    IDLE,
	MOVETOTARGET,
	DOINGTASK,
	COWERING,
	RUNNING,
	CALLPOLICE
};

public enum Task
{
    GETMONEY,
    GOTOBUILDING,
    WALKACROSSMAP
};

public class CivilianManger : MonoBehaviour
{
    

	public GameObject Civilian;
	public GameObject Clone;
	public GameObject Cop;

	//array used to determine whether or not civilian functions should be called
	public GameObject[] NumOfCivilians;

	//variables for creating a spawn area
	public Vector3 SpawnAreaLocation;
	public float SpawnRadius = 5.0f;
	public Vector3 Spawnpoint;

	// Use this for initialization
	void Start ()
	{
		//temp cop to test civilian alertness
		Cop = GameObject.FindGameObjectWithTag("Cop");
	}
	
	// Update is called once per frame
	void Update ()
	{
		//checks how many civilians exist
		//if 0 then the civilian functions will not be called
		NumOfCivilians = GameObject.FindGameObjectsWithTag ("Civilian");


		if (Input.GetKeyDown(KeyCode.C))
		{
			SpawnCivilian();
		}

		if (NumOfCivilians.Length > 0)
		{
            //set the path for the civilian
            if (Clone.GetComponent<AIRig>().AI.WorkingMemory.GetItem<string>("State") == "MOVETOTARGET")
			{
				Clone.GetComponent<AIRig>().AI.WorkingMemory.SetItem("varPath", Clone.GetComponent<AIRig>().AI.WorkingMemory.GetItem<string>("varGoal"));
			}

            //limits varAfraid to 0-100, not really sure if this is necessary, but its here for now
            if (Clone.GetComponent<AIRig>().AI.WorkingMemory.GetItem<int>("varAfraid") > 100)
            {
                Clone.GetComponent<AIRig>().AI.WorkingMemory.SetItem("varAfaid", 100);
            }
            if (Clone.GetComponent<AIRig>().AI.WorkingMemory.GetItem<int>("varAfraid") < 0)
            {
                Clone.GetComponent<AIRig>().AI.WorkingMemory.SetItem("varAfaid", 0);
            }

            //if the civilian gets too scared they'll call the police
			if (Clone.GetComponent<AIRig>().AI.WorkingMemory.GetItem<int>("varAfraid") == 100)
			{
				Clone.GetComponent<AIRig>().AI.WorkingMemory.SetItem("State", CivState.CALLPOLICE.ToString());
			}
			
            //if the civilian called the police activate the police
			if (Clone.GetComponent<RAIN.Core.AIRig>().AI.WorkingMemory.GetItem("State").Equals(CivState.CALLPOLICE.ToString()))
			{
				Cop.GetComponent<RAIN.Core.AIRig>().AI.IsActive = true;
			}
		}
	}

	//creates a random string to be assigned to the
	//civilian clone when spawned
	public string RandomizeGoal()
	{
		string Objective = ("0");
		int RandomNumber = Random.Range (1, 21);

		if (RandomNumber > 1 && RandomNumber <= 10)
		{
			Objective = "Bank";
		}
		if (RandomNumber > 10 && RandomNumber <=20)
		{
			Objective = "Building";
		}

		return Objective;
	}
	
	//set where the civilian spawns in
	//as well as their goal when they spawn in
	void SpawnCivilian()
	{
		SpawnAreaLocation = GameObject.FindGameObjectWithTag("CivilianSpawner").transform.position;

		Spawnpoint.x = SpawnAreaLocation.x + (Random.Range (-1, 2) * SpawnRadius);
		Spawnpoint.y = 1;
		Spawnpoint.z = SpawnAreaLocation.z + (Random.Range(-1, 2) * SpawnRadius);

		Clone = Instantiate(Civilian, Spawnpoint, Civilian.transform.rotation) as GameObject;
		Clone.GetComponent<AIRig> ().AI.WorkingMemory.SetItem("varGoal", RandomizeGoal());

		Clone.GetComponent<AIRig> ().AI.WorkingMemory.SetItem ("State", "MOVETOTARGET");
	}

    public string ChooseRandomTask()
    {
        string task;

        int RandomNumber = Random.Range(1, 4);

        switch(RandomNumber)
        {
            case 1:
                task = Task.GETMONEY.ToString();
                break;
            case 2:
                task = Task.GOTOBUILDING.ToString();
                break;
            case 3:
                task = Task.WALKACROSSMAP.ToString();
                break;
            default:
                task = Task.GETMONEY.ToString();
                break;
        }

        return task;
    }

	//just makes getting the civilian's goal shorter
	public string GetGoal()
	{
		if (NumOfCivilians.Length != 0)
		{
			string Goal = Clone.GetComponent<AIRig> ().AI.WorkingMemory.GetItem ("varGoal").ToString ();
			return Goal;
		}
		else
		{
			return null;
		}
	}
}


















