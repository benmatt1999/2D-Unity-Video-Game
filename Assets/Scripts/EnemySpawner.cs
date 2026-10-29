using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform player;
    [SerializeField] private LayerMask groundLayer;
    private float minSpawn = 0.1f;
    private float maxSpawn = 0.9f;
    private float hitRadius = 0.3f;
    private float minDistToPlayer = 1.8f;

    private float spawnInterval = 5f;
    private float spawnTimer;

    private int numOfBats = 0;

    private void Start()
    {
        Spawn();
        spawnTimer = spawnInterval;
    }

    private void Update()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
        {
            Spawn();
            spawnTimer = spawnInterval;
            spawnInterval *= 0.95f;
        }
    }

    public void Spawn()
    {
        Vector3 spawnPos = getSpawnPos();

        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        numOfBats++;
    }

    private Vector3 getSpawnPos()
    {
        bool validSpawn = false;
        Vector3 spawnPos = new Vector3(0f, 0f, 0f);
        
        while (!validSpawn)
        {
            float xPos = Random.Range(minSpawn, maxSpawn);
            float yPos = Random.Range(minSpawn, maxSpawn);


            Vector3 worldPos = new Vector3(xPos, yPos, 10f);
            spawnPos = Camera.main.ViewportToWorldPoint(worldPos);

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