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
    [SerializeField] GameObject G;
    
    // Start is called before the first frame update
   private void Awake()
    {
        CanSeePlayer = false;
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, playerPos.position) <= 10)
        { 
            CanSeePlayer = true;
        TimeToShoot(); }
        else
        {
            CanSeePlayer = false;

        }


    }


    private void TimeToShoot()
    {
        if (CanSeePlayer)
        {
            transform.LookAt(playerPos);
            StartCoroutine(Coloroutine(new Color[3] { Color.green, Color.yellow, Color.red }, 1, G));
            Destroy(GameObject.Find("Player"), 5);

        }
       

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
