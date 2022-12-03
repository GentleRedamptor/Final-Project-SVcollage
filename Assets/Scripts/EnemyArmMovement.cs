using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArmMovement : MonoBehaviour
{
    // Start is called before the first frame update
    Transform playerTransform;

    void Start()
    {
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();        
    }

    // Update is called once per frame
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
