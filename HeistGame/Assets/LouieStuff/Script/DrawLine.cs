using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawLine : MonoBehaviour {

	public LineRenderer lineRender;
	private int numberOfPoints = 0;
	public Vector3 check;
	public Camera MapCamera;

    List<LineRenderer> Lines;
	// Use this for initialization
	void Start () {
        Lines = new List<LineRenderer>();
	}

	void DeleteLines()
	{
        if (Input.GetKeyDown(KeyCode.P) && Lines.Count != 0)
       {
           var deletinglineobjecy = Lines[Lines.Count-1];
           Lines.RemoveAt(Lines.Count-1);
           Destroy(deletinglineobjecy.gameObject);
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
			mousePos.z = 1.0f;
			Vector3 worldPos = MapCamera.ScreenToWorldPoint(mousePos);
			lineRender.SetPosition(numberOfPoints - 1, worldPos);
			lineRender.transform.localScale = new Vector3(500.0f,500.0f,500.0f);
		}
		else if( Input.GetMouseButtonUp(0)){
            Lines.Add(Instantiate(lineRender));
			numberOfPoints = 0;
			lineRender.SetVertexCount(0);
		}
	}
}
