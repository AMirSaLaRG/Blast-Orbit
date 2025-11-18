using UnityEngine;

public class GassPowerup : MonoBehaviour
{
    public float powerupValue = 1;
    public int gasValueToAdd = 20;
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
                playerController.gassCurrentAmount += (int)gasValueToAdd;
                if (playerController.gassCurrentAmount > playerController.gassBaseAmount)
                {
                    playerController.gassCurrentAmount = playerController.gassBaseAmount;
                }
                InGameUIManager uiManager = GameObject.Find("InGameUIManager").GetComponent<InGameUIManager>();
                uiManager.setGasBar(playerController.gassCurrentAmount, playerController.gassBaseAmount);
                // Debug.Log("🛢️ Gas Powerup Collected! Current Gas: " + playerController.gassCurrentAmount);
                
                // Cancel any invokes on the PickableEffects component (public MonoBehaviour API) and destroy its GameObject.
                if (pickableEffects != null)
                {
                    pickableEffects.StopInvoking();
                    Destroy(gameObject);
                }
            }
        }
    } 
    
}
