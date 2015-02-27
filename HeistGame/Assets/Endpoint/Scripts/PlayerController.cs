using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]

public class PlayerController : MonoBehaviour
{
    #region Properties and Variable

    //
    public bool IsWallRunning;
    //

    public Transform headJoint;

    public Transform cameraTrans;

    // # User Configurable
    public KeyCode crouchKey = KeyCode.LeftControl;

    // # Runtime Data
    private Vector3 moveIntention;
    private bool runIntention = false;
    private bool crouchIntention = false;
    private bool jumpIntention = false;
    //private bool    wasGrounded = false;  // TODO: @terrehbyte

    private Vector3 groundNormal = Vector3.zero;

    private float sqrMag;
    private float sqrTarMag;

    private float MaximumWalkAngle;

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
    public int JumpLimit = 1;

    public int JumpCount
    {
        get
        {
            return jumpCount;
        }
        set
        {
            jumpCount = value;
        }
    }
    private int jumpCount;

    public float gravityMultipler = 1.0f;
    public bool scaleJumpByGravity = false;




    // # Cached Implementation Details
    float playerHeight;    // Player Hull Height
    [SerializeField]
    BoxCollider hull;      // Player-World Collision Hull

    public AnimationCurve SlopeCurveModifier = new AnimationCurve(new Keyframe(-90.0f, 1.0f), new Keyframe(0.0f, 1.0f), new Keyframe(90.0f, 0.0f));

    private Vector3 bumpForce = Vector3.zero;
    private float slopeCurveEval = 0.0f;

    #endregion

    #region Methods

    // Attempts to induce jumping in the player
    private void Jump()
    {
        if (curMoveState != MovementState.GROUNDED &&
            JumpCount <= 0) return; // exit if we aren't on the ground or don't have any jumps left

        jumpIntention = false;
        rigidbody.AddForce(Vector3.up * (jumpPower * (scaleJumpByGravity ? gravityMultipler : 1.0f)), ForceMode.Impulse);
        curMoveState = MovementState.FLYING;

        JumpCount--;
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

        //Debug.Log("Head Joint's Y at: "+ camPos.y);

        cameraTrans.position = camPos;
    }

    // Checks if the player is on the ground
    bool GroundCheck()
    {
        // Create a ray that points down from the centre of the character.
        Ray ray = new Ray(transform.position, -transform.up);

        float sphereRad = Mathf.Max(hull.size.x, hull.size.z) / 2.5f;
        float sphereDist = hull.size.y / 2;

        RaycastHit[] hits = Physics.SphereCastAll(ray, sphereRad, sphereDist);
        //Debug.Log(sphereRad);
        //Debug.Log(sphereRad * 1.5f);

        if (curMoveState == MovementState.GROUNDED || rigidbody.velocity.y < jumpPower * .5f)
        {
            // Default value if nothing is detected:
            curMoveState = MovementState.FLYING;

            // Check every collider hit by the ray
            for (int i = 0; i < hits.Length; i++)
            {
                // Check it's not a trigger
                if (!hits[i].collider.isTrigger &&
                    hits[i].collider != hull)
                {
                    // The character is grounded, and we store the ground angle (calculated from the normal)
                    curMoveState = MovementState.GROUNDED;

                    // - under consideration -
                    // stick to surface - helps character stick to ground - specially when running down slopes
                    if (rigidbody.velocity.y <= 0 &&
                        rigidbody.velocity.x != 0 &&
                        !jumpIntention)
                    {
                        // also revents you from falling off :(
                        rigidbody.position = Vector3.MoveTowards(rigidbody.position, hits[i].point + Vector3.up * hull.size.y * .5f, Time.deltaTime * .5f);
                    }

                    groundNormal = hits[i].normal;

                    //rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y, rigidbody.velocity.z);
                    break;
                }
            }
        }

        // Draw a ray
        Debug.DrawRay(ray.origin, ray.direction * sphereDist, curMoveState == MovementState.GROUNDED ? Color.green : Color.red);

        return curMoveState == MovementState.GROUNDED ? true : false;
    }

    void procGroundCheck()
    {
        bool preCheck = (curMoveState == MovementState.GROUNDED) ? true : false;
        bool grounded = GroundCheck();



        if (grounded)
        {
            curMoveState = MovementState.GROUNDED;
        }
        else
        {
            curMoveState = MovementState.FLYING;
        }

        if (grounded == preCheck)
            return;

        JumpCount = JumpLimit;


    }

    #endregion

    #region Unity Events

