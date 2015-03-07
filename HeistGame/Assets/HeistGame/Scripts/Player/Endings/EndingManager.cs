using UnityEngine;
using System.Collections;

public class EndingManager : MonoBehaviour {

	// Use this for initialization
	void Start () 
    {
        Screen.lockCursor = false;
        Cursor.visible = true;
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public void RestartLevelClick()
    {
        Application.LoadLevel("prototypedemolevel");
    }
}
