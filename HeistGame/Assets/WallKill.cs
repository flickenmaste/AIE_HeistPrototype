﻿using UnityEngine;
using System.Collections;

public class WallKill : Bolt.EntityBehaviour<IWallState>
{

	public Transform Player;

	public float SpeedX;

	public float SpeedY;

	public float SpeedZ;

    public bool started;

	void OnTriggerEnter(Collider coll)
	{
		//if (coll.attachedRigidbody == Player.rigidbody)
						//Application.LoadLevel (Application.loadedLevel);
	}

    public override void Attached()
    {
        state.WallTransform.SetTransforms(transform);
    }

	// Use this for initialization
	void Start () 
    {
        started = false;
	}
	
	// Update is called once per frame
    public override void SimulateOwner()
    {
        if (!started)
        {
            this.transform.rigidbody.AddForce(SpeedX, SpeedY, SpeedZ);
            started = true;
        }   
	}
}
