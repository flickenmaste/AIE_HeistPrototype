//#define BOX_COLLIDER
#define CAPSULE_COLLIDER    // we are using a capsule collider for the hull

using System;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]

public class PlayerController : Bolt.EntityBehaviour<IPlayerState>
{
    #region Properties and Variables

    // Indicates movement state
    public enum MovementState
    {
        GROUNDED,
        FLYING,
        FLOATING
    }
    public enum ParkourState
    {
        WALLRUN,
        VAULTING,   // to be implemented
        NOTHING
    }

    // # Attributes
    [Header("Base Attributes")]
    public float baseSpeed = 10.0f;    // base movement speed
    public float sprintSpeed = 15.0f;    // sprint speed (@terrehbyte: consider making this a multiplier)
    [Range(0f, 1f)]
    public float runstepLengthen; // for the footstep sounds
    //private float   airAccel    = 10.0f;    // air accel
    public float jumpHeight = 5.0f;     // force to apply for jumping
    public float walkJumpBoost = 1.0f;
    public float sprintJumpBoost = 5.0f;
    public int JumpLimit = 1;
    public float wallRunTimeLimit = 5.0f;
    public float maxGroundAngle = 75f;
    public float cameraTiltAngle = 15f;
    [SerializeField]
    bool drawDebugData;

    // ## Audio
    //[SerializeField]
    //private AudioClip[] footstepSounds; // will be randomly selected from
    //[SerializeField]
    //private AudioClip jumpSound;
    //[SerializeField]
    //private AudioClip landSound;
    [SerializeField]
    private float stepInterval;
    private float footstepSpeed;
    private float stepCycle = 0f;
    private float nextStep = 0f;

    // # User Configurable
    [Header("User Configurable")]
    public KeyCode crouchKey = KeyCode.LeftControl;
    public KeyCode sprintKey = KeyCode.LeftShift;

    // # Runtime Data

    // ## User Influencable
    private Vector3 moveIntention;
    private bool sprintIntention = false;
    private bool crouchIntention = false;
    private bool jumpIntention = false;
    private ParkourState parkourIntention = ParkourState.NOTHING;

    private Vector3 prevTargetVelocity;

    // ## Regular Movement Data

    //private bool    wasGrounded = false;  // TODO: @terrehbyte
    private MovementState curMoveState = MovementState.GROUNDED;

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

    private Vector3 bumpForce = Vector3.zero;
    private float slopeCurveEval;

    private float groundAngle = 0.0f;
    private Vector3 groundNormal = Vector3.zero;

    private float sqrMag;
    private float sqrTarMag;

    // # Parkour Movement Data
    private Collider previousWallRunCollider;
    private Collider currentWallRunCollider;
    private Vector3 currentWallRunPoint;
    private Vector3 relativeWallPosition;
    private bool onWall;
    private float wallRunTime;  // time spent running on wall
    public float NormalizedWallRunTime
    {
        get { return wallRunTime / wallRunTimeLimit; }
    }

    [Header("Implementation Tweaks")]
    public Vector3 gravityModifier;
    public Vector3 LocalGravity
    {
        get { return Physics.gravity + gravityModifier; }
    }

    public AnimationCurve SlopeCurveModifier = new AnimationCurve(new Keyframe(-90.0f, 1.0f), new Keyframe(0.0f, 1.0f), new Keyframe(90.0f, 0.0f));
    public AnimationCurve WallCurveModifier = new AnimationCurve(); // TODO: @terrehbyte, script out the default keys

    public float wallRunForce = 2.0f;

    [SerializeField]
    private float wallCheckRayLen = 1.1f;
    [SerializeField]
    private float groundRayLen = 1.1f;

    [Header("View Components")]
    //public Animator roboController;
    public Transform headJoint;
    public Transform cameraTrans;
    public GameObject myCam;

    // # Cached Implementation Details
#if BOX_COLLIDER
    BoxCollider hull;       // Player-World Collision Hull
#elif CAPSULE_COLLIDER
    CapsuleCollider hull;   // Player-World Collision Hull
#endif
    Rigidbody rbody;
    Transform trans;
    AudioSource asource;
    #endregion

    #region Methods

