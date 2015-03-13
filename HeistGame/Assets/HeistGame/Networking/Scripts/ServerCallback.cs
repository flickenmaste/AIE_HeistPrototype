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
        var pos = new Vector3(0, 2, 0);

        if (!character)
        {
            character = BoltNetwork.Instantiate(BoltPrefabs.PlayerNetworked, pos, Quaternion.identity);

            if (isServer)
            {
                character.TakeControl();
            }
            else
            {
                character.AssignControl(connection);
            }
        }
    }
}

[BoltGlobalBehaviour(BoltNetworkModes.Server)]
public class ServerCallback : Bolt.GlobalEventListener
{
    static List<PlayerObject> players = new List<PlayerObject>();

    void Awake()
    {
        PlayerObject p;
        p = new PlayerObject();
        p.connection = null;

        players.Add(p);
    }

    public override void Connected(BoltConnection arg)
    {
        PlayerObject p;
        p = new PlayerObject();
        p.connection = arg;
        p.connection.UserData = p;

        players.Add(p);
    }

    public override void SceneLoadLocalDone(string map)
    {
        players[0].Spawn();
    }

    public override void SceneLoadRemoteDone(BoltConnection connection)
    {
        GetPlayer(connection).Spawn();
    }

    public static PlayerObject GetPlayer(BoltConnection connection)
    {
        if (connection == null)
        {
            // blah
        }

        return (PlayerObject)connection.UserData;
    }
}
