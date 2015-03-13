using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class PerformIdleAction : RAINAction
{
    public string RandomizeGoal()
    {
        string Objective = ("0");
        int RandomNumber = Random.Range(1, 21);

        if (RandomNumber > 1 && RandomNumber <= 10)
        {
            Objective = "Bank";
        }
        if (RandomNumber > 10 && RandomNumber <= 20)
        {
            Objective = "Building";
        }

        return Objective;
    }
    private float WaitTime;
    public override void Start(RAIN.Core.AI ai)
    {
        WaitTime = ai.WorkingMemory.GetItem<float>("varWait");

        WaitTime += Time.time;

        ai.WorkingMemory.SetItem("varWait", WaitTime);

        base.Start(ai);
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        if (WaitTime > 3500)
        {
            ai.WorkingMemory.SetItem("varGoal", RandomizeGoal());
            ai.WorkingMemory.SetItem("State", "MOVETOTARGET");
        }

        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}