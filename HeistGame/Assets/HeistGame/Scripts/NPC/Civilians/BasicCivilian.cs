using UnityEngine;
using System.Collections;

public class BasicCivilian : Bolt.EntityBehaviour<ICivilianState>
{

    public float myHealth;

    public GameObject myCorpse;

    // Use this for initialization
    void Start()
    {
        myHealth = 100;
    }

    public override void Attached()
    {
        state.CivilianTransform.SetTransforms(transform);
    }

    // Update is called once per frame
    public override void SimulateController()
    {

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
