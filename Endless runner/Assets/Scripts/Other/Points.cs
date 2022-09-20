using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Points : MonoBehaviour // Sten
{
    [SerializeField] private int amount;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>())
        {   // Checks if it's the player
            GameManager.instance.AddScore(amount);  // Gives given amount of points
            AudioManager.instance.PlaySoundEffect(4);
        }
    }
}
