using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAndCamera : MonoBehaviour
{
    float mouseX;
    float mouseY;
    [SerializeField] float sensitivity;
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
    bool isGrounded;
    void Start()
    {
        LockMouse();
        originalSpeed = speed;
        //Getting Objects and Components
        rotateCam = GameObject.Find("RotateCamJoint").GetComponent<Transform>();
        controller = GetComponent<CharacterController>();
        groundCheck = GameObject.Find("GroundCheck").GetComponent<Transform>();        
    }

    void Update()
    {
        CheckIfGrounded();
        CamControl();
        if (Input.GetButtonDown("Jump") && isGrounded) Jump();
        if (Input.GetButton("Fire3")) StartSprint();
        if (Input.GetButtonUp("Fire3")) StopSprint();
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
        float horizontalinput = Input.GetAxis("Horizontal");
        float verticalinput = Input.GetAxis("Vertical");
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
        }
    }
    void Jump()
    {
        controller.slopeLimit = 100.0f;
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }
    void StartSprint()
    {
        speed = sprintSpeed;
    }
    void StopSprint()
    {
        speed = originalSpeed;
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

}
