using UnityEngine;
using System.Collections;

public class WinLevel : MonoBehaviour {

	[SerializeField]
	public string LevelToLoad;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{

	}

	void OnTriggerEnter(Collider c)
	{
		if (c.gameObject.tag == "Player")
						Application.LoadLevel (LevelToLoad);
	}
}
