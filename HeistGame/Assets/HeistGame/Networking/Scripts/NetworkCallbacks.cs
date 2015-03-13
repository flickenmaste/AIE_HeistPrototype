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

    }
}