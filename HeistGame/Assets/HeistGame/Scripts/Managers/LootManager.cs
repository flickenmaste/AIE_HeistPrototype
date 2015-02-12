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

    public GameObject Player;

	// Use this for initialization
	void Start () 
    {
        LootTaken = false;
        LootSpawn = GameObject.FindGameObjectWithTag("LootManSpawn");
        DropOffPoint = GameObject.FindGameObjectWithTag("LootManDropOff");
        Player = GameObject.FindGameObjectWithTag("Player");
        SpawnLoot();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Player.GetComponent<Player>().CarryingGold)
            LootIsTaken();
    }

    void SpawnLoot()
    {
        GameObject clone = Instantiate(Loot, LootSpawn.transform.position, Quaternion.identity) as GameObject;
    }

    void LootIsTaken()
    {
        float range = (1f * 1f);
        float distance = Vector3.Distance(Player.transform.position, DropOffPoint.transform.position);
        if (distance <= range)
        {
            Debug.Log("Dropped off");
        }
    }
}
