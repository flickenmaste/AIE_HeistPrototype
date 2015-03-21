using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

////////TO DO LIST
/// add a timer to the civilian seeing the player
/// have civilians spend time at their goal to perform a task

////////STRETCH GOALS
/// possibly make a list of goals for the civilians
/// get a list of possible goals once the level is fleshed out
public enum CivState
{
    IDLE,
	MOVETOTARGET,
	DOINGTASK,
	COWERING,
	RUNNING,
	CALLPOLICE,
    INQUEUE
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
    public GameObject Spot;

	public GameObject[] AllCivilians;
    public GameObject[] Queues;

    
    public GameObject[] QueueSpots; //handles the spots in the line

    public bool NewSpot;

	//variables for creating a spawn area
	public Vector3 SpawnAreaLocation;
	public float SpawnRadius = 5.0f;
	public Vector3 Spawnpoint;

	// Use this for initialization
	void Start ()
	{
		//temp cop to test civilian alertness
		Cop = GameObject.FindGameObjectWithTag("Cop");

        AllCivilians = GameObject.FindGameObjectsWithTag("Civilian");

        for (int x = 0; x < 4; x++)
        {
            //Clone = GameObject.Instantiate(Queues[0]) as GameObject;
        }

        Queues = GameObject.FindGameObjectsWithTag("Queue");

        NewSpot = false;
        for (int i = 0; i < Queues.Length; i++ )
        {
            Clone = GameObject.Instantiate(Spot, Queues[i].transform.position, Queues[i].transform.rotation) as GameObject;
            Clone.tag = "Spot" + Queues[i].name;
            QueueSpots.AddFirst<GameObject>(Clone);

            CreateSpot(Queues[i]);
        }

	}
	
	// Update is called once per frame
	void Update ()
	{
		//checks how many civilians exist
		//if 0 then the civilian functions will not be called
		AllCivilians = GameObject.FindGameObjectsWithTag ("Civilian");


		if (Input.GetKeyDown(KeyCode.C))
		{
			SpawnCivilian();
		}

        if (AllCivilians.Length > 0)
        {
            Execute();
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            NewSpot = true;
        }

        for (int i = 0; i < AllCivilians.Length; i++)
        {
            if (GetGoal(AllCivilians[i]) == "SpotQueue " || GetGoal(AllCivilians[i]) == "SpotQueue 1" || GetGoal(AllCivilians[i]) == "SpotQueue 2" || GetGoal(AllCivilians[i]) == "SpotQueue 3")
            {
                QueueSpots = GameObject.FindGameObjectsWithTag(GetGoal(AllCivilians[i]));

                SetTarget(AllCivilians[i], QueueSpots[QueueSpots.Length - 1].transform.position);
            }
            
        }

        if (NewSpot)
        {
            for (int j = 0; j < Queues.Length; j++)
            {
                CreateSpot(Queues[j]);
                NewSpot = false;
            }
        }
	}

    public void ReachedSpot()
    {
        for (int i = 0; i < AllCivilians.Length; i++)
        {
            if (GetGoal(AllCivilians[i]) == "SpotQueue" && GetState(AllCivilians[i]) == "IDLE"
                || GetGoal(AllCivilians[i]) == "SpotQueue 1" && GetState(AllCivilians[i]) == "IDLE"
                || GetGoal(AllCivilians[i]) == "SpotQueue 2" && GetState(AllCivilians[i]) == "IDLE"
                || GetGoal(AllCivilians[i]) == "SpotQueue 3" && GetState(AllCivilians[i]) == "IDLE")
            {
                QueueSpots = GameObject.FindGameObjectsWithTag(GetGoal(AllCivilians[i]));
                CreateSpot(QueueSpots[QueueSpots.Length - 1]);
            }
        }
    }

