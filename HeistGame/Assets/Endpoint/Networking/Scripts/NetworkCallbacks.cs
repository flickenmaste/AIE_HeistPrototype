﻿using UnityEngine;
using System.Collections;

[BoltGlobalBehaviour]
public class NetworkCallbacks : Bolt.GlobalEventListener
{
    public override void BoltStarted()
    {

    }

    public override void SceneLoadLocalDone(string map)
    {
        // randomize a position
        //var pos = new Vector3(0, 2, -191);
        //var pos = new Vector3(5, 2, -19);

        // instantiate cube
        //BoltNetwork.Instantiate(BoltPrefabs.PlayerEndpoint, pos, Quaternion.identity);

        Debug.Log("Lobby Started");
    }
}