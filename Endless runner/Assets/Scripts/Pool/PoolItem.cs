using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolItem : MonoBehaviour // Sten
{
    private Pool myPool;
    public Pool pool { set { myPool = value; } }
    public int ID; // ID needed to indentify the correct stack

    // Sets up the item and activates it
    public void Init(Vector3 position, Quaternion rotation, Transform parent)
    {
        transform.position = position;
        transform.rotation = rotation;
        transform.parent = parent;

        Activate();
    }

    // activate & deactivate blueprint
    protected virtual void Activate()
    {
        //
    }
    protected virtual void Deactivate()
    {
        //
    }

    // Deactivates the item & returns it back into the stack
    public void ReturnToPool()
    {
        Deactivate();

        myPool.ReturnObject(this);
    }
}
