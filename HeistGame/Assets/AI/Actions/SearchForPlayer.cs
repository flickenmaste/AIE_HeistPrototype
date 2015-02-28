using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;
using RAIN.Navigation;
using RAIN.Navigation.Graph;

[RAINAction]
public class SearchForPlayer : RAINAction
{
	private static float _time;

	public 	static bool reachedLoc1;

    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);

		//get and set time here (fixes it not reseting when the players detected and keeps it from always reseting)
		_time = ai.WorkingMemory.GetItem<float> ("varTimeToSearch");

		_time += Time.time;

		ai.WorkingMemory.SetItem ("varTimeToSearch", _time);

		//this bool needs to be set externally to avoid script stop and start problems
		reachedLoc1 = ai.WorkingMemory.GetItem<bool>("varReachedLastPlayerPos");
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {

		//get the players last seen location
		Vector3 pos = ai.WorkingMemory.GetItem<Vector3>("varLastPlayerPos");

		//and their velocity at that time
		Vector3 lastDir = ai.WorkingMemory.GetItem<Vector3> ("varLastPlayerDirection");

		//we only need the normal of their last velocity
		lastDir.Normalize ();

		var Loc1 = Vector3.zero;

		var Loc2 = Vector3.zero;

		//this is so the cop heads to the players last location first, to make searching look better
		Loc1 = pos;

		//get a location extrapolated from the players last velocity vector
		lastDir.x += 10 * lastDir.x;
		lastDir.z += 10 * lastDir.z;

		Loc2.x = pos.x + lastDir.x;
		Loc2.z = pos.z + lastDir.z;

		//tell the cop were to go
		if (Vector3.Distance (ai.Kinematic.Position, Loc1) > 0.1f && reachedLoc1 == false) {
						ai.WorkingMemory.SetItem<Vector3> ("varSeek", Loc1);
		} else {
						ai.WorkingMemory.SetItem<Vector3> ("varSeek", Loc2);
				}

		//once the cop has reached the players last position he needs to head to the velocity based on
		if (Vector3.Distance (ai.Kinematic.Position, Loc1) <= 0.2f) 
		{
			reachedLoc1 = true;
			ai.WorkingMemory.SetItem("varReachedLastPlayerPos", true);
		}

		//if this much time has passed, go back to patroling/last patrol location
		if (_time > 50000)
		{
			ai.WorkingMemory.SetItem("varBreak", false);
			ai.WorkingMemory.SetItem("varReachedLastPlayerPos", false);
			_time = 0;
		}

        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}