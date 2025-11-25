using UnityEngine;

public class PowerupHealth : PickableEffects
{
    [SerializeField] private int heal = 1;

    protected override void ApplyUniqueEffect(PlayerController playerController)
    {
        playerController.Heal(heal);
    }
}
