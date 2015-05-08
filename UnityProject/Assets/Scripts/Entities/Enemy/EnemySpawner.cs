using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour {

    public GameObject Enemy;

    private List<GameObject> spawns;
    public int spawnCount;

    public int spawnCap;

    private float spawnTimer;
    public float spawnTimerMax;

	// Use this for initialization
	void Start () {
        spawns = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Enemy != null)
        {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0.0f && spawns.Count < spawnCap)
            {
                Spawn();
                spawnTimer = spawnTimerMax;
            }
            CheckSpawns();
        }
	}

    void Spawn()
    {
        spawns.Add((GameObject)Instantiate(Enemy, transform.position, transform.rotation));
    }

    void CheckSpawns()
    {
        spawnCount = spawns.Count;

        for(int currSpawn = spawns.Count - 1; currSpawn >= 0; --currSpawn)
        {
            if (spawns[currSpawn] == null || spawns[currSpawn].GetComponent<Entity>().dead)
                spawns.RemoveAt(currSpawn);
        }
    }
}
