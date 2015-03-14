using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour 
{


	public Animator anim;
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{

			anim.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
			anim.SetFloat("Vertical", Input.GetAxis("Vertical"));

	}
}
