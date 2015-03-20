using UnityEngine;
using System.Collections;
using RAIN.Action;
using RAIN.Core;


public enum CopState
{
    IDLE,
    MOVEING,
    DRAWGUN,
    FIREGUN
};

public class CopAnimatorState : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        string CopState = this.GetComponent<AIRig>().AI.WorkingMemory.GetItem<string>("varState");

        float CopSpeed = this.GetComponent<AIRig>().AI.WorkingMemory.GetItem<float>("varSpeed");

        this.GetComponent<Animator>().;
	}
}
