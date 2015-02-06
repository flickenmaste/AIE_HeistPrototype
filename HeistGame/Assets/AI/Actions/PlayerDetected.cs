using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class PlayerDetected : RAINAction
{
    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
		//telling the cop it should break out of it's patrol state
		ai.WorkingMemory.SetItem ("varBreak", true);

		//get the player game object
		GameObject player = ai.WorkingMemory.GetItem<GameObject> ("varPlayer");

		//store the players location from the gameobject (once the player is out of range, this will be their last location)
		ai.WorkingMemory.SetItem("varLastPlayerPos", (Vector3)player.transform.position);

		//storing the players velocity for later (same as above)
		ai.WorkingMemory.SetItem ("varLastPlayerDirection", (Vector3)player.rigidbody.velocity);

		//reset the time to search (fixes problems a local variable has with rain scripts)
		ai.WorkingMemory.SetItem ("varTimeToSearch", 0);

		//make sure the cop is going to head to the players last position first
		ai.WorkingMemory.SetItem ("varReachedLastPlayerPos", false);

        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}