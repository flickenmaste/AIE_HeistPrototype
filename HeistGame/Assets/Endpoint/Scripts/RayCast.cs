using UnityEngine;
using System.Collections;

public class RayCast : MonoBehaviour {

	Ray ray;
	RaycastHit hit;


	void Start() {
	}

	void Update() {

		Vector3 fwd = transform.TransformDirection(Vector3.forward);
		Debug.DrawRay(transform.position, fwd);


		//RaycastHit hit = Physics.Raycast(transform.position,fwd, 1);

		ray = new Ray(transform.position, fwd);
		hit = new RaycastHit();


		if(Physics.Raycast(ray, out hit, 1)) {

			if(hit.collider.CompareTag("Ledge")) {
				Debug.Log ("LEDGE HIT!");
			}
		}

	}

}
