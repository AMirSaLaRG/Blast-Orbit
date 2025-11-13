using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlatformSpawner platformSpawner;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        platformSpawner = GameObject.Find("PlatformSpawner").GetComponent<PlatformSpawner>();
        Respawn();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void Respawn()
    {
        transform.position = platformSpawner.GetRandomCenter() + Vector3.up * 2f;
    }
   
}
