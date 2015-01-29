using UnityEngine;
using System.Collections;

public class BasicCop : NPC {

    public float myHealth;

    public NavMeshAgent Agent;
    
    // Use this for initialization
	void Start () 
    {
        myHealth = 100;
        renderer.material.color = Color.blue;
	}
	
	// Update is called once per frame
	void Update () 
    {

	}

    public void TakeDamage(float dmg)
    {
        if (myHealth > 0.0f)
            myHealth += -dmg;

        if (myHealth <= 0.0f)
            Destroy(this.gameObject, 0);
    }

    public void MoveToPlayer()  // TEST NAVMESH, DO NOT USE
    {

    }
}
