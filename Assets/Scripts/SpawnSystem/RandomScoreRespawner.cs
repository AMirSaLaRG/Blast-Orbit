using UnityEngine;

public class RandomScoreRespawner : RandomEventManager
{
    protected override void SpecialRespawn(SpawnService spawnService)
    {
        spawnService.SpawnRandomScore();
    }
}
