using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShootingFlyEnemy : FlyingEnemy
{
    [SerializeField] private Shooter shooter;
    [SerializeField] private EnemyAimer aimer;
    float lastShootTime;

    public event UnityAction<ShootingFlyEnemy> Died;

    protected override void Death(Entity killer)
    {
        Died?.Invoke(this);
        shooter.Dispose();
        base.Death(killer);
    }

    protected new void Awake()
    {
        base.Awake();
        if (shooter == null)
            shooter = GetComponentInChildren<Shooter>();
        if (aimer == null)
            aimer = GetComponentInChildren<EnemyAimer>();
    }

    protected new void Update()
    {
        base.Update();
        if (aimer.Target != null)
        {
            walkingState = MovingState.STAYING;
            aimer.FollowTarget();
            if (Time.time - lastShootTime >= (1 / attackSpeed))
            {
                lastShootTime = Time.time;
                shooter.Shoot(new DamageReport(damage, this));
            }
        }
    }
    protected void FixedUpdate()
    {
        if (walkingState == MovingState.STAYING)
        {
            if (aimer.Target == null)
                aimer.Aim();
            else if (!aimer.IsVisible())
                aimer.ResetTarget();
        }

    }
}
