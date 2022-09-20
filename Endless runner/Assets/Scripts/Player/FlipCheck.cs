using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipCheck : MonoBehaviour //Daan
{
   [SerializeField] private float raycastRange = 1;

   [SerializeField] private LayerMask ground;

    //timer
    [SerializeField] private float canGetPointsTimer = 1.5f;
    private bool canGetPoints;
    private float time;

    private void Update()
    {
        Check();

        //timer
        if(canGetPoints == false)
        {
            time = time + Time.deltaTime;
        }
        if(time >= canGetPointsTimer)
        {
            canGetPoints = true;
            time = 0;
        }
    }

    // if de player is flipt and the ray cast hits the ground it will give the player some points. the player wont get more point until te cangetPoint bool is true
    private void Check()
    {
        if (Physics2D.Raycast(transform.position + new Vector3(0,1,0) , Vector2.up * raycastRange, ground) && canGetPoints == true)
        {
            canGetPoints = false;
            GameManager.instance.AddScore(50);
            AudioManager.instance.PlaySoundEffect(4);
        }

        Debug.DrawRay(transform.position, Vector2.up * raycastRange, Color.magenta);
    }
}
