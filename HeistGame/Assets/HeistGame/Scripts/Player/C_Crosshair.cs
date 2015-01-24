using UnityEngine;
using System.Collections;

public class C_Crosshair : MonoBehaviour
{

    public Texture2D cross1;
    public Texture2D cross2;
    public Texture2D cross3;
    public Texture2D cross4;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        if ((Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0) || // aim
            (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)) // movement
        {
            // Left
            GUI.DrawTexture(new Rect(0 + (Screen.width / 2) - 120, 0 + (Screen.height / 2) - 3, 64, 32), cross1, ScaleMode.StretchToFill, true, 10.0F);
            // Right
            GUI.DrawTexture(new Rect(0 + (Screen.width / 2) + 55, 0 + (Screen.height / 2) - 3, 64, 32), cross2, ScaleMode.StretchToFill, true, 10.0F);
            // Up
            GUI.DrawTexture(new Rect(0 + (Screen.width / 2) - 16, 0 + (Screen.height / 2) - 105, 32, 64), cross3, ScaleMode.StretchToFill, true, 10.0F);
            // Down
            GUI.DrawTexture(new Rect(0 + (Screen.width / 2) - 16, 0 + (Screen.height / 2) + 65, 32, 64), cross4, ScaleMode.StretchToFill, true, 10.0F);
        }

        else
        {
            // Left
            GUI.DrawTexture(new Rect(0 + (Screen.width / 2) - 100, 0 + (Screen.height / 2) - 3, 64, 32), cross1, ScaleMode.StretchToFill, true, 10.0F);
            // Right
            GUI.DrawTexture(new Rect(0 + (Screen.width / 2) + 35, 0 + (Screen.height / 2) - 3, 64, 32), cross2, ScaleMode.StretchToFill, true, 10.0F);
            // Up
            GUI.DrawTexture(new Rect(0 + (Screen.width / 2) - 16, 0 + (Screen.height / 2) - 85, 32, 64), cross3, ScaleMode.StretchToFill, true, 10.0F);
            // Down
            GUI.DrawTexture(new Rect(0 + (Screen.width / 2) - 16, 0 + (Screen.height / 2) + 45, 32, 64), cross4, ScaleMode.StretchToFill, true, 10.0F);
        }
    }

}
