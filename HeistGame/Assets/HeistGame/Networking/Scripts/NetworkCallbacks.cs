using UnityEngine;
using System.Collections;

[BoltGlobalBehaviour]
public class NetworkCallbacks : Bolt.GlobalEventListener
{
    public override void BoltStarted()
    {

    }

    public override void SceneLoadLocalDone(string map) // When player loads a level
    {
        // randomize a position
        //var pos = new Vector3(0, 2, -191);
        //var pos = new Vector3(5, 2, -19);

        // instantiate cube
        //BoltNetwork.Instantiate(BoltPrefabs.PlayerEndpoint, pos, Quaternion.identity);

        if (map == "HostLobby") // Check map
            Debug.Log("Lobby Started");

        if (map == "TestNetworkedLevel")
        {
            var pos = new Vector3(0, 2, 0);
            BoltNetwork.Instantiate(BoltPrefabs.PlayerNetworked, pos, Quaternion.identity);
            
            if (BoltNetwork.isServer)
            {
                var civ1 = new Vector3(32, 1.3f, -21);
                BoltNetwork.Instantiate(BoltPrefabs.SimpleCivNetworked, civ1, Quaternion.identity);

                var civ2 = new Vector3(32, 1.3f, -25);
                BoltNetwork.Instantiate(BoltPrefabs.SimpleCivNetworked, civ2, Quaternion.identity);
            }
        }
    }
}