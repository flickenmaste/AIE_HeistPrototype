using UnityEngine;
using System.Collections;

public class OpenableDoor : MonoBehaviour 
{
	float smooth= 5.0f;
	float DoorOpenAngle= 90.0f;
	public bool  open;
	public bool  enter;
	public bool  auto;
	
	private Quaternion defaultRot;
	private Quaternion openRot;
	private Quaternion currentRot;
	public Vector3 defaultRotEular;
	public Vector3 openRotEular;
	
	void  Start ()
	{
		defaultRot = transform.localRotation;
		openRot = Quaternion.Euler(defaultRot.x, defaultRot.y + DoorOpenAngle, defaultRot.z);
	}
	
	//Main function

	void  Update ()
		{
		defaultRotEular = defaultRot.eulerAngles;
		openRotEular = openRot.eulerAngles;

	if (enter) 
	{
			//mouse scrolling to open
			if (Input.GetAxis ("Mouse ScrollWheel") > 0 && (currentRot.eulerAngles.y < openRot.eulerAngles.y)) {
				auto = false;
				transform.Rotate (Vector3.up * 5.0f, Space.Self);
				currentRot = transform.localRotation;

		}
			if (Input.GetAxis ("Mouse ScrollWheel") < 0 && (currentRot.eulerAngles.y > defaultRot.eulerAngles.y)) {
				auto = false;
				transform.Rotate (Vector3.down * 5.0f, Space.Self);
				currentRot = transform.localRotation;

		}
}

				if (open && auto) {
						//Open door
				transform.localRotation = Quaternion.Slerp(transform.localRotation, openRot, Time.deltaTime * smooth);
						
				} 
	
				//Close door
				if (open == false && auto) 
				{
				transform.localRotation = Quaternion.Slerp(transform.localRotation, defaultRot, Time.deltaTime * smooth);
				}


			if (Input.GetKeyDown ("e") && enter && open ==true) 
			{
				auto = true;
				open = false;
			}

			if (Input.GetKeyDown ("e") && enter && open ==false) 
			{
				auto = true;
				open = true;
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