	//creates a random string to be assigned to the
	//civilian clone when spawned
	public string RandomizeGoal()
	{
		string Objective = ("0");
		int RandomNumber = Random.Range (1, 61);

		if (RandomNumber > 1 && RandomNumber <= 10)
		{
			Objective = "Bank";
		}

		if (RandomNumber > 10 && RandomNumber <= 20)
		{
			Objective = "Building";
		}

        if (RandomNumber > 20 && RandomNumber <= 30)
        {
            Objective = "Spot" + Queues[0].name;
        }

        if (RandomNumber > 30 && RandomNumber <= 40)
        {
            Objective = "Spot" + Queues[1].name;
        }

        if (RandomNumber > 40 && RandomNumber <= 50)
        {
            Objective = "Spot" + Queues[2].name;
        }

        if (RandomNumber > 50 && RandomNumber <= 60)
        {
            Objective = "Spot" + Queues[3].name;
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

		Clone.GetComponent<AIRig> ().AI.WorkingMemory.SetItem ("varState", "MOVETOTARGET");

        AllCivilians = GameObject.FindGameObjectsWithTag("Civilian");
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

    public void Execute()
    {
        for (int i = 0; i < AllCivilians.Length; i++)
        {
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //set the path for the civilian
            if (GetState(AllCivilians[i]) == "MOVETOTARGET")
            {
                SetPath(AllCivilians[i], GetGoal(AllCivilians[i]));
            }

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //limits varAfraid to 0-100, not really sure if this is necessary, but its here for now
            if (GetAfraid(AllCivilians[i]) > 100)
            {
                SetAfraid(AllCivilians[i], 100);
            }
            if (GetAfraid(AllCivilians[i]) < 0)
            {
                SetAfraid(AllCivilians[i], 0);
            }

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //if the civilian gets too scared they'll call the police
            if (GetAfraid(AllCivilians[i]) == 100)
            {
                SetState(AllCivilians[i], CivState.CALLPOLICE.ToString());
            }

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //if the civilian called the police activate the police
            if (GetState(AllCivilians[i]) == (CivState.CALLPOLICE.ToString()))
            {
                Cop.GetComponent<RAIN.Core.AIRig>().AI.IsActive = true;
            }

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //if the civilian reaches their spot in the queue

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //get working later
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //possibly set up a function to get the name of the target to see if its the same as the queue and spot?
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            /*for (int j = 0; j < Queues.Length; j++)
            {
                
                if (GetGoal(AllCivilians[i]) == Queues[j].name + "Spot" && GetState(AllCivilians[i]) == "IDLE")
                {
                    CreateSpot(Queues[j]);
                }
            }*/
        }
    }

    void CreateSpot(GameObject queue)
    {
        QueueSpots = GameObject.FindGameObjectsWithTag("Spot" + queue.name);

        Vector3 SpawnLocation = QueueSpots[QueueSpots.Length - 1].transform.position;
        SpawnLocation.z -= 5;

        Clone = GameObject.Instantiate(Spot, SpawnLocation, Quaternion.identity) as GameObject;
        Clone.tag = "Spot" + queue.name;
        Clone.name = queue.name + "Spot";
    }

    //once the spot in front of the civilian is empty the civilian will move forward in the queue
	public void MoveForward()
    {
        
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public string GetState(GameObject ai)
    {
        string State = ai.GetComponent<AIRig>().AI.WorkingMemory.GetItem<string>("varState");
        return State;
    }

    public void SetState(GameObject ai, string state)
    {
        ai.GetComponent<AIRig>().AI.WorkingMemory.SetItem("varState", state);
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public int GetAfraid(GameObject ai)
    {
        int Afraid = ai.GetComponent<AIRig>().AI.WorkingMemory.GetItem<int>("varAfraid");
        return Afraid;
    }

    public void SetAfraid(GameObject ai, int afraid)
    {
        ai.GetComponent<AIRig>().AI.WorkingMemory.SetItem("varAfraid", afraid);
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public string GetPath(GameObject ai)
    {
        string Path = ai.GetComponent<AIRig>().AI.WorkingMemory.GetItem<string>("varPath");
        return Path;
    }

    public void SetPath(GameObject ai, string path)
    {
        ai.GetComponent<AIRig>().AI.WorkingMemory.SetItem("varPath", path);
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public string GetGoal(GameObject ai)
    {
        string Goal = ai.GetComponent<AIRig>().AI.WorkingMemory.GetItem<string>("varGoal");
        return Goal;
    }

    public void SetGoal(GameObject ai, string goal)
    {
        ai.GetComponent<AIRig>().AI.WorkingMemory.SetItem("varGoal", goal);
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public Vector3 GetTarget(GameObject ai)
    {
        Vector3 Target = ai.GetComponent<AIRig>().AI.WorkingMemory.GetItem<Vector3>("varTarget");
        return Target;
    }

    public void SetTarget(GameObject ai, Vector3 target)
    {
        ai.GetComponent<AIRig>().AI.WorkingMemory.SetItem("varTarget", target);
    }
}

//varGoal == "SpotQueue " || varGoal == "SpotQueue 1" || varGoal == "SpotQueue 2" || varGoal == "SpotQueue 3"