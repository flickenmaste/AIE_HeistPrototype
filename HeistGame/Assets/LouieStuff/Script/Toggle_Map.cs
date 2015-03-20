using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Toggle_Map : MonoBehaviour {

	public GameObject MapCamera;
	public bool Active;
	public MouseLook look;

    //Fog of war
    public GameObject FogofWar;
    List<GameObject> Tiles = new List<GameObject>();


	// Use this for initialization
	void Start () {
		Active = false;
		ShowMap ();
	
	}
    void Awake()
    {
        foreach (Transform kid in FogofWar.transform)
        {
            Tiles.Add(kid.gameObject);
        }
    }
    bool WithinATile(GameObject DistanceToPlayerGameObject)
    {
        if (Vector3.Distance(transform.position, DistanceToPlayerGameObject.transform.position) < 4.0f)
        {

            return true;
        }
        return false;
    }
    void TilesManagment()
    {
        foreach (GameObject tile in Tiles)
        {
            if (tile.GetComponent<Renderer>().material.color.a > 0 && WithinATile(tile))
            {
                if (tile.GetComponent<Renderer>().material.color.a <= 0)
                    Destroy(tile);

                else
                {
                    Color color = tile.GetComponent<Renderer>().material.color;
                    color.a -= 0.1f;
                    tile.GetComponent<Renderer>().material.color = color;
                }
            }
        }


    }
	void ShowMap()
	{
        if (MapCamera.GetComponent<Mapscript>().Captured == false)
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
                MapCamera.GetComponent<Camera>().enabled = false;
            }
            else if (Active == true)
            {
                //MapCamera.transform.gameObject.SetActive(true);
                MapCamera.GetComponent<Camera>().enabled = true;
                
            }
        }
            
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.M)) {
			Active = !Active;
		}
		ShowMap();
        TilesManagment();

	}
}
