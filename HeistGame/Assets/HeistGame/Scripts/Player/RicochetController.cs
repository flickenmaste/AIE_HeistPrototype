using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]

public class RicochetController : MonoBehaviour
{
    #region Properties and Variables

    public Transform headJoint;

    public Transform cameraTrans;

    // # User Configurable
    public KeyCode crouchKey = KeyCode.LeftControl;

    // # Runtime Data
    private Vector3 moveIntention;
    private bool runIntention = false;
    private bool crouchIntention = false;
    private bool jumpIntention = false;

    // Indicates movement state
    public enum MovementState
    {
        GROUNDED,
        FLYING
    }

    // Current Movement State
    private MovementState curMoveState = MovementState.GROUNDED;

    // # Attributes
    [SerializeField]
    private float speed = 10.0f;    // base movement speed
    [SerializeField]
    private float sprintSpeed = 15.0f;    // sprint speed (@terrehbyte: consider making this a multiplier)
    private float airAccel = 10.0f;    // air accel
    private float jumpPower = 5.0f;     // force to apply for jumping

    // # Cached Implementation Details
    float playerHeight;    // Player Hull Height
    BoxCollider hull;      // Player-World Collision Hull

    #endregion

    #region Methods

    // Attempts to induce jumping in the player
    private void Jump()
    {
        if (curMoveState != MovementState.GROUNDED) return; // exit if we aren't on the ground

        jumpIntention = false;
        rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        curMoveState = MovementState.FLYING;
    }

    // Retrieves player input as a Vector3
    Vector3 GetInput()
    {
        // Retrieve current input values
        Vector3 MovementVelocity = new Vector3(Input.GetAxisRaw("Horizontal"),  // X    
                                               0,                            // Y
                                               Input.GetAxisRaw("Vertical"));   // Z (think of this as a D-Pad)

        return MovementVelocity;
    }

    void syncCameraToJoint()
    {
        Vector3 camPos = cameraTrans.position;
        camPos.y = headJoint.position.y;

        Debug.Log("Head Joint's Y at: " + camPos.y);

        cameraTrans.position = camPos;
    }

    #endregion

    #region Unity Events

    void Start()
    {
        rigidbody.freezeRotation = true;
        rigidbody.useGravity = false;
        playerHeight = renderer.bounds.extents.y * 2;
        Debug.Log("Player is " + playerHeight + " units tall");
        //hull = (BoxCollider)collider;
    }

    void Update()
    {
        syncCameraToJoint();

        // Poll user's state
        moveIntention = GetInput();
        runIntention = Input.GetKey(KeyCode.LeftShift);
        jumpIntention = Input.GetButtonDown("Jump") || jumpIntention; // @terrehbyte: this looks cool but seems like one of those dick-move things

        bool crouchPressed = false;
        if (Input.GetKey(crouchKey))    // TODO: @emlowry, please write the event-based input manager
        {
            if (!crouchIntention)
            {
                // anims
            }
            crouchPressed = true;   // TODO @terrehbyte, move animator code into separate script during refactor time
        }
        else
        {
            // anims
        }

        crouchIntention = crouchPressed;
    }

    void FixedUpdate()
    {
        Vector3 targetVelocity = moveIntention;

        targetVelocity = transform.TransformDirection(targetVelocity);  // Convert direction from local space to world space
        targetVelocity *= (runIntention ? sprintSpeed : speed);          // Magnify with Player Movespeed
        targetVelocity.y = rigidbody.velocity.y;                        // preserve y (currently can't be directly influcened by player input)

        if (moveIntention != Vector3.zero)
        {
            if (runIntention)
            {
                // anims
            }
            else
            {
                // anims
            }
        }
        else
        {
            // anims
        }

        rigidbody.velocity = targetVelocity;    // Override existing velocity

        rigidbody.AddForce(Physics.gravity);    // Apply gravity

        if (jumpIntention)
        {
            // jump intention has been processed, reset to false
            jumpIntention = false;
            Jump();
        }
    }

    void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 200, 20), "RBody Vel: " + rigidbody.velocity.ToString());
        GUI.Label(new Rect(0, 20, 200, 20), "Target Vel: " + moveIntention);

        GUI.Label(new Rect(0, 40, 200, 20), "Jumping: " + (curMoveState != MovementState.GROUNDED ? "true" : "false"));
        GUI.Label(new Rect(0, 60, 200, 20), "Crouching: " + (crouchIntention ? "true" : "false"));
    }

    void OnCollisionEnter(Collision other)
    {
        // TODO: create layer enum
        if (other.gameObject.layer == 9)
        {
            foreach (ContactPoint contact in other.contacts)
            {
                print(contact.thisCollider.name + " hit " + contact.otherCollider.name);

                if (contact.point.y < transform.position.y)
                {
                    curMoveState = MovementState.GROUNDED;
                }

                Debug.DrawRay(contact.point, contact.normal, Color.white);
            }
        }
    }

    void OnCollisionExit(Collision other)
    {
        // TODO: create layer enum
        if (other.gameObject.layer == 9)
        {
            foreach (ContactPoint contact in other.contacts)
            {
                print(contact.thisCollider.name + " left " + contact.otherCollider.name);

                if (contact.point.y < transform.position.y)
                {
                    curMoveState = MovementState.FLYING;
                }

                Debug.DrawRay(contact.point, contact.normal, Color.white);
            }
        }
    }
    #endregion
}