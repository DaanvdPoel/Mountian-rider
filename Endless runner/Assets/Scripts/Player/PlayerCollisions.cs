using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour // Sten
{
    [SerializeField] private bool DieFromFloor;

    void OnCollisionEnter2D(Collision2D collision)
    {
        Obstacle obstacle = collision.transform.GetComponent<Obstacle>();
        Floor floor = collision.transform.GetComponent<Floor>();

        // Will continue death if it's an damagable obstacle or when it hits the floor
        if ((obstacle && obstacle.CausesDamage) || (DieFromFloor && floor))
            GameManager.instance.Death();
    }
}
