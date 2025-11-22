using UnityEngine;

public abstract class PickableEffects : MonoBehaviour
{
    [SerializeField] private float duration = 6f;
    [SerializeField] private AudioClip pickUpSound;
    private float timetoApplyEffectEnding = 3f;
    [SerializeField] private ParticleSystem gettingOutOfTimeEffect;
    [SerializeField] private float startTime = 0;
    private bool isGettingOutOfTime = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke("DestroyThis", duration);
        startTime=0;
        

    }

    // Update is called once per frame
    void Update()
    {
        
        transform.Rotate(Vector3.up * 50f * Time.deltaTime);
        startTime += Time.deltaTime;
        if (startTime >= duration - timetoApplyEffectEnding)
        {
            if (!isGettingOutOfTime)
            {
                isGettingOutOfTime = true;
                gettingOutOfTimeEffect.Play();
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                ApplyUniqueEffect(playerController);
            }
            Debug.Log("got it");
            AudioSource.PlayClipAtPoint(pickUpSound, transform.position);
            StopInvoking();
            Destroy(gameObject);
        }
    }
    public void StopInvoking()
    {
        CancelInvoke("DestroyThis");
    }
    private void DestroyThis()
    {
        Debug.Log("PickableEffect expired, destroying object.");
        Destroy(gameObject);
    }
    protected abstract void ApplyUniqueEffect(PlayerController player);
}
