using Combatant;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    private int counter = 1;
    private int spawnCount = 0;
    public float zOffset;
    private float initialOffset;

    public void SetEnemiesToSpawn(int howMany)
    {
        spawnCount = howMany;
        if (spawnCount % 2 == 0)
        {
            initialOffset = zOffset * 0.5f;
        } else
        {
            initialOffset = 0;
        }
    }

    public GameObject SpawnEnemy(Enemy e)
    {
        int midPoint = spawnCount / 2 + 1;
        if (spawnCount == 1)
        {
            midPoint = 0;
        }

        Vector3 spawnLocation = new Vector3(0, 0, initialOffset + (zOffset * (counter - midPoint)));
        GameObject spawnedEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        spawnedEnemy.GetComponent<EnemyGameObject>().SetCombatant(e);
        spawnedEnemy.GetComponent<EnemyGameObject>().SetUpStrategy();
        spawnedEnemy.transform.parent = transform;
        spawnedEnemy.transform.localPosition = spawnLocation;
        counter++;
        return spawnedEnemy;
    }
}
