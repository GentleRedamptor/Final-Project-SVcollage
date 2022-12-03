using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArmMovement : MonoBehaviour
{
    Transform playerTransform;

    void Start()
    {
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();        
    }

    void Update()
    {
       AimArm(); 
    }

    void AimArm()
    {
        Vector3 playerPosition = playerTransform.position;
        transform.LookAt(playerPosition);
    }
}
