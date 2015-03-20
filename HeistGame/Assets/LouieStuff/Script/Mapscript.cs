using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Mapscript : MonoBehaviour {

	


    //used for switch Blueprint view
    public List<GameObject> Blueprint;
    int CurrentCameraLook;

    // Take Screenshot varibles
    Texture2D image;

    bool Grab = false;
    public bool Captured = false;

    public GameObject Player;
    //byte[] pngShot;
    List<byte[]> pngShot;

    public GameObject FogofWar;
    public GameObject turnoffCanvas;

    int loadNumber;

    // ForgofwarUncover
    List<GameObject> Tiles = new List<GameObject>();

    void Awake()
    {
        addTiles();

    }
	void Start () {
        CurrentCameraLook = 0;


        image = new Texture2D(Screen.width, Screen.height);
        pngShot = new List<byte[]>();
        loadNumber = 0;
	}

    void SwitchBlueprint()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.UpArrow) && CurrentCameraLook < (Blueprint.Capacity - 1))
        {
            CurrentCameraLook++;
            transform.position = new Vector3(transform.position.x, transform.position.y + 6.0f, transform.position.z);
        }
        else if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.DownArrow) && CurrentCameraLook != 0)
        {
            CurrentCameraLook--;
            transform.position = new Vector3(transform.position.x, transform.position.y - 6.0f, transform.position.z);
        }
        
    }

    void PostScreen()
    {
        if (Input.GetKeyDown(KeyCode.S) && Grab == false){
			Grab = true;
			CurrentCameraLook = (Blueprint.Capacity - 1);
			GetComponent<Camera>().rect = new Rect(0,0,1,1);
            Player.GetComponentInChildren<Camera>().enabled = false;
            foreach (GameObject fog in Tiles)
            {
                fog.SetActive(false);
            }
            turnoffCanvas.SetActive(false);
            foreach (GameObject blueprint in Blueprint)
            {
                Mesh mesh = blueprint.GetComponent<MeshFilter>().mesh;

            }


		}
		if (Grab == true && Captured == false && loadNumber < Blueprint.Capacity)
			transform.position = new Vector3 (transform.position.x, Blueprint [loadNumber].transform.position.y + 6.0f, transform.position.z);
        if (Captured == true)
        {
            Player.GetComponentInChildren<Camera>().enabled = true;
            foreach (GameObject fog in Tiles)
            {
                fog.SetActive(true);
            }
            turnoffCanvas.SetActive(true);
        }

	}
    void TakeScreen()
    {
        if (Grab == true && Captured == false)
        {


            //Debug.Log("Captured Screenshot");

            if (loadNumber < Blueprint.Capacity)
            {
                image = new Texture2D(Screen.width, Screen.height);
                image.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);

                image.Apply();

                pngShot.Add(image.EncodeToPNG());
                //load map texture
                Blueprint[loadNumber].GetComponent<Renderer>().material.mainTexture = image;

                loadNumber++;
            }
            else if (Captured == false)
            {
                Captured = true;
                gameObject.GetComponent<Camera>().rect = new Rect(.5f, .5f, 1, 1);
                Player.GetComponentInChildren<Camera>().transform.gameObject.SetActive(true);
                //delete lines
                GameObject[] Lines;
                Lines = GameObject.FindGameObjectsWithTag("Lines");
                foreach (GameObject line in Lines)
                {
                    Destroy(line);
                }
            }
        }

    }

    void OnPostRender()
    {

        TakeScreen();
    }

    void addTiles()
    {
        foreach( Transform fog in FogofWar.transform)
        {
            Tiles.Add(fog.gameObject);
        }

    }



	// Update is called once per frame
	void Update () {
        SwitchBlueprint();
        PostScreen();

	}
}