    // Attempts to induce jumping in the player
    private void Jump()
    {
        jumpIntention = false;  // Clear the jump flag

        // # Check for jump eligibility
        if (JumpCount <= 0)     // Check if there are any jumps left
            return;

        // Copy existing velocity
        Vector3 finalVelocity = rbody.velocity;

        float jumpForce = Mathf.Sqrt(2 * jumpHeight * (LocalGravity.y * -1f));
        finalVelocity.y = Mathf.Max(jumpForce, finalVelocity.y);

        // Apply new velocity
        rbody.velocity = finalVelocity;

        // Assign data
        curMoveState = MovementState.FLYING;
        wallRunTime = 0f;   // jumping should reset wall running time
        JumpCount--;

        //## Audio
        PlayJumpSound();
    }

    // Retrieves player input as a Vector3
    Vector3 GetInput()
    {
        // Retrieve current input values
        Vector3 MovementVelocity = new Vector3(Input.GetAxisRaw("Horizontal"),  // X    
                                               0,                               // Y
                                               Input.GetAxisRaw("Vertical"));   // Z (think of this as a D-Pad)

        return MovementVelocity;
    }

    //TODO: make use of a coroutine and smooth this shit out


    void tiltCamera()   //tilt the camera based on wall run
    {
        Vector3 eulerRot = cameraTrans.rotation.eulerAngles; // because I can't work in Quats

        if (parkourIntention != ParkourState.WALLRUN)
        {
            // because I can't work in Quats
            //eulerRot.z = 0f;
            //cameraTrans.rotation = Quaternion.Euler(eulerRot); // i swear there's a better way to do this
            return;
        }

        // TILLLLLLT
        //cameraTrans.localRotation = Quaternion.Euler(eulerRot);

        //relativeWallPosition = trans.InverseTransformVector(currentWallRunPoint); // fuck dis bs

        eulerRot.z = cameraTiltAngle;   // TILT!!! yeah i'll fix it later

        //TODO: is it ternary operator time? <3333
        if (moveIntention.x < 0f)        // LEFT
        {
            eulerRot.z *= -1f;  // invert the angle

        }
        else if (moveIntention.x > 0f)   // RIGHT
        {

        }
        else // DIRECTLY AHEAD
        {
            // well wtf how
        }

        cameraTrans.rotation = Quaternion.Euler(eulerRot); // like I said I swear there's a better way
    }
    void syncCameraToJoint()
    {
        Vector3 camPos = cameraTrans.position;
        camPos.y = headJoint.position.y;

        cameraTrans.position = camPos;
    }

    // TODO: make this function const, not modifying any member fields
    // Checks if the player is on the ground
    bool GroundCheck()
    {
        // Create a ray that points down from the centre of the character.
        Ray ray = new Ray(trans.position, -trans.up);

#if BOX_COLLIDER
        float sphereRad = Mathf.Max(hull.size.x, hull.size.z) / 2.5f;
#elif CAPSULE_COLLIDER
        float sphereRad = hull.radius;
#endif
        RaycastHit[] hits = Physics.SphereCastAll(ray, sphereRad, groundRayLen);

        if (curMoveState == MovementState.GROUNDED ||   // if we were grounded
            rbody.velocity.y < jumpHeight * .5f)    // or falling down
        {
            // Default value if nothing is detected:
            curMoveState = MovementState.FLYING;

            // Check every collider hit by the ray
            for (int i = 0; i < hits.Length; i++)
            {
                // Check it's not a trigger
                if (!hits[i].collider.isTrigger &&
                    hits[i].collider != hull &&
                    Vector3.Angle(hits[i].normal, Vector3.up) < maxGroundAngle)
                {
                    // The character is grounded, and we store the ground angle (calculated from the normal)
                    curMoveState = MovementState.GROUNDED;
                    groundNormal = hits[i].normal;

                    //Debug.Log(hits[i].collider.name);

                    // stick to surface - helps character stick to ground - specially when running down slopes
                    if (rbody.velocity.y <= 0 &&
                        rbody.velocity.x != 0 &&
                        !jumpIntention)
                    {
#if BOX_COLLIDER
                        rbody.position = Vector3.MoveTowards(rbody.position, hits[i].point + Vector3.up * hull.size.y * .5f, Time.deltaTime * .5f);
#elif CAPSULE_COLLIDER
                        rbody.position = Vector3.MoveTowards(rbody.position, hits[i].point + Vector3.up * hull.height * .5f, Time.deltaTime * .5f);
#endif

                    }
                    break;  // we've found our ground, let's break
                }
            }
        }

        // Draw a ray
        Debug.DrawRay(ray.origin, ray.direction * groundRayLen, curMoveState == MovementState.GROUNDED ? Color.green : Color.red);

        return curMoveState == MovementState.GROUNDED;
    }

