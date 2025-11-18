using UnityEngine;

public class PickableEffects : MonoBehaviour
{
    private float duration = 6f;
    public AudioClip pickUpSound;
    private float timetoApplyEffectEnding = 3f;
    public ParticleSystem gettingOutOfTimeEffect;
    private float startTime = 0;
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
            AudioSource.PlayClipAtPoint(pickUpSound, transform.position);
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
}
