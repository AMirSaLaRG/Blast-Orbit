using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Jump Settings")]
    public PlatformSpawner platformSpawner;
    public float jumpDuration = 1f;
    public float jumpHeight = 2f;
    public float jumpDistance = 5f;

    private bool isJumping = false;
    private Vector3 jumpStart;
    private Vector3 jumpTarget;
    private float jumpProgress;
    private Vector3 moveDir;
    private float lastInputTime;

    // Rotation control
    private Quaternion startRotation;
    private Quaternion targetRotation;

    void Start()
    {
        platformSpawner = GameObject.Find("PlatformSpawner").GetComponent<PlatformSpawner>();
        jumpDistance = platformSpawner.Spacing * 0.8f;
        Respawn();
    }

    void Update()
    {
        if (isJumping)
        {
            UpdateJump();
            return;
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

    void StartJump()
    {
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

    void Respawn()
    {
        transform.position = platformSpawner.GetRandomCenter() + Vector3.up * 2f;
    }
}
