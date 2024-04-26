

using Combatant;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/EnemyFormation")]
public class EnemyFormation : ScriptableObject
{
    public List<Enemy> enemies;
    public List<FormationInventory> enemyInventory;
}