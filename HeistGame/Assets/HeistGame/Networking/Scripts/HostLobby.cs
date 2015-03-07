using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class HostLobby : MonoBehaviour
{
    public Text Player1;
    public Text Player2;
    public Text Player3;
    public Text Player4;
    
    // Use this for initialization
	void Start () 
    {

	}
	
	// Update is called once per frame
	void Update () 
    {
        // todo make this an event listener so we can populate and remove names on connect and disconnect
        foreach (var connection in BoltNetwork.connections)
        {
            var token = (PlayerName)connection.ConnectToken;
            Player1.text = token.Username;
        }
	}
    
    public void UpdateList()
    {

    }

    public void PrintConnections()
    {
        foreach (var connection in BoltNetwork.connections)
        {
            var token = (PlayerName)connection.ConnectToken;
            Debug.Log(token.Username);
        }
    }
}
