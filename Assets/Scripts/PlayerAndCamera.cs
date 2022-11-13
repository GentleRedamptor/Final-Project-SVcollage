using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAndCamera : MonoBehaviour
{
    float mouseX;
    float mouseY;
    [SerializeField] float sensitivity;
    Transform rotateCam;
    void Start()
    {
        sensitivity = 10;
        rotateCam = 
        
    }

    void Update()
    {
        CamControl();
        
    }

    void CamControl()
    {
        mouseX = Input.GetAxis("Mouse X") * sensitivity;         
        transform.localRotation = Quaternion.Euler(0f, mouseX, 0f);

        mouseY += Input.GetAxis("Mouse Y") * sensitivity;
        mouseY = Mathf.Clamp(mouseY, -89.9f, 89.9f);
        transform.localRotation = Quaternion.Euler(mouseY, 0f, 0f);
    }
}