    bool crouchCheck()
    {
        bool crouchPressed = false;
        if (Input.GetKey(crouchKey))
        {
            // If we weren't already crouching, then trigger the transition into that animation
            if (!crouchIntention)
            {
                //roboController.SetTrigger("Crouching");
            }
            crouchPressed = true;   // TODO @terrehbyte, move animator code into separate script during refactor time
            //roboController.SetBool("Crouch", true);
        }
        else
        {
            //roboController.SetBool("Crouch", false);
        }

        return crouchPressed;
    }

    void buildMovementState()
    {
        groundNormal = Vector3.zero;
        bool wasGrounded = curMoveState == MovementState.GROUNDED;
        bool isGrounded = GroundCheck();

        if (isGrounded)
        {
            wallRunTime = 0.0f;     // we're grounded, so reset the time we've been wall running
            curMoveState = MovementState.GROUNDED;

            previousWallRunCollider = null;
            currentWallRunCollider = null;
            //roboController.SetBool("Grounded", true);

            //## Audio
            if (!wasGrounded)
            {
                PlayLandingSound();
            }
        }
        else
        {
            curMoveState = MovementState.FLYING;
            //roboController.SetBool("Grounded", false);
        }

        // return if no change
        if (isGrounded == wasGrounded)
            return;

        // otherwise, a change has occurred
        JumpCount = JumpLimit;
    }
    void buildParkourState()
    {
        if (parkourIntention == ParkourState.WALLRUN &&
            NormalizedWallRunTime > 1)
        {
            previousWallRunCollider = currentWallRunCollider;
            currentWallRunCollider = null;
        }

        parkourIntention = ParkourState.NOTHING;

        // handle potential for wall running
        if (moveIntention.x != 0.0f &&
            curMoveState != MovementState.GROUNDED)
        {
            Vector3 rayDir = trans.TransformVector(new Vector3(moveIntention.x, 0f, 0f));

            Debug.DrawRay(trans.position, rayDir * wallCheckRayLen);

            // Check for walls
            Ray wallRay = new Ray(trans.position, rayDir);
            RaycastHit[] rayHits = Physics.RaycastAll(wallRay, wallCheckRayLen);

            for (int i = 0; i < rayHits.Length; i++)
            {
                if (rayHits[i].collider != hull &&
                    rayHits[i].collider != GetComponent<BoxCollider>() &&
                    rayHits[i].collider != previousWallRunCollider)
                {
                    currentWallRunCollider = rayHits[i].collider;
                    currentWallRunPoint = rayHits[i].point;
                    parkourIntention = ParkourState.WALLRUN;

                    wallRunTime += Time.deltaTime;
                }
            }
        }
    }
    void buildMecanimState()
    {
        //if (!roboController)
            //return;

        // pass data to animator
        if (moveIntention != Vector3.zero)
        {
            // consider refactoring this data to handle numbers 
            if (sprintIntention)
            {
                //roboController.SetBool("Running", true);
                //roboController.SetBool("Sprinting", true);
            }
            else
            {
                //roboController.SetBool("Running", true);
                //roboController.SetBool("Sprinting", false);
            }
        }
        else
        {
            //roboController.SetBool("Running", false);
            //roboController.SetBool("Sprinting", false);
        }
    }

    // ## Audio ---------------------------------------------------------------------------
    void ProgressStepCycle(float speed) // based on Unity sample character controller
    {
        if (rbody.velocity.sqrMagnitude > 0 && (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0))
        {
            stepCycle += (rbody.velocity.magnitude + (speed * (!sprintIntention ? 1f : runstepLengthen))) * Time.fixedDeltaTime;
        }
        if (!(stepCycle > nextStep)) return;

        nextStep = stepCycle + stepInterval;

        PlayFootStepAudio();
    }

