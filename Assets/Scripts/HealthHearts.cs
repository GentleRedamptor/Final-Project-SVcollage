using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthHearts : MonoBehaviour
{
    [SerializeField] int Health = 3;
    [SerializeField] Image[] hearts;
    [SerializeField] Sprite Heart;

    public void Update()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if(i<Health)
            {
                hearts[i].sprite = Heart;
            }
            if (i < Health)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;

            }
        }
    }
    public void takeDamage(int amount)
    {
        Health -= amount;
        if (Health <= 0) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
