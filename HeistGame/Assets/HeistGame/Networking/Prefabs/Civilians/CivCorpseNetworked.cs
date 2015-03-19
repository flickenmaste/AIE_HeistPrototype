using UnityEngine;
using System.Collections;

public class CivCorpseNetworked : Bolt.EntityBehaviour<ICivilianState>
{

	// Use this for initialization
	void Start () {
	
	}

    public override void Attached()
    {
        state.CivCorpseTransform.SetTransforms(transform);
    }
	
	// Update is called once per frame
    public override void SimulateOwner()
    {

    }
}
