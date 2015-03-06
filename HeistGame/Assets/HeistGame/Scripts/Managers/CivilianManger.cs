using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;


public class CivilianManger : MonoBehaviour
{
	public GameObject Civilian;
	public GameObject Clone;

	public Vector3 SpawnAreaLocation;
	public float SpawnRadius = 5.0f;
	public Vector3 Spawnpoint;

	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update ()
	{
		//set up some parameters for spawning more civilians
			//call civilian spawning function
	}

	public string RandomizeGoal()
	{
		string Objective;// = ("0");
		int RandomNumber = Random.Range (1, 3);
		switch (RandomNumber) {
		case 1:
			Objective = "Bank";
			break;
		case 2:
			Objective = "Building";
			break;
		default:
			Objective = "-1";
			break;
		}

		if (Objective.Equals("-1"))
		{
			Debug.Log("Objective not assigned");
		}
		return Objective;
	}
	
	//set where the civilian spawns in
	//as well as their goal when they spawn in
//	void SpawnCivilian()
//	{
//		//get a random point (using a Vector2) between -1 and 1 scaled to the radius of a circle around a spawnpoint
//		//to create an area for the civilians to spaw in, just make the radius the size of the sidewalk or something
//		SpawnAreaLocation = GameObject.FindGameObjectWithTag("CivilianSpawner").transform.position;
//
//		Spawnpoint = (Random.Range(-1, 2) * SpawnRadius, 0,Random.Range(-1, 2) * SpawnRadius);
//
//		GameObject Clone = Instantiate(Civilian/*,set position, set rotation*/) as GameObject;
//		Clone.GetComponent<RAIN.Core.AIRig> ().AI.WorkingMemory.SetItem("varObjective", RandomizeGoal());
//	}
}
