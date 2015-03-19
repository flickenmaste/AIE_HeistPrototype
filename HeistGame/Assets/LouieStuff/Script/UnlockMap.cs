using UnityEngine;
using System.Collections;

public class UnlockMap : MonoBehaviour {

	// Use this for initialization
	public GameObject[] KeyIndividuals;
	public int KeyIndivFound;

	void Start () {
		KeyIndivFound = 0;
	}
	void UnlockFogofWar()
	{
		//Raycast View
		RaycastHit hit;
		if (Physics.Raycast (transform.position, transform.forward,out hit, 100.0f))
		{
			Debug.DrawLine(transform.position,hit.transform.position);
		}
	}

	// Update is called once per frame
	void Update () {
		if (KeyIndivFound <= KeyIndividuals.Length) {
			RaycastHit hit;
			if (Physics.Raycast (transform.position, transform.forward, out hit, 100)) {
				if(hit.transform.GetComponent<ImportantPeaple>() != null){
					foreach(GameObject Individual in KeyIndividuals){
						if (Individual.GetComponent<ImportantPeaple>().Found == false && hit.transform.gameObject == Individual.gameObject){
							Individual.GetComponent<ImportantPeaple>().Found = true;

						}
					}
				}
			}
		}
	}
}
