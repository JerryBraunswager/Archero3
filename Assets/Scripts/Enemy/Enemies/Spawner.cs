using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private const float overlapSphereRadius = 0.5f;
    [SerializeField] private int spawnCount;
    [SerializeField] private float spawnDelay;
    [SerializeField] private ShootingFlyEnemy spawner;
    [SerializeField] private ShootingFlyEnemy spawnable;
    [SerializeField] private GlobalEnemyHandler globalEnemyHandler;
    private EnemyHandler enemyHandler;

    private int alives;
    private float temporarySpawnDelay;

    private void Start()
    {
        enemyHandler = globalEnemyHandler.componentCache;
        Spawn();
        temporarySpawnDelay = spawnDelay;
    }

    private void Update()
    {
        if(alives == 0)
        {
            temporarySpawnDelay -= Time.deltaTime;

            if(temporarySpawnDelay <= 0)
            {
                Spawn();
                temporarySpawnDelay = spawnDelay;
            }
        }
    }

    private void Spawn()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 spawnPos;
            int loopBreaker = 0;
            do
            {
                loopBreaker++;
                spawnPos = new Vector3(transform.position.x + Random.Range(-1f, 1f), 0.51f, transform.position.z + Random.Range(1f, 1f));//Значения вытаскиваются из конструктора поля или из данных о префабе поля, если игровые поля будут разного размера
            } while (CheckCollisions(spawnPos) && loopBreaker < 100); //Может войти в бесконечный цикл, если не будет места для спавна
            if (loopBreaker < 100)
            {
                var newEnemy = Instantiate(spawnable, transform.parent);
                newEnemy.transform.position = spawnPos;
                alives++;
                newEnemy.GetComponent<ShootingFlyEnemy>().Died += Spawned_Died;
                enemyHandler.AddEnemy(newEnemy.GetComponent<Enemy>());
                newEnemy.GetComponent<Enemy>().Init(globalEnemyHandler);
            }
        }
    }

    private void Spawned_Died(ShootingFlyEnemy entity)
    {
        alives--;
        entity.Died -= Spawned_Died;
    }

    private bool CheckCollisions(Vector3 pos)
    {
        Collider[] hitColliders = Physics.OverlapSphere(pos, overlapSphereRadius);
        return hitColliders.Length > 0;
    }
}
