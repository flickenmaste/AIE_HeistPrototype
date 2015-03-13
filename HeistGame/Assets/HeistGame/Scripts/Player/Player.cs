using UnityEngine;
using System.Collections;
using RAIN.Action;
using RAIN.Core;

public class Player : MonoBehaviour {

    public float Health;
    
    // Bool to show player is carrying gold
    public bool CarryingGold;

	public float ShoutDistance;

    // Managers
    public GameObject LootMan;
    public GameObject PhaseMan;

    public GameObject Loot;
    
    // Use this for initialization
	void Start () 
    {
        Health = 100.0f;
        CarryingGold = false;
        LootMan = GameObject.FindGameObjectWithTag("LootManager");
        PhaseMan = GameObject.FindGameObjectWithTag("PhaseManager");
	}
	
	// Update is called once per frame
	void Update () 
    {
        CheckInput();
		Shout();
	}

    public void TakeDamage(float dmg)
    {
        if (Health > 0.0f)
            Health += -dmg;

        //if (Health <= 0.0f)
            //Application.LoadLevel("GameOver");  // reload level or something
    }

    void CheckInput()
    {
        if (Input.GetKeyUp(KeyCode.E))
            PickUpGold();

        if (Input.GetKeyUp(KeyCode.G))
            DropGold();
    }

    void PickUpGold()
    {
        RaycastHit Hit;	// Store raycast info

        Vector3 dir = new Vector3(transform.forward.x, transform.forward.y - 0.5f, transform.forward.z);

        Debug.DrawRay(this.transform.position, dir * 2, Color.red);	// Ray so we can see


        if (Physics.Raycast(transform.position, dir, out Hit, 2))
        {
            if (Hit.transform.tag == "Loot")
            {
                LootMan.gameObject.GetComponent<LootManager>().LootTaken = true;
                Loot = Hit.transform.gameObject;
                Loot.GetComponent<GoldLoot>().Taken = true;
                Loot.GetComponent<GoldLoot>().HideLoot();
                CarryingGold = true;
            }
        }
    }

	void Shout()
	{
		if (Input.GetKeyDown(KeyCode.P))
		{
			RaycastHit Hit;
			Debug.DrawRay (this.transform.position, this.transform.forward);

			Vector3 Direciton = new Vector3 (transform.forward.x, transform.forward.y,transform.forward.z);
			if (Physics.Raycast(transform.position, Direciton, out Hit, 20.0f))
			{
				if (Hit.transform.tag == "Civilian")
				{
					//scare the civilian
					Hit.transform.gameObject.GetComponent<RAIN.Core.AIRig>().AI.WorkingMemory.SetItem("varAfraid", 100);
				}
			}
		}
	}

    void DropGold()
    {
        if (CarryingGold)
        {
            LootMan.gameObject.GetComponent<LootManager>().LootTaken = false;
            Loot.GetComponent<GoldLoot>().Taken = false;
            Loot.GetComponent<GoldLoot>().HideLoot();
            CarryingGold = false;
            Loot.transform.position = transform.position + transform.forward;
        }
    }
}