    void PlayFootStepAudio() // based on Unity sample character controller
    {
        if (!GroundCheck()) return;
        //int n = UnityEngine.Random.Range(1, footstepSounds.Length);

        // if not supported, exit
        if (asource == null ||
            !asource.enabled)
            return;

        //asource.clip = footstepSounds[n];
        //asource.PlayOneShot(asource.clip);
        // move picked sound to index 0 so it's not picked next time
        //footstepSounds[n] = footstepSounds[0];
        //footstepSounds[0] = asource.clip;
    }

    void PlayJumpSound()
    {
        if (!asource)
            return;

        //asource.clip = jumpSound;
        //asource.Play();
    }

    void PlayLandingSound()
    {
        if (!asource)
            return;

        //asource.clip = landSound;
        //asource.Play();
        nextStep = stepCycle + 0.5f;
    }

    // ## End Audio ----------------------------------------------------------------------

    #endregion

    #region Unity Events

    void Start()
    {
        // acquire hull for stairs
#if BOX_COLLIDER
        hull = (BoxCollider)collider;
#elif CAPSULE_COLLIDER
        hull = GetComponent<CapsuleCollider>();
#endif
        rbody = GetComponent<Rigidbody>();
        trans = GetComponent<Transform>();
        asource = GetComponent<AudioSource>();

        // initialize properties
        JumpCount = JumpLimit;

        stepCycle = 0f;
        nextStep = stepCycle / 2f;

    }

    public override void Attached()
    {
        state.PlayerTransform.SetTransforms(transform);
    }

    public override void SimulateController()
    {
        IPlayerCommandInput input = PlayerCommand.Create();
        myCam.SetActive(true);
        moveIntention = GetInput();
        sprintIntention = Input.GetKey(sprintKey);
        jumpIntention = Input.GetButtonDown("Jump") || jumpIntention;
        crouchIntention = crouchCheck();

        input.Move = moveIntention;

        entity.QueueInput(input);
    }

    public override void ExecuteCommand(Bolt.Command command, bool resetState)
    {
        PlayerCommand cmd = (PlayerCommand)command;

        FixedUpdate();

        cmd.Result.Position = this.gameObject.transform.position;
        cmd.Result.Velocity = rbody.velocity;
        cmd.Result.IsGrounded = jumpIntention;
    }

    void FixedUpdate()
    {
        Vector3 targetVelocity;

        // build condition of player character
        if (curMoveState != MovementState.FLOATING)
        {
            buildMovementState();
            //buildParkourState();
        }
        else // we're floating, is there anything we need to check?
        {
            if (prevTargetVelocity.y < 0f)          // if we tried to go down
            {
                if (Math.Abs(rbody.velocity.y) < 0.0001f)     // but couldn't get anywhere...
                {
                    buildMovementState();   // rebuild movement state. maybe we're on the ground?
                }
            }
        }

        // # Process Regular Movement

        // ## Process Ground Movement                   // TODO: convert to a switch
        if (curMoveState == MovementState.FLOATING)
        {
            targetVelocity.x = moveIntention.x;
            targetVelocity.y = (cameraTrans.forward.y >= 0 ? 1f : -1f) * (moveIntention.z);
            targetVelocity.z = 0f;  // always

            targetVelocity = trans.TransformDirection(targetVelocity);
            targetVelocity *= baseSpeed;
        }
        else // we're on the ground / in the air with air control!!!
        {
            targetVelocity = moveIntention;
            targetVelocity = trans.TransformDirection(targetVelocity);  // Convert direction from local space to world space
            targetVelocity *= (sprintIntention ? sprintSpeed : baseSpeed);  // Magnify with Player Movespeed
            targetVelocity.y = rbody.velocity.y;                        // preserve y (currently can't be directly influcened by player input)

            if (curMoveState == MovementState.GROUNDED) // player is authorative of their movement on the ground
            {

            }
            else if (curMoveState == MovementState.FLYING)
            {
                // todo? implement air input as acceleration?

                // let the rbody fly if we didn't touch anything
                if (moveIntention == Vector3.zero)
                {
                    targetVelocity.x = rbody.velocity.x;
                    targetVelocity.z = rbody.velocity.z;
                }
            }
        }

        // ## Populate/reset run-time data
        sqrMag = rbody.velocity.sqrMagnitude;   // record previous real sqr magnitude velocity
        sqrTarMag = 0.0f;
        bumpForce = Vector3.zero;
        slopeCurveEval = 0.0f;
        groundAngle = Vector3.Angle(groundNormal, Vector3.up);
        if (moveIntention != Vector3.zero)
            sqrTarMag = (sprintIntention ? sprintSpeed : baseSpeed) * (sprintIntention ? sprintSpeed : baseSpeed);  // consider refactoring moveIntention to show mag

        // ## Pass Data to Rigidbody Velocity
        rbody.velocity = targetVelocity;    // Override existing velocity

        // # Process Additional Movement Possibilities
        if (parkourIntention == ParkourState.WALLRUN &&    // are we wallrunning?
            NormalizedWallRunTime <= 1f)                   // have we not yet exhausted our wall running time?
        {
            float wallRunPeriod = WallCurveModifier.Evaluate(wallRunTime);

            var appliedForce = (trans.forward + Vector3.up) * wallRunForce;
            appliedForce *= wallRunPeriod;  // scale according to curve

            Vector3 finalVelocity = rbody.velocity;
            finalVelocity.y = Mathf.Max(appliedForce.y, rbody.velocity.y);  // retain superior velocity, if present

            rbody.velocity = finalVelocity;
        }

        buildMecanimState();

        if (curMoveState != MovementState.FLOATING)     // apply gravity if not floating
            rbody.AddForce(LocalGravity);

        // proc jump
        if (jumpIntention)
            Jump();

        prevTargetVelocity = targetVelocity;    // record the target velocity from the last frame

        // ## Audio
        // set speed modifier for footstep sounds
        footstepSpeed = sprintIntention ? sprintSpeed : baseSpeed;
        ProgressStepCycle(footstepSpeed);
    }

