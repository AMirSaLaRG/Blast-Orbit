using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject platformPrefab;
    public int gridSize = 8;
    public int GridSize => gridSize;
    public float spacing = 11f;
    public float Spacing => spacing;
    public List<Vector3> platformCenters = new List<Vector3>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        SpawnPlatform();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void SpawnPlatform()
    {
        float offset = (gridSize - 1) * spacing * 0.5f;

        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                float posX = x * spacing - offset;
                float posZ = z * spacing - offset;
                // float posY = Random.Range(0.5f, 6f);
                Vector3 spawnPos = new Vector3(posX, 0, posZ);
                GameObject platform = Instantiate(platformPrefab, spawnPos, Quaternion.identity);
                platform.name = $"Platform_{x}_{z}";
                platform.transform.parent = transform;
                platformCenters.Add(spawnPos);
            }
        }
    }
    // Optional: Helper to get random center
    public Vector3 GetRandomCenter()
    {

        if (platformCenters.Count == 0) return Vector3.zero;
        return platformCenters[Random.Range(0, platformCenters.Count)];
    }
    public Vector3 GetNearestCenter(Vector3 fromPosition)
    {
        if (platformCenters.Count == 0) return Vector3.zero;

        Vector3 nearest = platformCenters[0];
        float minDist = Vector3.Distance(fromPosition, nearest);

        foreach (Vector3 center in platformCenters)
        {
            float dist = Vector3.Distance(fromPosition, center);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = center;
            }
        }

        return nearest;
    }
}
