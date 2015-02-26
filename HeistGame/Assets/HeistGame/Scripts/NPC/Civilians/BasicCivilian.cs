using UnityEngine;
using System.Collections;

public class BasicCivilian : NPC {

    public float myHealth;

    public GameObject myCorpse;

    // Use this for initialization
    void Start()
    {
        myHealth = 100;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(float dmg)
    {
        if (myHealth > 0.0f)
            myHealth += -dmg;

        if (myHealth <= 0.0f)
        {
            GameObject clone = Instantiate(myCorpse, this.gameObject.transform.position, Quaternion.Euler(new Vector3(5, 0, 0))) as GameObject;
            Destroy(this.gameObject, 0);
        }
    }
}
