using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] Transform playerPos;
    // ready to shoot
    [SerializeField] float timeTillShoot;
    [SerializeField] bool CanSeePlayer;
    [SerializeField] GameObject EnemyBody;
    [SerializeField] GameObject Target;
    [SerializeField] GameObject PlayerTarget;
    [SerializeField] GameObject saibaman;
    [SerializeField] bool alreadyAttacked;
    [SerializeField] LayerMask whatIsPlayer;
    [SerializeField] float range;
    [SerializeField] int dmg;

    // Start is called before the first frame update
    private void Awake()
    {
        timeTillShoot = 5;
        range = 100;
        dmg = 1;
        playerPos = GameObject.Find("Player").transform;
        CanSeePlayer = false;
       
    }

    // Update is called once per frame
    void Update()
    {
        CanSeePlayer = Physics.CheckSphere(transform.position, range, whatIsPlayer);
        if (CanSeePlayer)
        { saibaman.transform.parent = PlayerTarget.transform;

            TimeToShoot();
        }
    }


    private void TimeToShoot()
    {
      transform.LookAt(playerPos);
      StartCoroutine(Coloroutine(new Color[3] { Color.green, Color.yellow, Color.red }, 1, EnemyBody));
        // int timeTillShoot = 5;
        if (!alreadyAttacked )
        {
            RaycastHit hit;
            if (Physics.Raycast(Target.transform.position, transform.forward, out hit, range))// range = 20
            {
                
                Debug.Log(hit.transform.name);
                if (hit.collider.name == "Player")
               {
                    PlayerAndCamera player = hit.transform.GetComponent<PlayerAndCamera>();

                    player.takeDamage(dmg);
               }
            }
            StopCoroutine(Coloroutine(new Color[3] { Color.green, Color.yellow, Color.red }, 1, EnemyBody));
            alreadyAttacked = true;   
            Invoke(nameof(ResetAttack), 3);
           
        }
    }
    void ResetAttack()
    {
        alreadyAttacked = false;

    }

    IEnumerator Coloroutine(Color[] colors, float Time, GameObject GO)
    {
        //foreach (Color color in colors)
        for (int i = 0; i < colors.Length; i++)
        {
            GO.GetComponent<Renderer>().material.color = colors[i];
            yield return new WaitForSeconds(2);
        }
    }
}
