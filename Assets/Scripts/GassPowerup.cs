using UnityEngine;

public class GassPowerup : MonoBehaviour
{
    public float powerupValue = 1;
    public int gasValueToAdd = 20;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
                Debug.Log("🛢️ Gas Powerup Collected! Current Gas: " + playerController.gassCurrentAmount);
                Destroy(gameObject);
            }
        }
    } 
}
