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
    [SerializeField] float timeUntilShooting;
    SpriteRenderer exclamationMark;
    [SerializeField] float bigTInterval; //D 0.25
    [SerializeField] float smallTInterval; //D 0.1
    [SerializeField] float lastTInterval; //D 1
    
    void Start()
    {
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        exclamationMark = GameObject.Find("ExclamationMark").GetComponent<SpriteRenderer>();
        sphereCastredius = 0.1f;
        exclamationMark.enabled = false;
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
            if (hit.collider.tag == "Player")
            {
                isFollowingPlayer = true;
                exclamationMark.enabled = true;
            } 
        }

    }
    
    IEnumerator ShootingProcess()
    {
        isAttacking = true;

        //yield return new WaitForSeconds(timeUntilShooting);
        for (int i = 0; i < 5; i++) 
        {
            yield return new WaitForSeconds(bigTInterval);
            exclamationMark.enabled = false;
            yield return new WaitForSeconds(bigTInterval);
            exclamationMark.enabled = true;   
            //beep
        }
        for (int i = 0; i < 7; i++) 
        {
            yield return new WaitForSeconds(smallTInterval);
            exclamationMark.enabled = false;
            yield return new WaitForSeconds(smallTInterval);
            exclamationMark.enabled = true;
            //beep   
        }
        yield return new WaitForSeconds(smallTInterval);
        exclamationMark.enabled = false;
        yield return new WaitForSeconds(smallTInterval);
        exclamationMark.enabled = true;
        //long beep
        yield return new WaitForSeconds(lastTInterval);
        RaycastHit hit;
        Vector3 direction = playerTransform.position - transform.position;
        if (Physics.SphereCast(gunTip.position, sphereCastredius , direction, out hit, lookRange)) 
        {   
            Debug.DrawRay(gunTip.position, direction, Color.red, 1.0f);
            if (hit.collider.tag == "Player")
            {
                //play gun explosion SFX
                Debug.Log(hit.transform.name);
                PlayerAndCamera player = hit.transform.GetComponent<PlayerAndCamera>();
                player.TakeDamage();
            }
        }
        isAttacking = false;
        isFollowingPlayer = false;
        exclamationMark.enabled = false;
    }
}
