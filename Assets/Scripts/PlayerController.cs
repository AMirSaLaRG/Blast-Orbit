using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private bool isGamePause = false;
    [Header("Jump Settings")]
    private PlatformSpawner platformSpawner;
    [SerializeField] private float multiplayerJumpDurationWithOutGas = 2.1f;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private float multiplayerJumpDurationWithSupperGas = 0.6f;
    [SerializeField] private float baseJumpDuration = 1.2f;
    [SerializeField] private float jumpDuration;
    [SerializeField] private float jumpHeight = 2f;
    private float jumpDistance = 5f;
    private bool usingSuperGas = false;

    public int currentCash  { get; private set; } = 0;
    private Animator animator;
    [SerializeField] private ParticleSystem jumpEffect;

    private bool isJumping = false;
    private Vector3 jumpStart;
    private Vector3 jumpTarget;
    private float jumpProgress;
    private Vector3 moveDir;
    private float lastInputTime;

    // Rotation control
    private Quaternion startRotation;
    private Quaternion targetRotation;
    [SerializeField] private int health = 3;
    [SerializeField] private int gassBaseAmount = 100;
    [SerializeField] private int gassCurrentAmount = 100;
    [SerializeField] private int gassReductionPerJump = 5;
    private GameManager gameManager;
    private InGameUIManager uiManager;
    private AudioSource audioSource;

    [SerializeField] private ParticleSystem damageEffect;
    [SerializeField] private AudioClip damageSound;
    private bool imuneToDamage = false;
    void Start()
    {
        ApplyUpgrades();
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
    private void ApplyUpgrades()
    {
        gassBaseAmount += (int)UserSetting.Instance.characterGasLvl;
        gassCurrentAmount = gassBaseAmount;
        health += UserSetting.Instance.characterMaxHealthLvl;
        // this number will be between 1 to 100
        float currentSpeedUpdated = UserSetting.Instance.characterSpeedLvl;
        // i want 2 upgrade make player 1 persent faster up to 50persent faster
        float calculateHowMuchFaster = currentSpeedUpdated * 0.005f;
        float multipllerToMkaItFaster = 1 - calculateHowMuchFaster ;
        baseJumpDuration *= multipllerToMkaItFaster;

    }

    void Update()
    {
        if (isGamePause)
        {
            Time.timeScale = 0;
        }
        GoToMainMenu();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (usingSuperGas) return;
            if (gassCurrentAmount < (2*gassReductionPerJump)) return;
            if (!isJumping) return;
            usingSuperGas = true;
            gassCurrentAmount -= 2*gassReductionPerJump;
            uiManager.setGasBar(gassCurrentAmount, gassBaseAmount);
            jumpDuration *= multiplayerJumpDurationWithSupperGas;            
        
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
    public void ResetCash()
    {
        currentCash = 0;
       
    }
    public void GetGas(float gasToGet)
    {
        gassCurrentAmount += (int)gasToGet;
        if (gassCurrentAmount > gassBaseAmount)
        {
            gassCurrentAmount = gassBaseAmount;
        }
        uiManager.setGasBar(gassCurrentAmount, gassBaseAmount);
        // Debug.Log("🛢️ Gas Powerup Collected! Current Gas: " + playerController.gassCurrentAmount);
        
        // Cancel any invokes on the PickableEffects component (public MonoBehaviour API) and destroy its GameObject.
       
    }
    // You need a method that Invoke can target to stop the audio
    void StopAudio()
    {
        audioSource.Stop();
    }
    void GoToMainMenu()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            isGamePause = true;
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }
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
            usingSuperGas = false;
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
