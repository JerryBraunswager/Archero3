using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : Shell
{
    protected override void OnEnemyCollision(Entity entity) {}

    protected override void OnObstacleCollision(Transform obstacle) {}

    protected override void OnPlayerCollision(Entity entity)
    {
        entity.TakeDamage(damageReport);
        shooter.DeactivateShell(gameObject);
    }
}
