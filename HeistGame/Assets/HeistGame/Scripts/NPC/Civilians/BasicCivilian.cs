using UnityEngine;
using System.Collections;
using RAIN.Action;
using RAIN.Core; 

public class BasicCivilian : Bolt.EntityBehaviour<ICivilianState>
{
    public float myHealth;

    public GameObject myCorpse;
    public Animator myAnim;

    // Use this for initialization
    void Start()
    {
        myHealth = 100;
    }

    public override void Attached()
    {
        state.CivilianTransform.SetTransforms(transform);
        state.SetAnimator(myAnim);

        state.Animator.applyRootMotion = entity.isOwner;
    }

    // Update is called once per frame
    public override void SimulateOwner()
    {
        if (this.gameObject.GetComponent<AIRig>().AI.WorkingMemory.GetItem<string>("State") == "MOVETOTARGET")
        {
           state.Walking = true;
        }

        if (this.gameObject.GetComponent<AIRig>().AI.WorkingMemory.GetItem<int>("varAfraid") == 100)
        {
            //this.anim.SetBool("Walking", false);
            //this.anim.SetBool("Coward", true);
            state.Walking = false;
            state.Coward = true; 

        }
        if (this.gameObject.GetComponent<AIRig>().AI.WorkingMemory.GetItem<int>("varAfraid") >= 200)
        {
            //this.anim.SetBool("YelledAt", true);
            state.YelledAt = true; 
        }

        if (this.gameObject.GetComponent<AIRig>().AI.WorkingMemory.GetItem<string>("State") != "MOVETOTARGET" && this.gameObject.GetComponent<AIRig>().AI.WorkingMemory.GetItem<string>("varIdle") == "SMOKE")
        {
            //this.anim.SetBool("Walking", false);
            //this.anim.SetBool("Running", false);
            //this.anim.SetInteger("IdleNumber", 1);
            state.Walking = false;
            state.Running = false;
            state.IdleNumber = 1; 
        }
        else
        {
            //this.anim.SetInteger("IdleNumber", 0);
            state.IdleNumber = 0; 
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            //this.anim.SetBool("Running", true);
            state.Running = true; 
        }
    }

    public void TakeDamage(float dmg)
    {
        if (myHealth > 0.0f)
            myHealth += -dmg;

        if (myHealth <= 0.0f)
        {
            //GameObject clone = Instantiate(myCorpse, this.gameObject.transform.position, Quaternion.Euler(new Vector3(5, 0, 0))) as GameObject;
            //var clone = BoltNetwork.Instantiate(BoltPrefabs.SimpleCivCorpseNetworked, null, this.gameObject.transform.position, Quaternion.Euler(new Vector3(5, 0, 0)));
            BoltNetwork.Detach(this.gameObject);
        }
    }
}
