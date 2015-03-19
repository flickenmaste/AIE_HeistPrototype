using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FogofWarUncover : MonoBehaviour {

	// Use this for initialization

    public GameObject Player;

    List<GameObject> Tiles = new List<GameObject>();


	void Start () {
        foreach(Transform kid in gameObject.transform)
        {
            Tiles.Add(kid.gameObject);
        }
	}
    bool WithinATile(GameObject DistanceToPlayerGameObject)
    {
        if (Vector3.Distance(Player.transform.position, DistanceToPlayerGameObject.transform.position) < 5.0f)
        {

            return true;
        }
        return false;
    }
    void TilesManagment(){
        foreach(GameObject tile in Tiles)
        {
            if (tile.GetComponent<Renderer>().material.color.a > 0 && WithinATile(tile))
            {
                //Destroy(tile);
                Color color = tile.GetComponent<Renderer>().material.color;
                color.a -= 0.1f;
                tile.GetComponent<Renderer>().material.color = color;
            }
        }


    }
	
	// Update is called once per frame
	void LateUpdate () {
        TilesManagment();
	}
}
