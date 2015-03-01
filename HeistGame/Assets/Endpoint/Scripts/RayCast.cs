using UnityEngine;
using System.Collections;

public class RayCast : MonoBehaviour {

	public GameObject player;
	public LayerMask mask;

	Ray ray;
	RaycastHit hit;
	Vector3 norm;

	float ledgeTimer;





	IEnumerator SnapToLedge(Vector3 playerPos, Vector3 hitPoint) {


		Vector3 pointBuffer = new Vector3 (hitPoint.x, hitPoint.y, hitPoint.z);

		float dist = Vector3.Distance(player.transform.position, pointBuffer);
		//Debug.Log (dist);
		if(dist < 0.1f) {
			player.transform.position = pointBuffer;
			//Debug.Log ("Coroutine Finished!");
		}

		while (player.transform.position != pointBuffer) {
			player.transform.position = Vector3.Lerp(player.transform.position, pointBuffer, Time.deltaTime * 15);
			yield return null;
		}


	}

	void Start() {
	}

	void Update() {


		ledgeTimer += 1 * Time.deltaTime;
		Debug.Log (ledgeTimer);

		Vector3 fwd = transform.TransformDirection(Vector3.forward);
		Debug.DrawRay(transform.position, fwd);


		//RaycastHit hit = Physics.Raycast(transform.position,fwd, 1);

		ray = new Ray(transform.position, fwd);
		//hit = new RaycastHit();






		if (Physics.Raycast (ray, out hit, 1, mask)) {

			if (hit.collider.CompareTag ("Ledge")) {
				//Debug.Log ("LEDGE HIT!");
				player.GetComponent<PlayerController> ().facingLedge = true;

				if(!player.GetComponent<PlayerController>().snapToWall && ledgeTimer > 0.25f) {
					player.GetComponent<Rigidbody>().velocity = Vector3.zero;
					player.GetComponent<Rigidbody>().isKinematic = true;
					//player.transform.position = hit.point;

					StartCoroutine(SnapToLedge(player.transform.position, hit.point));
					player.GetComponent<PlayerController>().snapToWall = true;
				} 


			} else {
				player.GetComponent<Rigidbody>().isKinematic = false;
				player.GetComponent<PlayerController>().snapToWall = false;
				StopCoroutine ("SnapToLedge");
			}
		}
		if(Input.GetKey(KeyCode.Space) && player.GetComponent<PlayerController>().snapToWall) {
			player.GetComponent<Rigidbody>().isKinematic = false;
			player.GetComponent<PlayerController>().snapToWall = false;
			StopCoroutine ("SnapToLedge");
			ledgeTimer = 0;
				
		}


	}

}
