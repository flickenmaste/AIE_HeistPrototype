using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Toggle_Map : MonoBehaviour {

	public GameObject MapCamera;
	public bool Active;
	public MouseLook look;

    //Fog of war
    public List<GameObject> FogGameObjects;
    FogofWar[] FogPlanes;

    int count;
    
    
    void Awake()
    {
        count = 0;
        FogPlanes = new FogofWar[FogGameObjects.Count];
        while (count < FogGameObjects.Count)
        {
            FogPlanes[count] = new FogofWar(FogGameObjects[count]);
            count++;
        }

    }
	// Use this for initialization
	void Start () {
		Active = false;
		ShowMap ();
	
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
        foreach (FogofWar fogs in FogPlanes)
        {
            fogs.ClearFog(transform);

        }

	}
}

class FogofWar
{
    public GameObject FogofWarPlane;

    Vector3[] vertices;
    Color[] changeAlpha;
    int count = 0;
    public FogofWar(GameObject FogTile)
    {
        int i = 0;
        FogofWarPlane = FogTile;

       
       Mesh GetFogMesh = FogofWarPlane.GetComponent<MeshFilter>().mesh;
       vertices = GetFogMesh.vertices;
       changeAlpha = new Color[vertices.Length];
       

       while (i < vertices.Length)
       {
           changeAlpha[i] = new Color(1, 1, 1, 1);
           i++;
       }
    }

    public void ClearFog(Transform PlayersTransform)
    {
        while (count < vertices.Length)
       {
           Vector3 VerticesWorldSpace = FogofWarPlane.transform.TransformPoint(vertices[count].x, vertices[count].y, vertices[count].z);
           float Distance = Vector3.Distance(PlayersTransform.position, VerticesWorldSpace);
           if (Distance < 3.0f && changeAlpha[count].a > 0)
           {
               changeAlpha[count] = new Color(changeAlpha[count].r, changeAlpha[count].g, changeAlpha[count].b, (changeAlpha[count].a - ( .25f * Time.deltaTime)));
           }
           count++;
       }
       count = 0;
           FogofWarPlane.GetComponent<MeshFilter>().mesh.colors = changeAlpha;
    }

}
