using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerObject
{
    public BoltEntity character;
    public BoltConnection connection;

    public bool isServer
    {
        get { return connection == null; }
    }

    public bool isClient
    {
        get { return connection != null; }
    }

    public void Spawn()
    {
        var pos = new Vector3(0, 3, 0);

        if (!character)
        {
            character = BoltNetwork.Instantiate(BoltPrefabs.FPSController, pos, Quaternion.identity);

            if (isServer)
            {
                character.TakeControl();    // Host control
            }
            else
            {
                character.AssignControl(connection);    // Give control to client
            }
        }
    }
}

[BoltGlobalBehaviour(BoltNetworkModes.Server)]
public class ServerCallback : Bolt.GlobalEventListener
{
    static List<PlayerObject> players = new List<PlayerObject>();   // Store list of players in server

    void Awake()
    {
        PlayerObject p;
        p = new PlayerObject(); // Set up hosts player
        p.connection = null;

        players.Add(p);
    }

    public override void Connected(BoltConnection arg)
    {
        PlayerObject p;
        p = new PlayerObject(); // Set up clients player
        p.connection = arg;
        p.connection.UserData = p;

        players.Add(p);
    }

    public override void SceneLoadLocalDone(string map)
    {
        players[0].Spawn(); // Spawn host
    }

    public override void SceneLoadRemoteDone(BoltConnection connection)
    {
        GetPlayer(connection).Spawn();  // Spawn client
    }

    public static PlayerObject GetPlayer(BoltConnection connection) // Get the player to spawn
    {
        if (connection == null)
        {
            // blah
        }

        return (PlayerObject)connection.UserData;
    }

    public override void Disconnected(BoltConnection connection, Bolt.IProtocolToken token) // Remove player from list of players
    {
        players.Remove((PlayerObject)connection.UserData);
        foreach (var player in players)
        {
            if (player == (PlayerObject)connection.UserData)
            {
                BoltNetwork.Detach(player.character);
                players.Remove(player);
            }
        }
    }

    public override void EntityDetached(BoltEntity entity)
    {
        BoltEntity.Destroy(entity);
    }
}
