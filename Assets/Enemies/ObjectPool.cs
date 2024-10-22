using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    public GameObject projectilePrefab; // Prefab of the projectile
    public int poolSize = 10; // Size of the pool

    private List<GameObject> projectilePool;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Initialize the pool
        projectilePool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(projectilePrefab);
            obj.SetActive(false); // Deactivate the projectile
            projectilePool.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        // Find an inactive projectile in the pool
        foreach (GameObject obj in projectilePool)
        {
            if (!obj.activeInHierarchy)
            {
                return obj;
            }
        }

        // Optionally, you can expand the pool if needed
        GameObject newObj = Instantiate(projectilePrefab);
        projectilePool.Add(newObj);
        return newObj;
    }
}
