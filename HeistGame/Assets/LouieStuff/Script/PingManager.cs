using UnityEngine;
using System.Collections;

public class PingManager : MonoBehaviour {

	// Use this for initialization
	public GameObject Manager;
    public GameObject Blueprint;

    // Use this for initialization
    //for uncover map
    public GameObject[] KeyIndividuals;
    public int KeyIndivFound;

    public GameObject DrillPrefab;

    public GameObject Player;
    public GameObject DrillMiniMap;
    public GameObject ManagerMiniMap;

    int DropOne = 1;
	void Start () {
	
	}

    void CheckForFoundIndividuals()
    {
        if (KeyIndivFound <= KeyIndividuals.Length)
        {
            RaycastHit hit;
            if (Physics.Raycast(Player.transform.position, Player.transform.forward, out hit, 100))
            {
                if (hit.transform.name == KeyIndividuals[0].name)
                    KeyIndivFound++;
            }
        }

    }
    void DrillLocator()
    {
        if (DropOne == 0)
            DrillMiniMap.transform.position = DrillPrefab.transform.position;
        else
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                DropOne = 0;
                DrillPrefab = Instantiate(DrillPrefab,Player.transform.position,new Quaternion(0,0,0,0)) as GameObject;
            }
        }


    }
	// Update is called once per frame
	void Update () {
        CheckForFoundIndividuals();
        DrillLocator();
        foreach (GameObject Individual in KeyIndividuals)
        {
            if (KeyIndivFound == 1)
                ManagerMiniMap.transform.position = Manager.transform.position;
        }


	}

        
}
