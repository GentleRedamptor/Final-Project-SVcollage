using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookX : MonoBehaviour
{
    [SerializeField]
    private float sensitivity = 1.0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");         
        Vector3 newrotation = transform.localEulerAngles;
        newrotation.y += mouseX * sensitivity;
        transform.localEulerAngles = newrotation;
    }
}
