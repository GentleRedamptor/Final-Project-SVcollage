using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArmMovement : MonoBehaviour
{
    Transform playerTransform;
    [SerializeField] EnemyLogic enemyscript;

    void Start()
    {
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();        
    }

    void Update()
    {
       if (enemyscript.isFollowingPlayer)AimArm(); 
    }

    void AimArm()
    {
        Vector3 playerPosition = playerTransform.position;
        transform.LookAt(playerPosition);
    }
}
