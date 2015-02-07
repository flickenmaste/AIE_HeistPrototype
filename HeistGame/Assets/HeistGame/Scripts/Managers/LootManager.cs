using UnityEngine;
using System.Collections;

public class LootManager : MonoBehaviour {

    // Nodes for loot manager
    public GameObject LootSpawn;
    public GameObject DropOffPoint;

    // bool to check if loot picked up or not
    public bool LootTaken;

    // Loot to spawn
    public GameObject Loot;

	// Use this for initialization
	void Start () 
    {
        LootTaken = false;
        LootSpawn = GameObject.FindGameObjectWithTag("LootManSpawn");
        DropOffPoint = GameObject.FindGameObjectWithTag("LootManDropOff");
        SpawnLoot();
	}
	
	// Update is called once per frame
	void Update () 
    {

	}

    void SpawnLoot()
    {
        GameObject clone = Instantiate(Loot, LootSpawn.transform.position, Quaternion.identity) as GameObject;
    }

    void LootIsTaken()
    {

    }
}
