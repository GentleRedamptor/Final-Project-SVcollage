using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : MonoBehaviour
{  
    [SerializeField] float lookRange;
    [SerializeField] LayerMask playerLayerMask;
    public bool isFollowingPlayer;
    Transform playerTransform;
    bool isAttacking;
    [SerializeField] Transform gunTip;
    float sphereCastredius;
    [SerializeField] SpriteRenderer exclamationMark;
    [SerializeField] float bigTInterval; //D 0.25
    [SerializeField] float smallTInterval; //D 0.1
    [SerializeField] float lastTInterval; //D 1
    // sound effects
    [SerializeField] AudioSource shortBeepSFX;
    [SerializeField] AudioSource longBeepSFX;
    [SerializeField] AudioSource fireSFX;
    [SerializeField] AudioSource alertSFX;
    [SerializeField] Transform enemyHead;
    Vector3 playerHightOffset;
    
    // laser sight
    [SerializeField] LineRenderer laserSight;  
    [SerializeField] Transform laserSightPos; 



    void Start()
    {
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        sphereCastredius = 0.1f;
        exclamationMark.enabled = false;
        playerHightOffset = new Vector3(0,2.5f,0);
        laserSight.SetPosition(0, laserSightPos.position);
        laserSight.enabled = false;
    }

    void Update()
    {
        laserSight.SetPosition(0, laserSightPos.position);
        if(Physics.CheckSphere(transform.position, lookRange, playerLayerMask))
        {
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
        Vector3 direction = (playerTransform.position + playerHightOffset) - enemyHead.position;
        RaycastHit hit;
        if (Physics.SphereCast(enemyHead.position, sphereCastredius , direction, out hit, lookRange)) 
        {
            Debug.DrawRay(enemyHead.position , direction, Color.red, 1.0f);
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
        alertSFX.Play();
        for (int i = 0; i < 5; i++) 
        {
            yield return new WaitForSeconds(bigTInterval);
            exclamationMark.enabled = false;
            yield return new WaitForSeconds(bigTInterval);
            exclamationMark.enabled = true;
            shortBeepSFX.Play();

        }
        for (int i = 0; i < 7; i++) 
        {
            yield return new WaitForSeconds(smallTInterval);
            exclamationMark.enabled = false;
            yield return new WaitForSeconds(smallTInterval);
            exclamationMark.enabled = true;
            shortBeepSFX.Play();
        }
        yield return new WaitForSeconds(smallTInterval);
        exclamationMark.enabled = false;
        yield return new WaitForSeconds(smallTInterval);
        exclamationMark.enabled = true;
        longBeepSFX.Play();
        yield return new WaitForSeconds(lastTInterval);
        RaycastHit hit;
        Vector3 direction = playerTransform.position - transform.position;
        if (Physics.SphereCast(gunTip.position, sphereCastredius , direction, out hit, lookRange)) 
        {   
            fireSFX.Play();
            Debug.DrawRay(gunTip.position, direction, Color.red, 1.0f);
            Vector3 direction2 = playerTransform.position - laserSightPos.position;
            laserSight.SetPosition(0, laserSightPos.position);
            RaycastHit laserhit;
            if (Physics.Raycast(laserSightPos.position, direction2, out laserhit, lookRange)) 
            {
                laserSight.enabled = true;
                laserSight.SetPosition(1, hit.point - new Vector3 (0,1,0));
                Invoke(nameof(DisableShotTrace), 3);
            }
            if (hit.collider.tag == "Player")
            {
                PlayerAndCamera player = hit.transform.GetComponent<PlayerAndCamera>();
                player.TakeDamage();
            }
        }
        isAttacking = false;
        isFollowingPlayer = false;
        exclamationMark.enabled = false;       
    }

    void DisableShotTrace()
    {   
        laserSight.enabled = false;
        laserSight.SetPosition(1, laserSightPos.position);
    }
}
