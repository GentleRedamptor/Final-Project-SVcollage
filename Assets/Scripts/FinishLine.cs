using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    GameManager gameManager;
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    private void OnTriggerEnter(Collider other) 
    {
        if(other.tag == "Player")
        {
            StartCoroutine(gameManager.WinLevel());
        }
        
    }

}
