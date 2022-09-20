using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour // Sten
{
    [Header("Floor Settings")]

    [SerializeField] private List<GameObject> sections = new List<GameObject>();
    [SerializeField] private Transform beginPosition;
    [SerializeField] private Transform mapParent;

    [Tooltip("Minimal amount of floors at all time")]
    [Min(0)] [SerializeField] private int minFloor;


    [Header("Obstacle Settings Settings")]

    [SerializeField] private List<GameObject> obstacles = new List<GameObject>();
    [SerializeField] private Transform obstacleParent;
    
    [Tooltip("The chance of an obstacle spawning")]
    [Range(1, 100)] [SerializeField] private float spawnChance;

    private Pool floorPool;
    private Pool objectPool;

    private List<GameObject> placedSections = new List<GameObject>();
    private List<GameObject> placedObstacles = new List<GameObject>();

    private Vector3 currentPosition; // Needed to know where the previous floor was placed

    // Will Destroy the script if it can't spawn any floors
    void Awake()
    {
        if (beginPosition == null || sections.Count == 0)
            Destroy(this);
    }

    void Start()
    {
        floorPool = mapParent.GetComponent<Pool>();
        objectPool = obstacleParent.GetComponent<Pool>();

        currentPosition = beginPosition.position;

        // Makes some premade floors
        for (int i = 0; i < minFloor; i++)
            CreateFloor(Random.Range(0, sections.Count), Random.Range(4, 25));

        StartCoroutine(CheckFloors());      // Needed to despawn floors
        StartCoroutine(CheckObstacles());   // Needed to despawn obstacles
    }

    // Creates a given obstacle (type) and spawns it onto a floor
    void CreateObstacle(int type, GameObject floor)
    {
        GameObject newObstacle = objectPool.GetObject(type, Vector3.zero, Quaternion.identity, null); // Gets the object from pool

        // Sets position & rotation
        newObstacle.transform.position = floor.transform.position + new Vector3(0, 4.5f - newObstacle.GetComponent<Obstacle>().spawnPoint.position.y);
        newObstacle.transform.rotation = floor.transform.rotation;

        placedObstacles.Add(newObstacle); // Adds into a stack for despawning
    }

    // Creates a given floor (type) with given rotation
    void CreateFloor(int type, float rotation)
    {
        GameObject newFloor = floorPool.GetObject(type, Vector3.zero, Quaternion.identity, null); // Gets floor from pool

        Floor floor = newFloor.GetComponent<Floor>();

        // calculates the position needed to connect and sets it with rotation
        newFloor.transform.rotation = Quaternion.Euler(0, 0, -rotation);
        newFloor.transform.position = currentPosition + (newFloor.transform.position - floor.End.position);

        currentPosition = floor.Begin.position;
        placedSections.Add(newFloor);

        // Spawn chance for spawning an obstacle
        if (Random.Range(0, 100) <= spawnChance - 1)
            CreateObstacle(Random.Range(0, obstacles.Count), newFloor);

        // Ups the spawnchance to make it harder over time
        if (spawnChance < 30)
            spawnChance += 0.08f;
    }

    // Coroutine for despawning obstacles
    IEnumerator CheckObstacles()
    {
        while (true)
        {
            yield return new WaitForSeconds(1); // waits every second to check

            for (int i = 0; i < placedObstacles.Count; i++)
            {
                GameObject obstacle = placedObstacles[i];

                if (Camera.main.transform.position.x - 30 > obstacle.transform.position.x) // checks if it's out of players view
                {
                    obstacle.GetComponent<PoolItem>().ReturnToPool(); // returns to pool
                    placedObstacles.Remove(obstacle);                 // removes from list
                }
            }
        }
    }

    IEnumerator CheckFloors()
    {
        while (true)
        {
            yield return new WaitForSeconds(1); // waits every second to check

            for (int i = 0; i < placedSections.Count; i++)
            {
                GameObject floor = placedSections[i];

                if (Camera.main.transform.position.x - 30 > floor.GetComponent<Floor>().End.position.x) // checks if it's out of players view
                {
                    floor.GetComponent<PoolItem>().ReturnToPool(); // returns to pool
                    placedSections.Remove(floor);                  // removes from list
                }
            }

            // Makes new floors if needed
            if (placedSections.Count < minFloor)
                for (int i = 0; i < minFloor - placedSections.Count; i++)
                    CreateFloor(Random.Range(0, sections.Count), Random.Range(4, 25)); // makes a floor with a random rotation from 4 till 25 (So it will always go down)
        }
    }
}
