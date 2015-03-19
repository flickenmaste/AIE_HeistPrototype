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
        float xLocation = Player.transform.position.x + transform.position.x;
        if ((DistanceToPlayerGameObject.transform.position.x - xLocation) > 0.0f &&
            (DistanceToPlayerGameObject.transform.position.x - xLocation) < 8.0f &&
            (DistanceToPlayerGameObject.transform.position.z - Player.transform.position.z) > 0.0f &&
            (DistanceToPlayerGameObject.transform.position.z - Player.transform.position.z) < 8.0f &&
            (Player.transform.position.y - DistanceToPlayerGameObject.transform.position.y) > 0.0f &&
            (Player.transform.position.y - DistanceToPlayerGameObject.transform.position.y) < 2.0f){

            return true;
        }
        return false;
    }
    void TilesManagment(){
        foreach(GameObject tile in Tiles)
        {
            if ( tile != null && WithinATile(tile))
            {
                Destroy(tile);
                break;
            }
        }


    }
	
	// Update is called once per frame
	void LateUpdate () {
        TilesManagment();
	}
}
