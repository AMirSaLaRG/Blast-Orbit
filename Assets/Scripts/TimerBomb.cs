using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TimerBomb : MonoBehaviour
{
    public TextMeshProUGUI countdownText;
    public float bombTimer;
    public float explosionDuration = 1.0f;
    public int damage = 1;
    private bool isExploding;
    private bool playerDamaged = false;
    private const float TargetY = 3f;
    public ParticleSystem explosionEffect;
    private PlayerController playerController;
    private AudioSource audioSource;
    private float auidoSourceLength ;
    public float timeToPlayClip = 2.0f;
    public float endOfClipTime = 2f;
    private bool isplayed = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
        countdownText.text = $"{bombTimer}s";
        StartCoroutine(DropDown(this.gameObject, bombTimer)); 
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        
        
    }

    // Update is called once per frame
    void Update()
    {
     
    }
    // void LateUpdate()
    // {
    //     if (countdownText != null && Camera.main != null)
    //     {
    //         // Calculate the direction from the text to the camera
    //         Vector3 targetDir = countdownText.transform.position - Camera.main.transform.position;
            
    //         // Apply the rotation needed to look at that direction
    //         countdownText.transform.rotation = Quaternion.LookRotation(targetDir);
    //     }
    // }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TimerBomb"))
        {
            Destroy(gameObject);
        }
        // Debug.Log($"🔔 Trigger entered by: {other.gameObject.name}");
        CheckAndApplyDamage(other);
    }
    void OnTriggerStay(Collider other)
    {
        
        CheckAndApplyDamage(other);
    }
    void OnTriggerExit(Collider other)
    {
        // Debug.Log($"🚪 Trigger exited by: {other.gameObject.name}");
    }
    private IEnumerator DropDown(GameObject bomb, float explodeTime)
    {
        // Define the start and end positions
        Vector3 startPos = bomb.transform.position;
        Vector3 endPos = new Vector3(startPos.x, TargetY, startPos.z);
        float dropTime = explodeTime * 1/4f;
        float countdownTime = bombTimer;

        float elapsedTime = 0f;
        StartCoroutine(Explode(bomb));

        // The while loop runs every frame until the drop time is complete
        
        while (elapsedTime < dropTime)
        {
            // Calculate the 0-to-1 factor for Lerp
            float t = elapsedTime / dropTime;
            
            // Calculate and apply the new position using Lerp
            bomb.transform.position = Vector3.Lerp(startPos, endPos, t);

            // Advance the timer by the time elapsed since the last frame
            elapsedTime += Time.deltaTime;
            

            countdownText.text = $"{Mathf.CeilToInt(countdownTime)}s"; 
            countdownTime -= Time.deltaTime;

            // Wait until the next frame before running the loop again
            yield return null;
        }

        // 1. Ensure the bomb ends exactly at the target position (y=2)
        bomb.transform.position = endPos;
        
        // 2. Start the explosion sequence after landing
        
        
        while (countdownTime > 0)
        {
            // Update the display text every frame for a smooth countdown
            if (countdownText != null)
            {
                // Format F1 shows one decimal place (e.g., 3.0s, 2.9s)
                countdownText.text = $"{Mathf.CeilToInt(countdownTime)}s"; 
                PlayBombSound(countdownTime);
            }

            // Decrease time every frame
            countdownTime -= Time.deltaTime;
            yield return null; // Wait until the next frame
        }
        
    }

    // Coroutine to handle the bomb's explosion countdown
    private IEnumerator Explode(GameObject bomb)
    {
        // Debug.Log($"💣 Bomb at {bomb.transform.position} landed. Starting {bombTimer}s countdown.");
        
        // Wait for the duration of the explosion
        // This is where you might play an animation or visual indicator
        yield return new WaitForSeconds(bombTimer);
        
        // 🚨 TODO: You would typically add code here to check for player proximity and apply damage.
        
        // Debug.Log("💥 BOOM! Bomb destroyed.");
        // Destroy the bomb GameObject
        StartCoroutine(ExplotionDuration(bomb));
    }
    private IEnumerator ExplotionDuration(GameObject bomb)
    {
        isExploding = true;
        explosionEffect.Play();
        countdownText.text = "BOOM";
        
        yield return new WaitForSeconds(explosionDuration);
        isExploding = false;
        Destroy(bomb);
    }
    void CheckAndApplyDamage(Collider other)
    {
        // CRITICAL CHECKS: We only apply damage if...
        // 1. The bomb is currently in the active explosion state.
        // 2. The player has not already been damaged by this specific bomb.
        if (isExploding && !playerDamaged)
        {
            // Assuming the Player GameObject has the tag "Player"
            if (other.CompareTag("Player"))
            {
                // DAMAGE APPLIED HERE
                playerDamaged = true; // Set flag to prevent double damage
                // Debug.Log($"💔 Player entered explosion radius! Apply damage to: {other.gameObject.name}");
                playerController.TakeDamage(damage);
                
                // 🚨 TODO: In a real game, you would call a function on the player component here:
                // other.GetComponent<PlayerHealth>().TakeDamage(25);
            }
        }
    }
    public void PlayBombSound(float countdownTime)
    {
        if (isplayed) return;
        float playTime = bombTimer - timeToPlayClip;
        
        if (countdownTime <= playTime)
        {
            Debug.Log("playtime: "+ playTime + " time: " + countdownTime);
            isplayed = true;
            audioSource.Play();
        }
        
    }
}
