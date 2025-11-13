using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public PlatformSpawner platformSpawner;
    public float heightMultiplier = 2f;
    public float lookAhead = 0f;

    private Transform player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player")?.transform;
        if (player == null) Debug.LogError("Tag Player as 'Player'!");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (player == null) return;

        float arenaSize = (platformSpawner.GridSize - 1) * platformSpawner.Spacing;
        float targetHeight = arenaSize * heightMultiplier;
        Vector3 targetPosition = new Vector3(0, targetHeight, 0);
        if (lookAhead > 0f)
        {
            Vector3 playerOffset = player.position;
            playerOffset.y = 0;  // Ignore player Y (we control height)
            targetPosition += playerOffset * lookAhead;
        }
        transform.position = targetPosition;
        transform.rotation = Quaternion.Euler(90, 0, 0);
    }
    
}
