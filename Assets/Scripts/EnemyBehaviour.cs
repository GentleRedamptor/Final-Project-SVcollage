using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] Transform playerPos;
    // ready to shoot
    
    
    [SerializeField] bool CanSeePlayer;
    [SerializeField] GameObject EnemyBody;
    [SerializeField] GameObject Target;
    [SerializeField] bool alreadyAttacked;
    [SerializeField] LayerMask whatIsPlayer;
    [SerializeField] float range;
    [SerializeField] int dmg;
    [SerializeField] Vector3 Direction;

    // Start is called before the first frame update
    private void Awake()
    {
        
       
        range = 100;
        dmg = 1;
        playerPos = GameObject.Find("PlayerHead").transform;
        CanSeePlayer = false;
       
    }

    // Update is called once per frame
    void Update()
    {
        Direction = playerPos.position - Target.transform.position;

        CanSeePlayer = Physics.CheckSphere(transform.position, range, whatIsPlayer);
        if (CanSeePlayer)
        {
            Target.transform.LookAt(playerPos);
            RaycastHit hit;
            if (Physics.Raycast(Target.transform.position, Direction, out hit, range))// range = 20
            {
                Debug.Log(hit.transform.name);
                if (hit.collider.name == "Player")
                {
                    StartCoroutine(DelayShot());
                    TimeToShoot();
                }
            }
        }
    }


    private void TimeToShoot()
    {
        transform.LookAt(playerPos);

        if (!alreadyAttacked)
        {
            RaycastHit hit;
            if (Physics.Raycast(Target.transform.position, Direction, out hit, range))// range = 20
            {
                Debug.Log(hit.transform.name);
                if (hit.collider.name == "Player")
                {
                    HealthHearts player = hit.transform.GetComponent<HealthHearts>();

                    player.takeDamage(dmg);
                }
            }
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), 1);
        }

        

    }
    void ResetAttack()
    {
        alreadyAttacked = false;

    }
    IEnumerator DelayShot()
    {

        yield return new WaitForSeconds(3);

    }
}
