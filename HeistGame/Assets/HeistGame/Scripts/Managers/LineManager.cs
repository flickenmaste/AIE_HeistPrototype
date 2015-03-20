using UnityEngine;
using System.Collections;

public class LineManager : MonoBehaviour
{
    public GameObject Clone;

    public GameObject[] Lines;
    public GameObject[] Spots; //handles the spots in the line

	// Use this for initialization
	void Start ()
    {
        Lines = GameObject.FindGameObjectsWithTag("Queue");
	}
	
	// Update is called once per frame
	void Update ()
    {
        
	}

    //creates lines for the AIs to queue up in
    void CreateQueue()
    {

    }

    //creates spots in the lines
    void CreateSpot()
    {
        Vector3 SpawnLocation = new Vector3(Spots[Spots.Length - 1].transform.position.x,
                                            Spots[Spots.Length - 1].transform.position.y,
                                            Spots[Spots.Length - 1].transform.position.z + 5);

        Clone = GameObject.Instantiate(Spots[Spots.Length - 1], SpawnLocation, Spots[Spots.Length - 1].transform.rotation) as GameObject;
    }
}
