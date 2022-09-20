using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour // Sten
{
    [SerializeField] private GameObject target;
    [SerializeField] private Vector3 offset;

    void Update()
    {
        // stops if no target selected
        if (target == null)
            return;

        // Sets the position with given offset
        this.transform.position = new Vector3(0, 0, -10) + target.transform.position + offset;       
    }
}