    void LateUpdate()
    {
        syncCameraToJoint();
        tiltCamera();
    }

    void OnGUI()
    {
        if (!drawDebugData)
            return;

        // Left side
        GUI.Label(new Rect(0, 0, 200, 20), "RBody Vel: " + rbody.velocity.ToString());
        GUI.Label(new Rect(0, 20, 200, 20), "Target Vel: " + moveIntention);

        GUI.Label(new Rect(0, 40, 200, 20), "Grounded: " + (curMoveState == MovementState.GROUNDED ? "true" : "false"));
        GUI.Label(new Rect(0, 60, 200, 20), "Crouching: " + (crouchIntention ? "true" : "false"));

        GUI.Label(new Rect(0, 100, 200, 20), "Ground Normal: " + groundNormal.ToString());
        GUI.Label(new Rect(0, 120, 200, 20), "Ground Angle: " + Vector3.Angle(groundNormal, Vector3.up));
        GUI.Label(new Rect(0, 140, 200, 20), "SqMag: " + sqrMag.ToString());
        GUI.Label(new Rect(0, 160, 200, 20), "SqTargetMag: " + sqrTarMag.ToString());
        GUI.Label(new Rect(0, 180, 200, 20), "Bump Force: " + bumpForce.ToString());
        GUI.Label(new Rect(0, 200, 200, 20), "Slope Eval: " + slopeCurveEval.ToString());
        GUI.Label(new Rect(0, 220, 200, 20), "Ground Angle: " + groundAngle.ToString());

        GUI.Label(new Rect(0, 240, 200, 20), "Jump Count: " + JumpCount.ToString());

        int screenWidth = Screen.width;

        // Right side
        GUI.Label(new Rect(screenWidth - 200, 0, 200, 20), parkourIntention.ToString());
        GUI.Label(new Rect(screenWidth - 200, 20, 200, 20), "Wall Run Perc:" + NormalizedWallRunTime.ToString());
        GUI.Label(new Rect(screenWidth - 300, 40, 300, 20), "Wall Run Relative Position:" + relativeWallPosition.ToString());
    }

    // TODO: Process this w/ TriggerEvents and ColliderEvents
    void OnCollisionEnter(Collision other)
    {
    }
    void OnCollisionStay(Collision other) { }
    void OnCollsionExit(Collision other)
    {

    }

    void OnTriggerEnter(Collider other)
    {
        // proc ladder
        if (other.CompareTag("Ladder"))
        {
            curMoveState = MovementState.FLOATING;
            Debug.Log("LADDER GRAB on Enter!");
        }
    }
    void OnTriggerStay(Collider other)
    {
    }
    void OnTriggerExit(Collider other)
    {
        // proc ladder
        if (other.CompareTag("Ladder"))
        {
            curMoveState = MovementState.FLYING;
        }
    }

    #endregion
}