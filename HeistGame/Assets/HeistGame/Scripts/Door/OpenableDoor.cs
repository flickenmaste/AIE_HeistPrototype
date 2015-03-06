using UnityEngine;
using System.Collections;

public class OpenableDoor : MonoBehaviour 
{
	float smooth= 5.0f;
	float DoorOpenAngle= 90.0f;
	private bool  open;
	private bool  enter;
	
	private Vector3 defaultRot;
	private Vector3 openRot;
	
	void  Start ()
	{
		defaultRot = transform.eulerAngles;
		openRot = new Vector3 (defaultRot.x, defaultRot.y + DoorOpenAngle, defaultRot.z);
	}
	
	//Main function

	void  Update ()
		{

				if (enter == true && !open) 
				{
						if (Input.GetAxis ("Mouse ScrollWheel") > 0) {
								transform.Rotate (Vector3.up * 5.0f, Space.Self);
						}
						if (Input.GetAxis ("Mouse ScrollWheel") < 0) {
								transform.Rotate (Vector3.down * 5.0f, Space.Self);
						}
				}

				if (open) 
				{
						//Open door

						transform.eulerAngles = Vector3.Slerp (transform.eulerAngles, openRot, Time.deltaTime * smooth);
				} 

				else 
				{
						//Close door

						transform.eulerAngles = Vector3.Slerp (transform.eulerAngles, defaultRot, Time.deltaTime * smooth);
				}

				if (Input.GetKeyDown ("e") && enter) 
				{
						open = !open;
				}
		}
		
	
	void  OnGUI ()
	{
		if(enter)
		{
			GUI.Label(new Rect(Screen.width/2 - 75, Screen.height - 100, 150, 30), "Press 'E' to open the door");
		}
	}
	
	//Activate the Main function when player is near the door

	void  OnTriggerEnter ( Collider other  )
	{
		if (other.gameObject.tag == "Player") 
		{
			enter = true;
		}
	}
	
	//Deactivate the Main function when player is away from door

	void  OnTriggerExit ( Collider other  )
	{
		if (other.gameObject.tag == "Player") 
		{
			enter = false;
		}
	}
}




































































/*
 * using UnityEngine;
using System.Collections;

public class OpenableDoor : MonoBehaviour 
{
	float smooth= 5.0f;
	float DoorOpenAngle= 90.0f;
	private bool  open;
	private bool  enter;
	
	private Vector3 defaultRot;
	private Vector3 openRot;
	
	void  Start ()
	{
		defaultRot = transform.eulerAngles;
		openRot = new Vector3 (defaultRot.x, defaultRot.y + DoorOpenAngle, defaultRot.z);
	}
	
	//Main function

	void  Update (){
		if(open){

			//Open door

			transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, openRot, Time.deltaTime * smooth);
		}
		else
		{
			//Close door

			transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, defaultRot, Time.deltaTime * smooth);
		}
		
		if(Input.GetKeyDown("e") && enter){
			open = !open;
		}
	}
	
	void  OnGUI ()
	{
		if(enter)
		{
			GUI.Label(new Rect(Screen.width/2 - 75, Screen.height - 100, 150, 30), "Press 'E' to open the door");
		}
	}
	
	//Activate the Main function when player is near the door

	void  OnTriggerEnter ( Collider other  )
	{
		if (other.gameObject.tag == "Player") 
		{
			enter = true;
		}
	}
	
	//Deactivate the Main function when player is away from door

	void  OnTriggerExit ( Collider other  )
	{
		if (other.gameObject.tag == "Player") 
		{
			enter = false;
		}
	}
} */
