using UnityEngine;
using System.Collections;

public class PhaseManager : Bolt.EntityBehaviour<IPhaseMan>
{

    [SerializeField]
    public Queue PhaseQueue;
    
    // Use this for initialization
	void Start () 
    {
        PhaseQueue = new Queue(3);
        PhaseQueue.Enqueue("Casing");
        PhaseQueue.Enqueue("Planning");
        PhaseQueue.Enqueue("Execution");
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
        if (PhaseQueue.Count <= 3 && PhaseQueue.Count >= 1)
        {
            switch (PhaseQueue.Peek().ToString())
            {
                case "Casing":
                    if (Input.GetKeyUp(KeyCode.F))
                        PhaseQueue.Dequeue();
                    break;
                case "Planning":
                    if (Input.GetKeyUp(KeyCode.F))
                        PhaseQueue.Dequeue();
                    break;
                case "Execution":
                    //Debug.Log("Phase Two");
                    break;
                default:
                    //Debug.Log("Out of phases");
                    break;
            }
        }
    }
}
