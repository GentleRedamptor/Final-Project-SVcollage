using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerAndCamera : MonoBehaviour
{
    public int health = 3;
    float mouseX;
    float mouseY;
    [SerializeField] float sensitivity;
    float originalSensitivity;
    float aimAssistSensitivity;
    Transform rotateCam;
    [SerializeField] float speed;
    [SerializeField] float sprintSpeed;
    float originalSpeed;
    [SerializeField] float gravity;
    [SerializeField] float jumpHeight = 3f;
    CharacterController controller;
    Vector3 velocity;
    Transform groundCheck;
    [SerializeField] float groundCheckRadius = 0.4f;
    [SerializeField] LayerMask groundMask;
    [SerializeField] LayerMask fogMask;
    bool isGrounded;
    bool hasFalled;
    float horizontalinput;
    float verticalinput;

    //grapple things
    Transform cam;
    public Transform gunTip;
    public LineRenderer grappleLine;
    [SerializeField] LayerMask grappleLayer;
    [SerializeField] float maxGrappleDistance;
    [SerializeField] float grappleDelayTime;
    public Vector3 grapplePoint;
    [SerializeField] float grappleCD;
    float grappleCDTimer;
    public bool isGrappling = false;
    bool activeGrapple;
    [SerializeField] float overshootYAxis;
    [SerializeField] float grappleOffset = 5;
    bool canCancelGrapple;
    [SerializeField] float grappleFOV;
    [SerializeField] Image crosshair;
    

    //Attack things
    Animator playerAnimator;
    BoxCollider attackCollider;

    void Start()
    {
        LockMouse();
        originalSpeed = speed;
        originalSensitivity = sensitivity;
        aimAssistSensitivity = sensitivity * 0.5f;
        //Getting Objects and Components
        rotateCam = GameObject.Find("RotateCamJoint").GetComponent<Transform>();
        controller = GetComponent<CharacterController>();
        groundCheck = GameObject.Find("GroundCheck").GetComponent<Transform>();   
        cam = GameObject.Find("Main Camera").GetComponent<Transform>();  
        gunTip = GameObject.Find("GunTip").GetComponent<Transform>();
        grappleLine = GameObject.Find("GrappleGun").GetComponent<LineRenderer>();
        playerAnimator = GameObject.Find("Armature").GetComponent<Animator>();
        attackCollider = GetComponent<BoxCollider>();
    }

    void Update()
    {
        CheckIfGrounded();
        FallDeath();
        CamControl();
        if (Input.GetButtonDown("Jump") && isGrounded) Jump();
        if (Input.GetButtonDown("Jump") && !isGrounded && canCancelGrapple) CancelGrapple();
        if (Input.GetButton("Fire3")) StartSprint();
        if (Input.GetButtonUp("Fire3")) StopSprint();
        CheckIfCanGrapple();
        if (Input.GetButtonDown("Fire1")) StartGrapple();
        if (grappleCDTimer > 0) grappleCDTimer -= Time.deltaTime;
        if (Input.GetButtonDown("Fire2")) Attack(); 
        PlayerMovement();
        if (Input.GetKeyDown(KeyCode.Escape)) ReleaseMouse();
                
    }


    void CamControl()
    {
        mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;         
        transform.Rotate(Vector3.up * mouseX);

        mouseY += Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
        mouseY = Mathf.Clamp(mouseY, -89.9f, 89.9f);
        rotateCam.localRotation = Quaternion.Euler(-mouseY, 0f, 0f);
    }
    void PlayerMovement()
    {
        if (isGrounded)
        {
            horizontalinput = Input.GetAxis("Horizontal");
            verticalinput = Input.GetAxis("Vertical");
        }
        else if(activeGrapple) 
        {
            horizontalinput = 0; 
            verticalinput = 0;
        }
        Vector3 move = transform.right * horizontalinput + transform.forward * verticalinput;
        velocity.y += gravity * Time.deltaTime;
        controller.Move(move * speed * Time.deltaTime); //move the character on the X and Z axis
        if ((controller.collisionFlags & CollisionFlags.Above) != 0) velocity.y = -2f; //If there is a ceiling stop jump force 
        controller.Move(velocity * Time.deltaTime); //apply gravity
    }
    void CheckIfGrounded()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask);
        if (isGrounded && velocity.y < 0) 
        {
            controller.slopeLimit = 45.0f;
            velocity.y = -2f; //Reseting fall force if grounded
            velocity.x = 0;
            velocity.z = 0;
            canCancelGrapple = false;
        }
    }
    public void takeDamage(int amount)
    {
        health -= amount;
        if (health <= 0) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
        void FallDeath()
    {
        hasFalled = Physics.CheckSphere(groundCheck.position, groundCheckRadius, fogMask);
        if (hasFalled) 
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    void Jump()
    {
        controller.slopeLimit = 100.0f;
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }
    void CancelGrapple()
    {
        canCancelGrapple = false;
        horizontalinput = Input.GetAxis("Horizontal") / 2;
        verticalinput = Input.GetAxis("Vertical") / 2;
        controller.slopeLimit = 100.0f;
        velocity.y = Mathf.Sqrt((jumpHeight / 2) * -2f * gravity);
        StopGrapple();
        velocity.x = 0;
        velocity.z = 0;
    }
    void StartSprint()
    {
        speed = sprintSpeed;
    }
    void StopSprint()
    {
        speed = originalSpeed;
    }
    void CheckIfCanGrapple()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.position , cam.forward , out hit , maxGrappleDistance , grappleLayer))
        {
            crosshair.color = new Color32(36 , 255 , 0, 255);
            sensitivity = aimAssistSensitivity;
        }
        else
        {
            crosshair.color = new Color32(255 , 0 , 30 , 255);
            sensitivity = originalSensitivity;
        }
    }
    void StartGrapple()
    {
        if (grappleCDTimer > 0) return;
        isGrappling = true;
        RaycastHit hit;
        if (Physics.Raycast(cam.position , cam.forward , out hit , maxGrappleDistance , grappleLayer))
        {
            grapplePoint = hit.point;
            Invoke(nameof(ExecuteGrapple), grappleDelayTime);
        }
        else
        {
            grapplePoint = cam.position + cam.forward * maxGrappleDistance;
            Invoke(nameof(StopGrapple), grappleDelayTime);
        }
        grappleLine.enabled = true;
        //grappleLine.SetPosition(1, grapplePoint); Don't need cause of script animation

    }
    public Vector3 CalculateJumpVelocity(Vector3 startpoint, Vector3 endpoint, float trajectoryHeight)
    {
        float mGravity = gravity - grappleOffset;
        float displacementY = endpoint.y - startpoint.y;
        Vector3 displacementXZ = new Vector3(endpoint.x - startpoint.x, 0f , endpoint.z - startpoint.z);
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * mGravity * trajectoryHeight);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * trajectoryHeight / mGravity)
            + Mathf.Sqrt(2 * (displacementY - trajectoryHeight) / mGravity)); 
        return velocityXZ + velocityY;
    }
    public void JumpToPosition(Vector3 targetPosition , float trajectoryHeight)
    {
        activeGrapple = true;
        canCancelGrapple = true;
        velocityToSet = CalculateJumpVelocity(transform.position , targetPosition , trajectoryHeight);
        Invoke(nameof(SetVelocity), 0.1f);
    }
    Vector3 velocityToSet;
    void SetVelocity()
    {
        velocity = velocityToSet;
    }
    void ExecuteGrapple()
    {
        Vector3 lowestPoint = new Vector3(transform.position.x , transform.position.y - 1f , transform.position.z);
        float grapplePointRelativeYPos = grapplePoint.y - lowestPoint.y;
        float highestPointOnArc = grapplePointRelativeYPos + overshootYAxis;
        if (grapplePointRelativeYPos < 0) highestPointOnArc = overshootYAxis;
        JumpToPosition(grapplePoint, highestPointOnArc);
        DoFov(grappleFOV);
        Invoke(nameof(StopGrapple), 1f);
    }
    void StopGrapple()
    {
        isGrappling = false;
        activeGrapple = false;
        grappleCDTimer = grappleCD;
        grappleLine.enabled = false;
        DoFov(85.0f);
    }
    void LockMouse()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    void ReleaseMouse()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void Attack()
    {
        playerAnimator.SetTrigger("Attack");
        attackCollider.enabled = true;
        Invoke(nameof(TurnOffAttackCollider), 0.5f);
    }
    void TurnOffAttackCollider()
    {
        attackCollider.enabled = false;
    }
    private void OnTriggerEnter(Collider other) 
    {
        if(other.tag == "Enemy")
        {
            Debug.Log("Killed an enemy");
            Destroy(other.gameObject);
        }
        
    }
    void DoFov(float endValue)
    {
       cam.gameObject.GetComponent<Camera>().DOFieldOfView(endValue , 0.25f);

    }

}
