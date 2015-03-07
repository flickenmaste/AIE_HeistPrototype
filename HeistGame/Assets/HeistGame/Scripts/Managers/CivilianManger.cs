using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

////////TO DO LIST
/// add a timer to the civilian seeing the player
/// get civilians to perform a task once they reach their goal
/// get civilians to hear the player

////////STRETCH GOALS
/// possibly make a list of goals for the civilians
/// get a list of possible goals once the level is fleshed out

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

	public float WaitTime;

	// Use this for initialization
	void Start ()
	{
		Cop = GameObject.FindGameObjectWithTag("Cop");
		SpawnCivilian();
		WaitTime = 3000;
	}
	
	// Update is called once per frame
	void Update ()
	{
		//perhaps using a timer to spawn civilians?
			//SpawnCivilian();


		//checks how many civilians exist
		//if 0 then the civilian functions will not be called
		NumOfCivilians = GameObject.FindGameObjectsWithTag ("Civilian");


		if (Input.GetKeyDown(KeyCode.C))
		{
			SpawnCivilian();
		}

		if (NumOfCivilians.Length != 0)
		{
			if (GetGoal() == "Bank")
			{
				Clone.GetComponent<RAIN.Core.AIRig>().AI.WorkingMemory.SetItem("varPath", Clone.GetComponent<RAIN.Core.AIRig> ().AI.WorkingMemory.GetItem ("varGoal").ToString());
			}
			if (GetGoal() == "Building")
			{
				Clone.GetComponent<RAIN.Core.AIRig>().AI.WorkingMemory.SetItem("varPath", Clone.GetComponent<RAIN.Core.AIRig> ().AI.WorkingMemory.GetItem ("varGoal").ToString());
			}


			if (Clone.GetComponent<RAIN.Core.AIRig>().AI.WorkingMemory.GetItem("varAfraid").Equals(100))
			{
				Clone.GetComponent<RAIN.Core.AIRig>().AI.WorkingMemory.SetItem("varGoal", "CallPolice");
			}
			
			if (GetGoal() == "CallPolice")
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
		Clone.GetComponent<RAIN.Core.AIRig> ().AI.WorkingMemory.SetItem("varGoal", RandomizeGoal());
	}

	//just makes getting the civilian's goal shorter
	public string GetGoal()
	{
		if (NumOfCivilians.Length != 0)
		{
			string Goal = Clone.GetComponent<RAIN.Core.AIRig> ().AI.WorkingMemory.GetItem ("varGoal").ToString ();
			return Goal;
		}
		else
		{
			return null;
		}
	}
}


















