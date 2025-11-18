using UnityEngine;

public class CashPickUp : MonoBehaviour
{
    public int cashAmount = 10;
    private PickableEffects pickableEffects;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pickableEffects = GetComponent<PickableEffects>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.AddCash(cashAmount);
                if (pickableEffects != null)
                {
                    pickableEffects.StopInvoking();
                    Destroy(gameObject);
                }
            }
        }
    }
}
