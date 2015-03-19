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

		ai.WorkingMemory.SetItem ("varPlayer", (GameObject)null);

		//get and set time here (fixes it not reseting when the players detected and keeps it from always reseting)
		_time = ai.WorkingMemory.GetItem<float> ("varTimeToSearch");

		_time += Time.deltaTime;

		ai.WorkingMemory.SetItem ("varTimeToSearch", _time);

		//this bool needs to be set externally to avoid script stop and start problems
		reachedLoc1 = ai.WorkingMemory.GetItem<bool>("varReachedLastPlayerPos");
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {

		//get the players last seen location
		Vector3 pos = ai.WorkingMemory.GetItem<Vector3>("varLastPlayerPos");
		
		var PlayerLastPos = Vector3.zero;

		var SearchPos = Vector3.zero;
		
		//uses the index number to get the waypoints position for the players last location
		//PlayerLastPos = NavigationManager.Instance.GetWaypointSet ("NewSearchPath").Waypoints[foundPlayer].position;

		//get a location extrapolated from the players last velocity vector
		//lastDir.x += 50 * lastDir.x;
		//lastDir.z += 50 * lastDir.z;
		
		//Loc2.x = PlayerLastPos.x + lastDir.x;
		//Loc2.z = PlayerLastPos.z + lastDir.z;

		//gets the index number for the closest waypoint in the graph for the players last velocity vector
		//int foundSearch = NavigationManager.Instance.GetWaypointSet("NewSearchPath").GetClosestWaypointIndex(Loc2);

		//uses the index number to get the waypoints position for the velocity vector based location
		//SearchPos = NavigationManager.Instance.GetWaypointSet ("NewSearchPath").Waypoints[foundSearch].position;

		//tell the cop were to go
		//if (Vector3.Distance (ai.Kinematic.Position, PlayerLastPos) > 0.1f && reachedLoc1 == false) {
						ai.WorkingMemory.SetItem<Vector3> ("varSearch", pos);
		//} //else {
				//		ai.WorkingMemory.SetItem<Vector3> ("varSeek", SearchPos);
				//}

		//once the cop has reached the players last position he needs to head to the velocity based on
		//if (Vector3.Distance (ai.Kinematic.Position, PlayerLastPos) <= 0.2f) 
		//{
		//	reachedLoc1 = true;
		//	ai.WorkingMemory.SetItem("varReachedLastPlayerPos", true);
		//}

		//if this much time has passed, go back to patroling/last patrol location
		if (_time > 35)
		{
			ai.WorkingMemory.SetItem("varBreak", 0);
			ai.WorkingMemory.SetItem("varReachedLastPlayerPos", false);
			ai.WorkingMemory.SetItem ("varPlayer", (GameObject)null);
			_time = 0;
		}

        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
		ai.WorkingMemory.SetItem ("varPlayer", (GameObject)null);

        base.Stop(ai);
    }
}