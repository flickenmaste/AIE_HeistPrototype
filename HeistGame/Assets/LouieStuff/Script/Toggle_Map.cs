using UnityEngine;
using System.Collections;

public class Toggle_Map : MonoBehaviour {

	public Camera MapCamera;
	public bool Active;
	public MouseLook look;
	// Use this for initialization
	void Start () {
		Active = false;
		ShowMap ();
	
	}
	void ShowMap()
	{
        if (MapCamera.GetComponent<TakeScreenShot>().Captured == false)
        {
            if (Active == false)
            {

                MapCamera.transform.gameObject.SetActive(false);
                look.sensitivityX = 15.0f;
                look.sensitivityY = 15.0f;
                look.minimumX = -360.0f;
                look.maximumX = 360.0f;
                look.minimumY = -45.0f;
                look.maximumY = 45.0f;

            }
            else if (Active == true)
            {
                MapCamera.transform.gameObject.SetActive(true);
                look.sensitivityX = 0.0f;
                look.sensitivityY = 0.0f;
                look.minimumX = 0.0f;
                look.maximumX = 0.0f;
                look.minimumY = 0.0f;
                look.maximumY = 0.0f;
            }
        }
        else
        {
            look.enabled = true;
            look.sensitivityX = 15.0f;
            look.sensitivityY = 15.0f;
            look.minimumX = -360.0f;
            look.maximumX = 360.0f;
            look.minimumY = -45.0f;
            look.maximumY = 45.0f;
            if (Active == false)
            {
                MapCamera.transform.gameObject.SetActive(false);
            }
            else if (Active == true)
            {
                MapCamera.transform.gameObject.SetActive(true);
            }
        }
            
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.M)) {
			Active = !Active;
		}
		ShowMap();

	}
}
