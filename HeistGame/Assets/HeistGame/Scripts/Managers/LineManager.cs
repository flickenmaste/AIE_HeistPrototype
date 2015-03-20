using UnityEngine;
using System.Collections;

public class LineManager : MonoBehaviour
{
    public GameObject QueueClone;
    public GameObject SpotClone;

    public GameObject[] Queues;
    public GameObject[] Spots; //handles the spots in the line

    public bool NewSpot;

	// Use this for initialization
	void Start ()
    {
        NewSpot = true;
        Queues = GameObject.FindGameObjectsWithTag("Queue");
	}
	
	// Update is called once per frame
	void Update ()
    {
        for (int i = 0; i < Queues.Length; i++)
        {
            if (NewSpot)
            {
                CreateSpot();
            }
        }
	}

    //creates spots in the lines
    void CreateSpot()
    {
        Vector3 SpawnLocation = new Vector3(Spots[Spots.Length - 1].transform.position.x,
                                            Spots[Spots.Length - 1].transform.position.y,
                                            Spots[Spots.Length - 1].transform.position.z + 5);

        SpotClone = GameObject.Instantiate(Spots[Spots.Length - 1], SpawnLocation, Quaternion.identity) as GameObject;
        SpotClone.tag = "Spot";
    }
}
