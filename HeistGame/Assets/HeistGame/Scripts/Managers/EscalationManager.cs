using UnityEngine;
using System.Collections;

public class EscalationManager : Bolt.EntityBehaviour<IEscaMan>
{

    #region old code
    private bool phaseOne;
    private bool phaseTwo;
    private bool phaseThree;
    public bool PhaseOne   // Cops are called in
    {
        get
        {
            return phaseOne;
        }
        set
        {
            phaseOne = value;
            //CheckPhase();
        }
    }
    public bool PhaseTwo  // Cops will try and enter building
    {
        get
        {
            return phaseTwo;
        }
        set
        {
            phaseTwo = value;
            //CheckPhase();
        }
    }
    public bool PhaseThree // Cops fire on sight
    {
        get
        {
            return phaseThree;
        }
        set
        {
            phaseThree = value;
            //CheckPhase();
        }
    }
    #endregion

    public Queue PhaseQueue;
    
    // Use this for initialization
	void Start () 
    {
        //PhaseOne = false;
        //PhaseTwo = false;
        //PhaseThree = false;
        PhaseQueue = new Queue(4);
        PhaseQueue.Enqueue("NoPhase");
        PhaseQueue.Enqueue("PhaseOne");
        PhaseQueue.Enqueue("PhaseTwo");
        PhaseQueue.Enqueue("PhaseThree");
	}

    public override void Attached() // Attach For Networking
    {
        state.ManagerObj.SetTransforms(transform);
    }
	
	// Update is called once per frame
    public override void SimulateOwner()   // For server authorative net
    {
        CheckPhase();
	}

    void CheckPhase()
    {
        // Check in queue which phase is active
        if (PhaseQueue.Count <= 4 && PhaseQueue.Count >= 1)
        {
            switch (PhaseQueue.Peek().ToString())
            {
                case "NoPhase":
                    //Debug.Log("No Phase");
                    break;
                case "PhaseOne":
                    //Debug.Log("Phase One");
                    break;
                case "PhaseTwo":
                    //Debug.Log("Phase Two");
                    break;
                case "PhaseThree":
                    //Debug.Log("Phase Three");
                    break;
                default:
                    //Debug.Log("Out of phases");
                    break;
            }
        }
    }
}
