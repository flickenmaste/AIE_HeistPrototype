using UnityEngine;
using System.Collections;

public class DrawLine : MonoBehaviour {

	public LineRenderer lineRender;
	private int numberOfPoints = 0;
	public Vector3 check;
	public Camera MapCamera;
	// Use this for initialization
	void Start () {
		
	}

	void DeleteLines()
	{
		if (Input.GetMouseButtonDown (1)) {
			RaycastHit hit;
			check = Input.mousePosition;
			Debug.DrawLine(Input.mousePosition,Vector3.down * 100,Color.cyan,5.0f);
			if (Physics.Raycast (Input.mousePosition,Vector3.down, out hit, 1000)) {
				Debug.DrawLine(Input.mousePosition,hit.point,Color.cyan,5.0f);
			}
		}
	}
	// Update is called once per frame
	void Update () {
		DeleteLines ();
		if( Input.GetMouseButton(0) ) {
			numberOfPoints++;
			lineRender.SetVertexCount( numberOfPoints );
			Vector3 mousePos = new Vector3(0,0,0);
			mousePos = Input.mousePosition;
			mousePos.z = 10.0f;
			//mousePos.z = 1.0f;
			Vector3 worldPos = MapCamera.ScreenToWorldPoint(mousePos);
			//lineRender.SetPosition(numberOfPoints - 1, new Vector3(worldPos.x+200.0f,worldPos.y - 10.0f,worldPos.z));
			lineRender.SetPosition(numberOfPoints - 1, worldPos);
			//lineRender.transform.localScale = new Vector3(.5f,.5f,.5f);
			lineRender.transform.localScale = new Vector3(500.0f,500.0f,500.0f);
		}
		else if( Input.GetMouseButtonUp(0)){
			LineRenderer Line = Instantiate(lineRender);



			numberOfPoints = 0;
			lineRender.SetVertexCount(0);
		}
	}
}
