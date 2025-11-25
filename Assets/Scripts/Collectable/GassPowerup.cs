using UnityEngine;

public class GassPowerup : PickableEffects
{
    [SerializeField] private int gasValueToAdd = 20;

    protected override void ApplyUniqueEffect(PlayerController playerController)
    {
        playerController.GetGas(gasValueToAdd);
    }
    
}
