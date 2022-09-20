using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour // Sten
{
    /*
     * Had to edit the whole pool for it to support multiple objects at once, making it more efficient & easier to use
     * I made it have a list instead of just one gameobject
     * makes a stack for each object & auto create all of them
     * Its a stack inside of a list with PoolItem as stack item
     * :-)
    */

    [Tooltip("Set this to true if you want to expand the pool if you run out of pooled objects.")]
    [SerializeField] private bool autoExpand = false;

    [Tooltip("The amount of new objects added when the pool runs out of objects.")]
    [SerializeField] private int expansionSize;

    [Tooltip("The prefab used for the Pool.")]
    [SerializeField] public List<GameObject> poolPrefabs = new List<GameObject>();

    public int poolSize = 10;

    private List<Stack<PoolItem>> stack = new List<Stack<PoolItem>>();

    private void Awake()
    {
        // Creates the stacks needed to store the objects
        for (int i = 0; i < poolPrefabs.Count; i++)
            stack.Add(new Stack<PoolItem>());

        // Creates the first poolSize objects
        for (int i = 0; i < poolPrefabs.Count; i++)
            Expand(poolSize, i);

        // Auto sets expansionSize to 1 if it's set to 0
        if (expansionSize == 0)
            expansionSize = 1;
    }

    // Expands the given object type with given amount of size
    private void Expand(int size, int type)
    {
        for (int i = 0; i < size; i++)
        {
            GameObject newObject = Instantiate(poolPrefabs[type], transform); // Creates an new object

            PoolItem item = newObject.GetComponent<PoolItem>();

            item.pool = this;
            item.gameObject.SetActive(false);
            item.ID = type; // ID is to check which stack it came from

            stack[type].Push(item); // Puts it into the stack
        }
    }

    // Gets the object with type given & other stuff
    public GameObject GetObject(int type, Vector3 position, Quaternion rotation, Transform parent)
    {
        if (stack[type].Count == 0)
        {
            if (autoExpand)
                Expand(expansionSize, type);
            else
            {
                print($"{name}, Pool is empty.");
                return null;
            }
        }

        PoolItem item = stack[type].Pop(); // Gets the item from stack

        item.Init(position, rotation, parent != null ? parent : transform);
        item.gameObject.SetActive(true);

        return item.gameObject;
    }

    // Returns the object which uses ID to indentify which stack it came from
    public void ReturnObject(PoolItem item)
    {
        if (item.gameObject.activeSelf == false) // check if not already inactive
            return;

        item.transform.parent = transform;
        item.gameObject.SetActive(false);

        stack[item.ID].Push(item); // Puts it back into the stack
    }
}
