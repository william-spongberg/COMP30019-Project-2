using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{


    [SerializeField]
    private Image[] hearts; // Drag and drop heart images from the inspector
    [SerializeField]
    private int maxHealth;

    public void SetMaxHealth(int health)
    {
        maxHealth = health;
    }

    public void SetHealth(int health)
    {
        // Calculate how many hearts should be shown based on the player's current health
        int fullHeartsToShow = (health * hearts.Length) / maxHealth;

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < fullHeartsToShow)
            {
                hearts[i].enabled = true; // Show heart
            }
            else
            {
                hearts[i].enabled = false; // Hide heart
            }
        }
    }
    

}
