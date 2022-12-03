using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : MonoBehaviour
{  
    [SerializeField] float lookRange;
    [SerializeField] LayerMask playerLayerMask;
    bool isFollowingPlayer;
    Transform playerTransform;
    bool isAttacking;
    [SerializeField] Transform gunTip;
    float sphereCastredius;
    
    void Start()
    {
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        sphereCastredius = 0.1f;
    }

    void Update()
    {
        if(Physics.CheckSphere(transform.position, lookRange, playerLayerMask))
        {
            //Debug.Log("Player in range");
            CheckIfPlayerInLOS();
            if(isFollowingPlayer) //if following look at player no X rotation
            {
                Vector3 playerPosition = playerTransform.position;
                playerPosition.y = transform.position.y;
                transform.LookAt(playerPosition);
            }  
            if (isFollowingPlayer && !isAttacking)StartCoroutine(ShootingProcess()) ; //if following and not attacking start attack routine

        }
    }

    void CheckIfPlayerInLOS()
    {
        if(isFollowingPlayer) return;
        Vector3 direction = playerTransform.position - transform.position;
        RaycastHit hit;
        if (Physics.SphereCast(gunTip.position, sphereCastredius , direction, out hit, lookRange)) 
        {
            Debug.DrawRay(gunTip.position, direction, Color.red, 1.0f);
            if (hit.collider.tag == "Player") isFollowingPlayer = true;
        }

    }
    
    IEnumerator ShootingProcess()
    {
        isAttacking = true;
        Debug.Log("Started shooting...");
        yield return new WaitForSeconds(1f);
        Debug.Log("1 second pass");
        yield return new WaitForSeconds(1f);
        Debug.Log("2 second pass");
        yield return new WaitForSeconds(1f);
        Debug.Log("3 second pass");
        yield return new WaitForSeconds(1f);
        Debug.Log("4 second pass");
        yield return new WaitForSeconds(1f);
        Debug.Log("5 second pass");
        yield return new WaitForSeconds(1f);
        Debug.Log("Shoot!!!!");
        RaycastHit hit;
        Vector3 direction = playerTransform.position - transform.position;
        if (Physics.SphereCast(gunTip.position, sphereCastredius , direction, out hit, lookRange)) 
        {   
            Debug.DrawRay(gunTip.position, direction, Color.red, 1.0f);
            if (hit.collider.tag == "Player")
            {
                Debug.Log(hit.transform.name);
            }
        }
        yield return new WaitForSeconds(1f);
        isAttacking = false;
        isFollowingPlayer = false;
    }
}
