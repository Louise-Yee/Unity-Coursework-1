using UnityEngine;
using System.Collections.Generic;

public class RandomEnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Your enemy prefab
    public int numberOfEnemies; // Number of enemies to spawn
    public float spawnAreaWidth, spawnAreaHeight; // Area where enemies can spawn
    public float minimumDistance; // Minimum distance between enemies

    private List<Vector3> spawnPositions = new List<Vector3>();

    private void Start()
    {
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            Vector3 spawnPosition;
            bool validPosition = false;
            int attempts = 0;

            // Try to find a valid spawn position
            while (!validPosition && attempts < 100) // Limit attempts to avoid infinite loops
            {
                spawnPosition = new Vector3(
                    Random.Range(-spawnAreaWidth / 2, spawnAreaWidth / 2),
                    0, // Assuming y=0 for ground level
                    Random.Range(-spawnAreaHeight / 2, spawnAreaHeight / 2)
                );

                // Check if the position is valid
                if (IsPositionValid(spawnPosition))
                {
                    spawnPositions.Add(spawnPosition);
                    Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                    validPosition = true;
                }
                attempts++;
            }
        }
    }

    private bool IsPositionValid(Vector3 position)
    {
        // Check distance from other spawn positions
        foreach (Vector3 existingPosition in spawnPositions)
        {
            if (Vector3.Distance(position, existingPosition) < minimumDistance)
            {
                return false; // Too close to another enemy
            }
        }
        return true; // Valid position
    }
}
