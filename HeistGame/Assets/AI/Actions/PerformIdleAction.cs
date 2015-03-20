using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

public enum IdleBehaviors
{
    USEPHONE,
    SMOKE,
    CHECKWATCH,
    TYPE,
};



[RAINAction]
public class PerformIdleAction : RAINAction
{
    IdleBehaviors Idle;

    private float WaitTime;
    public float MaxWaitTime;

    public override void Start(RAIN.Core.AI ai)
    {
        //WaitTime = 0.0f;

        WaitTime = ai.WorkingMemory.GetItem<float>("varWait");

        WaitTime += Time.deltaTime;

        ai.WorkingMemory.SetItem("varWait", WaitTime);

        RandomizeIdle(ai);

        base.Start(ai);
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        if (WaitTime > MaxWaitTime)
        {
            ai.WorkingMemory.SetItem("varGoal", RandomizeGoal());
            WaitTime = 0.0f;
            ai.WorkingMemory.SetItem("State", "MOVETOTARGET");
        }

        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }

    ///////////////////////////////////////////////////////////////////////////////
    //functions that the AI uses to perform idle actions
    ///////////////////////////////////////////////////////////////////////////////

    void RandomizeIdle(RAIN.Core.AI ai)
    {
        Idle = IdleBehaviors.SMOKE;
        ai.WorkingMemory.SetItem("varIdle", Idle.ToString());
        MaxWaitTime = 6.0f;

        /*int RandomNumber = Random.Range(1, 5);
        switch (RandomNumber)
        {
            case 1:
                //Idle = IdleBehaviors.CHECKWATCH;
                Idle = IdleBehaviors.SMOKE;
                // MaxWaitTime = 500.0f;
                ai.WorkingMemory.SetItem("varIdle", Idle.ToString());
                MaxWaitTime = 6.0f;
                break;
            case 2:
                Idle = IdleBehaviors.SMOKE;
                //MaxWaitTime = 1500.0f;
                ai.WorkingMemory.SetItem("varIdle", Idle.ToString());
                MaxWaitTime = 6.0f;
                break;
            case 3:
                //Idle = IdleBehaviors.TYPE;
                Idle = IdleBehaviors.SMOKE;
                //MaxWaitTime = 3500.0f;
                ai.WorkingMemory.SetItem("varIdle", Idle.ToString());
                MaxWaitTime = 6.0f;
                break;
            case 4:
                //Idle = IdleBehaviors.USEPHONE;
                Idle = IdleBehaviors.SMOKE;
                //MaxWaitTime = 1000.0f;
                ai.WorkingMemory.SetItem("varIdle", Idle.ToString());
                MaxWaitTime = 6.0f;
                break;
            default:
                //Idle = IdleBehaviors.CHECKWATCH;
                Idle = IdleBehaviors.SMOKE;
                //MaxWaitTime = 500.0f;
                ai.WorkingMemory.SetItem("varIdle", Idle.ToString());
                MaxWaitTime = 6.0f;
                break;
        }*/
    }

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
}