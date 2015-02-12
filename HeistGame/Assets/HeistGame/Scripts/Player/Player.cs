using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    // Bool to show player is carrying gold
    public bool CarryingGold;

    // Managers
    public GameObject LootMan;
    public GameObject PhaseMan;

    public GameObject Loot;
    
    // Use this for initialization
	void Start () 
    {
        CarryingGold = false;
        LootMan = GameObject.FindGameObjectWithTag("LootManager");
        PhaseMan = GameObject.FindGameObjectWithTag("PhaseManager");
	}
	
	// Update is called once per frame
	void Update () 
    {
        CheckInput();
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