    void Start()
    {
        rigidbody.freezeRotation = true;
        rigidbody.useGravity = false;
        playerHeight = renderer.bounds.extents.y * 2;
        //Debug.Log("Player is " + playerHeight + " units tall");
        //hull = (BoxCollider)collider;

        JumpCount = JumpLimit;
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
            }
            crouchPressed = true;   // TODO @terrehbyte, move animator code into separate script during refactor time
        }
        else
        {

        }

        crouchIntention = crouchPressed;
    }

    void FixedUpdate()
    {
        procGroundCheck();

        Vector3 targetVelocity = moveIntention;

        targetVelocity = transform.TransformDirection(targetVelocity);  // Convert direction from local space to world space
        targetVelocity *= (runIntention ? sprintSpeed : speed);          // Magnify with Player Movespeed
        targetVelocity.y = rigidbody.velocity.y;                        // preserve y (currently can't be directly influcened by player input)

        sqrMag = rigidbody.velocity.sqrMagnitude;
        rigidbody.velocity = targetVelocity;    // Override existing velocity

        //sqrTarMag = (runIntention ? sprintSpeed : speed) * (runIntention ? sprintSpeed : speed);

        bumpForce = Vector3.zero;
        if (sqrMag < sqrTarMag //&&
            //Vector3.Angle(groundNormal, Vector3.up) > 0f
            )
        {
            //Debug.Log("bbbump it up");


            //slopeCurveEval = SlopeCurveModifier.Evaluate(Vector3.Angle(groundNormal, Vector3.up));
            //bumpForce = Vector3.up * (slopeCurveEval);
            //rigidbody.AddForce(bumpForce, ForceMode.Impulse);

            // ^ terrys shitty stair code
        }


        if (moveIntention != Vector3.zero)
        {
            sqrTarMag = (runIntention ? sprintSpeed : speed) * (runIntention ? sprintSpeed : speed);
            if (runIntention)
            {

            }
            else
            {

            }
        }
        else
        {
            sqrTarMag = 0.0f;
        }

        CheckWallRunning();

        rigidbody.AddForce(Physics.gravity * gravityMultipler);    // Apply gravity





        if (jumpIntention)
        {
            // jump intention has been processed, reset to false
            jumpIntention = false;
            Jump();
        }


    }

    void LateUpdate()
    {

    }

    void OnGUI()
    {
        //GUI.Label(new Rect(0, 0, 200, 20), "RBody Vel: " + rigidbody.velocity.ToString());
        //GUI.Label(new Rect(0, 20, 200, 20), "Target Vel: " + moveIntention);

        //GUI.Label(new Rect(0, 40, 200, 20), "Jumping: " + (curMoveState != MovementState.GROUNDED ? "true" : "false"));
        //GUI.Label(new Rect(0, 60, 200, 20), "Crouching: " + (crouchIntention ? "true" : "false"));

        //GUI.Label(new Rect(0, 100, 200, 20), "Ground Normal: " + groundNormal.ToString());
        //GUI.Label(new Rect(0, 120, 200, 20), "Ground Angle: " + Vector3.Angle(groundNormal, Vector3.up));


        //GUI.Label(new Rect(0, 140, 200, 20), "SqMag: " + sqrMag.ToString());
        //GUI.Label(new Rect(0, 160, 200, 20), "SqTargetMag: " + sqrTarMag.ToString());
        //GUI.Label(new Rect(0, 180, 200, 20), "Bump Force: " + bumpForce.ToString());
        //GUI.Label(new Rect(0, 200, 200, 20), "Slope Eval: " + slopeCurveEval.ToString());
        //GUI.Label(new Rect(0, 240, 200, 20), "Jump Count: " + JumpCount.ToString());
    }

    void OnCollisionEnter(Collision other)
    {
        return;

        // TODO: create layer enum
        //if (other.gameObject.layer == 9)
        {
            foreach (ContactPoint contact in other.contacts)
            {
                print(contact.thisCollider.name + " hit " + contact.otherCollider.name);

                if (contact.point.y < transform.position.y)
                {
                    //curMoveState = MovementState.GROUNDED;
                }

                Debug.DrawRay(contact.point, contact.normal, Color.white);
            }
        }
    }

    void OnCollisionExit(Collision other)
    {
        return;

        // TODO: create layer enum
        //if (other.gameObject.layer == 9)
        {
            foreach (ContactPoint contact in other.contacts)
            {
                print(contact.thisCollider.name + " left " + contact.otherCollider.name);

                if (contact.point.y < transform.position.y)
                {
                    //curMoveState = MovementState.FLYING;
                }

                Debug.DrawRay(contact.point, contact.normal, Color.white);
            }
        }
    }

    void OnCollisionStay(Collision other)
    {

    }
    #endregion

    #region Wills stuff

    void CheckWallRunning()
    {
        if (IsWallRunning)
        {
            Debug.Log("Wall Running");
            this.gameObject.rigidbody.AddForce(Vector3.up * 8, ForceMode.Force);
        }
    }

    #endregion
}