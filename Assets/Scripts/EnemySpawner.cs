using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class SpawnEntry
    {
        public GameObject enemyPrefab;
        public float interval = 2f;
        public float minInterval = 0.5f;
        public float rampFactor = 0.95f;
        public bool spawnsOnGround;

        [System.NonSerialized] public float timer;
    }

    [SerializeField] private SpawnEntry[] spawnEntries;

    [SerializeField] private Transform player;
    [SerializeField] private LayerMask groundLayer;
    private float minSpawn = 0.1f;
    private float maxSpawn = 0.9f;
    private float hitRadius = 0.3f;
    private float minDistToPlayer = 2.8f;
    private float groundOffset = 1f;

    private void Start()
    {
        foreach (SpawnEntry entry in spawnEntries)
        {
            entry.timer = entry.interval;
        }
    }

    private void Update()
    {
        foreach (SpawnEntry entry in spawnEntries)
        {
            entry.timer -= Time.deltaTime;
            if (entry.timer > 0) continue;

            Vector3 validSpawn = getSpawnPos(entry);
            Spawn(entry, validSpawn);
            entry.interval = Mathf.Max(entry.minInterval, entry.interval * entry.rampFactor);
            entry.timer = entry.interval;
        }
    }

    public void Spawn(SpawnEntry enemyPrefab, Vector3 spawnPos)
    {
        Instantiate(enemyPrefab.enemyPrefab, spawnPos, Quaternion.identity);
    }

    private Vector3 getSpawnPos(SpawnEntry entry)
    {
        bool validSpawn = false;
        Vector3 spawnPos = new Vector3(0f, 0f, 0f);
        
        while (!validSpawn)
        {
            float xPos = Random.Range(minSpawn, maxSpawn);
            float yPos = Random.Range(minSpawn, maxSpawn);


            Vector3 worldPos = new Vector3(xPos, yPos, 10f);
            spawnPos = Camera.main.ViewportToWorldPoint(worldPos);

            if (entry.spawnsOnGround)
            {
                RaycastHit2D floor = Physics2D.Raycast(spawnPos, Vector2.down, 20f, groundLayer);
                if (!floor) continue;
                spawnPos = floor.point + Vector2.up * groundOffset;
            }

            bool groundSpawn = Physics2D.OverlapCircle(spawnPos, hitRadius, groundLayer) != null;
            bool tooCloseToPlayer = Vector2.Distance(player.position, spawnPos) < minDistToPlayer;

            if (!groundSpawn && !tooCloseToPlayer)
            {
                validSpawn = true;
            }
        }
      
        return spawnPos;
    }
}