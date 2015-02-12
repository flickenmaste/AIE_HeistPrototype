using UnityEngine;
using System.Collections;

public class GoldLoot : MonoBehaviour {

    public bool Taken;
    
    // Use this for initialization
	void Start () 
    {
        Taken = false;
	}
	
	// Update is called once per frame
	void Update () 
    {

	}

    public void HideLoot()
    {
        if (Taken)
            gameObject.SetActive(false);
        else
            gameObject.SetActive(true);
    }
}
