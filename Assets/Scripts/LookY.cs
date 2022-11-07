using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookY : MonoBehaviour
{
    [SerializeField]
    private float sensitivity = 1.0f;
    private float mouseY;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        mouseY += Input.GetAxis("Mouse Y") * sensitivity;
        mouseY = Mathf.Clamp(mouseY, -89.9f, 89.9f);
        transform.localRotation = Quaternion.Euler(mouseY, 0f, 0f);
    }
}
