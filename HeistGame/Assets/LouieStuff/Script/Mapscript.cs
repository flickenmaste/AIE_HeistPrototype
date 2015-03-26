﻿using UnityEngine;
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

     
	void Start () {
        CurrentCameraLook = 0;


        image = new Texture2D(Screen.width, Screen.height);
        pngShot = new List<byte[]>();
        loadNumber = 0;
	}

    void SwitchBlueprint()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.UpArrow) && CurrentCameraLook < Blueprint.Count)
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
			CurrentCameraLook = Blueprint.Count;
			GetComponent<Camera>().rect = new Rect(0,0,1,1);
            Player.GetComponentInChildren<Camera>().enabled = false;
            turnoffCanvas.SetActive(false);
            FogofWar.SetActive(false);
		}

		if (Grab == true && Captured == false && loadNumber < Blueprint.Capacity)
			transform.position = new Vector3 (transform.position.x, Blueprint [loadNumber].transform.position.y + 6.0f, transform.position.z);

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

                //pngShot.Add(image.EncodeToPNG());

                //load map texture
                Blueprint[loadNumber].GetComponent<Renderer>().material.mainTexture = image;

                loadNumber++;
            }
            else if (Captured == false)
            {
                Captured = true;
                gameObject.GetComponent<Camera>().rect = new Rect(.5f, .5f, 1, 1);
                Player.GetComponentInChildren<Camera>().transform.gameObject.SetActive(true);

                turnoffCanvas.SetActive(true);

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



	// Update is called once per frame
	void Update () {
        SwitchBlueprint();
        PostScreen();

	}
}