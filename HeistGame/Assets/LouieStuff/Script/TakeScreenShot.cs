using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;

#endif

using System.Runtime.Serialization.Formatters.Binary;


using System.Runtime.Serialization;



public class TakeScreenShot : MonoBehaviour {

	Texture2D image;

	bool Grab = false;
	public bool Captured = false;

	public Camera PlayerCamera;
	public Camera MapCamera;
	//byte[] pngShot;
	List<byte[]> pngShot;

	public List<GameObject> Blueprint;
	GameObject CurrentFloor;
	int loadNumber;

	// Use this for initialization
	void Start () {
		image = new Texture2D(Screen.width,Screen.height);
		pngShot = new List<byte[]>();
		loadNumber = 0;
	}
	void OnDestroy()
	{

	}

	void  OnPostRender() 
	{
		if (Grab == true && Captured == false ) {

			
			//Debug.Log("Captured Screenshot");

			if(loadNumber < Blueprint.Capacity){
				image = new Texture2D(Screen.width,Screen.height);
				image.ReadPixels(new Rect(0,0,Screen.width,Screen.height),0,0);

				image.Apply();

				pngShot.Add(image.EncodeToPNG());
				//load map texture
				Blueprint[loadNumber].GetComponent<Renderer>().material.mainTexture = image;

				loadNumber++;
			}
			else if(Captured == false){
				Captured = true;
				MapCamera.rect = new Rect(.5f,.5f,1,1);
				PlayerCamera.transform.gameObject.SetActive(true);
				//delete lines
				GameObject[]  Lines;
				Lines = GameObject.FindGameObjectsWithTag("Lines");
				foreach(GameObject line in Lines){
					Destroy(line);
				}
			}	
		}
	}
		// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.S) && Grab == false){
			Grab = true;
			MapCamera.GetComponent<SwitchBlueprints>().CurrentCameraLook = (Blueprint.Capacity - 1);
			MapCamera.rect = new Rect(0,0,1,1);
			PlayerCamera.transform.gameObject.SetActive(false);

		}
		if (Grab == true && Captured == false && loadNumber < Blueprint.Capacity)
			MapCamera.transform.position = new Vector3 (MapCamera.transform.position.x, Blueprint [loadNumber].transform.position.y + 10.0f, MapCamera.transform.position.z);


	}
}
