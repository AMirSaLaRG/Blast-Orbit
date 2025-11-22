using UnityEngine;

public class CashPickUp : PickableEffects
{
    [SerializeField] private int cashAmount = 10;

    protected override void ApplyUniqueEffect(PlayerController playerController)
    {
        playerController.AddCash(cashAmount);
    }
}
