using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Jump Settings")]
    public PlatformSpawner platformSpawner;
    public float multiplayerJumpDurationWithOutGas = 2.1f;
    public AudioClip jumpSound;
    public float multiplayerJumpDurationWithSupperGas = 0.6f;
    public float baseJumpDuration = 1.2f;
    public float jumpDuration;
    public float jumpHeight = 2f;
    public float jumpDistance = 5f;
    private bool usingSuperGas = false;

    private int currentCash = 0;
    private Animator animator;
    public ParticleSystem jumpEffect;

    private bool isJumping = false;
    private Vector3 jumpStart;
    private Vector3 jumpTarget;
    private float jumpProgress;
    private Vector3 moveDir;
    private float lastInputTime;

    // Rotation control
    private Quaternion startRotation;
    private Quaternion targetRotation;
    public int health = 3;
    public int gassBaseAmount = 100;
    public int gassCurrentAmount = 100;
    public int gassReductionPerJump = 5;
    private GameManager gameManager;
    private InGameUIManager uiManager;
    private AudioSource audioSource;
    public bool gameOver = false;

    public ParticleSystem damageEffect;
    public AudioClip damageSound;
    private bool imuneToDamage = false;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        gameManager = GameManager.Instance;
        uiManager = GameObject.Find("InGameUIManager").GetComponent<InGameUIManager>();
        uiManager.setGasBar(gassCurrentAmount, gassBaseAmount);
        uiManager.SetHealthText(health);
        jumpEffect.Stop();
        uiManager.SetCashText(currentCash);
        animator = GetComponentInChildren<Animator>();
        platformSpawner = GameObject.Find("PlatformSpawner").GetComponent<PlatformSpawner>();
        jumpDistance = platformSpawner.Spacing * 0.8f;
        Respawn();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isJumping && !usingSuperGas && gassCurrentAmount > (2*gassReductionPerJump)) 
        {
            gassCurrentAmount -= 2*gassReductionPerJump;
            uiManager.setGasBar(gassCurrentAmount, gassBaseAmount);
            jumpDuration *= multiplayerJumpDurationWithSupperGas;            
        }
        }
        if (isJumping)
        {
            animator.SetFloat("Blend", 1);
            jumpEffect.Play();

            UpdateJump();
            return;
        } else
        {
            jumpEffect.Stop();
            animator.SetFloat("Blend", 0);
        }

        // 🔹 WASD = world-relative (not camera-relative)
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if (h != 0 || v != 0)
        {
            // World-relative direction
            moveDir = new Vector3(h, 0, v).normalized;
            lastInputTime = Time.time;
        }
        else if (lastInputTime > 0f && Time.time - lastInputTime < 0.2f)
        {
            StartJump();
            lastInputTime = 0f;
        }
        
    }
    // You need a method that Invoke can target to stop the audio
    void StopAudio()
    {
        audioSource.Stop();
    }

    // The flexible method to play your clips
    void playClip(AudioClip clip, float pitch = 1f, float fromTimeToPlay = 0f, float? toTimeToPlay = null)
    {
        // 1. Set the necessary properties on the AudioSource
        audioSource.clip = clip;
        audioSource.pitch = pitch;
        audioSource.time = fromTimeToPlay; // Set the starting position

        // 2. Start playback
        audioSource.Play(); // Must use Play() for timed control (not PlayOneShot)

        // 3. Conditional Stopping Logic
        if (toTimeToPlay.HasValue)
        {
            // Calculate the duration we need to play the clip for
            float durationOfSegment = toTimeToPlay.Value - fromTimeToPlay;

            // Calculate the real time delay needed to stop. If pitch is 2.0 (twice as fast), 
            // the delay must be halved.
            float delayBeforeStopping = durationOfSegment / pitch;

            // Use Invoke to call the StopAudio method after the calculated delay
            Invoke("StopAudio", delayBeforeStopping);
        }
    }

    void StartJump()
    {
        if (gassCurrentAmount < gassReductionPerJump)
        {
            Debug.Log("⛽ Not enough gas to jump!");
            jumpDuration = baseJumpDuration * multiplayerJumpDurationWithOutGas;
            
            
            
        }
        
        else
        {
            gassCurrentAmount -= gassReductionPerJump;
            uiManager.setGasBar(gassCurrentAmount, gassBaseAmount);
            jumpDuration = baseJumpDuration;

        }
        playClip(jumpSound, 1, 3f, 3+jumpDuration);

        
        isJumping = true;
        jumpStart = transform.position;

        Vector3 forwardTarget = jumpStart + moveDir * jumpDistance;
        jumpTarget = platformSpawner.GetNearestCenter(forwardTarget);
        jumpTarget.y += 1f;

        jumpProgress = 0f;

        // 🧭 Rotation setup
        Vector3 flatDir = (jumpTarget - jumpStart);
        flatDir.y = 0f;
        if (flatDir != Vector3.zero)
        {
            startRotation = transform.rotation;
            targetRotation = Quaternion.LookRotation(flatDir);
        }

        // Debug.Log($"🚀 Jump started toward {jumpTarget}");
    }

    void UpdateJump()
    {
        jumpProgress += Time.deltaTime / jumpDuration;

        // Height curve (0.2 up, 0.6 forward, 0.2 down)
        float t = jumpProgress;
        float heightFactor;
        if (t < 0.2f) heightFactor = t / 0.2f;
        else if (t < 0.8f) heightFactor = 1f;
        else heightFactor = 1f - ((t - 0.8f) / 0.2f);

        float yOffset = Mathf.Sin(heightFactor * Mathf.PI / 2) * jumpHeight;

        Vector3 currentPos = Vector3.Lerp(jumpStart, jumpTarget, t);
        currentPos.y = jumpStart.y + yOffset;
        transform.position = currentPos;

        // 🎯 Smooth rotation for first 0.4 of jump
        if (t < 0.4f)
        {
            float rotT = t / 0.4f;
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, rotT);
        }
        else
        {
            transform.rotation = targetRotation;
        }

        if (t >= 1f)
        {
            transform.position = new Vector3(jumpTarget.x, 1.5f, jumpTarget.z);
            
            isJumping = false;
            // Debug.Log("✅ Landed!");
        }
    }
    public void TakeDamage(int? damage)
    {
        if (imuneToDamage) return;
        int dmg = damage ?? 1;
        StartCoroutine(TakingdamageEffect());
        health -= dmg;
        uiManager.SetHealthText(health);
        if (health <= 0)
        {
            Debug.Log("💀 Player has died!");
            // Handle player death (e.g., respawn, game over, etc.)
            // Respawn();
            // health = 3; // Reset health on respawn
            
        }
        else
        {
            Debug.Log("⚠️ Player took damage! Remaining health: " + health);
        }
    }

    void Respawn()
    {
        transform.position = platformSpawner.GetRandomCenter() + Vector3.up * 2f;
    }
    public void AddCash(int amount)
    {
        currentCash += amount;
        uiManager.SetCashText(currentCash);
    }
    private IEnumerator TakingdamageEffect()
    {
        damageEffect.Play();
        imuneToDamage = true;
        audioSource.volume = 0.7f;
        audioSource.PlayOneShot(damageSound);
        yield return new WaitForSeconds(2f);
        audioSource.Stop();
        imuneToDamage = false;
        damageEffect.Stop();
    }
}
