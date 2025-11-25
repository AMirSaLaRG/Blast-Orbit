using UnityEngine;

public class RandomPowerupRespawner : RandomEventManager
{
    protected override void SpecialRespawn(SpawnService spawnService)
    {
        spawnService.SpawnRandomPowerup();
    }
}
