using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PingManager : MonoBehaviour {

	// Use this for initialization
	public GameObject Manager;
    public GameObject Blueprint;

    // Use this for initialization
    //for uncover map
    public List<GameObject> KeyIndividuals;
    public int KeyIndivFound;
    ImportantPeaple[] Targets;

    public GameObject DrillPrefab;

    public GameObject Player;
    public GameObject DrillMiniMap;
    public GameObject ManagerMiniMap;

    int DropOne = 1;
	void Start () {
        int i = 0;
        Targets = new ImportantPeaple[KeyIndividuals.Count];
        while (i < KeyIndividuals.Count)
        {
            Targets[i] = new ImportantPeaple(KeyIndividuals[i]);
            i++;
        }
	
	}

    void CheckForFoundIndividuals()
    {
        if (KeyIndivFound <= KeyIndividuals.Count)
        {
            RaycastHit hit;
            if (Physics.Raycast(Player.transform.position, Player.transform.forward, out hit, 100))
            {
                foreach (ImportantPeaple target in Targets)
                if (hit.transform.name == target.IPerson.name) {
                    target.Found = true;
                    KeyIndivFound++;
                }
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
        foreach (ImportantPeaple Individual in Targets)
        {
            if (Individual.Found)
                ManagerMiniMap.transform.position = Individual.IPerson.transform.position;
        }


	}

        
}

class ImportantPeaple
{

    public bool Found;
    public GameObject IPerson;
    public ImportantPeaple(GameObject POI)
    {
        IPerson = POI;
        Found = false;
    }

}
