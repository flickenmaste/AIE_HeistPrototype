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

	//		anim.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
	//		anim.SetFloat("Vertical", Input.GetAxis("Vertical"));
		if(Input.GetKey(KeyCode.W))
		{
		anim.SetBool ("Walking", true);
		}
		else
		{
			anim.SetBool ("Walking", false);
		}

		if(Input.GetKey(KeyCode.S))
		{
			anim.SetBool ("Backward", true);
		}
		else
		{
			anim.SetBool ("Backward", false);
		}

	}
}
