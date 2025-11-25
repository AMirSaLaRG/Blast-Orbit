using UnityEngine;

public class RandomBasicBombRespawner : RandomEventManager
{
    protected override void SpecialRespawn(SpawnService spawnService)
    {
        spawnService.SpawnRandomBasicEnemy();
    }
}
