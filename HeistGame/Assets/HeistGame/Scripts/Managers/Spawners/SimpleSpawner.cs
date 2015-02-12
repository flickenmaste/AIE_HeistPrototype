using UnityEngine;
using System.Collections;
using RAIN;

public class SimpleSpawner : MonoBehaviour {

    public GameObject entityToSpawn;
    
    // Use this for initialization
	void Start () 
    {

	}
	
	// Update is called once per frame
	void Update () 
    {
        Spawn();
	}

    void Spawn()
    {
        if (Input.GetKeyUp(KeyCode.Keypad0))
        {
            GameObject copClone = Instantiate(entityToSpawn, transform.position, Quaternion.identity) as GameObject;
        }
    }
}
