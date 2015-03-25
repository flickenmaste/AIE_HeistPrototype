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
        //nope
        base.Start(ai);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
		//telling the cop it should break out of it's patrol state
		ai.WorkingMemory.SetItem ("Int_Break", 1);

		//get the player game object
		GameObject player = ai.WorkingMemory.GetItem<GameObject> ("Obj_Player");

		//store the players location from the gameobject (once the player is out of range, this will be their last location)
		ai.WorkingMemory.SetItem("Vec3_LastPlayerPos", (Vector3)player.transform.position);

		//reset the time to search (fixes problems a local variable has with rain scripts)
		ai.WorkingMemory.SetItem ("Float_TimeToSearch", 0);

		//make sure the cop is going to head to the players last position first
		ai.WorkingMemory.SetItem ("Bool_ReachedLastPlayerPos", false);

        return ActionResult.SUCCESS;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public override void Stop(RAIN.Core.AI ai)
    {
        //tell the squad leader to go back to what it was doing
		ai.WorkingMemory.SetItem ("Int_Break", 0);

        base.Stop(ai);
    }
}