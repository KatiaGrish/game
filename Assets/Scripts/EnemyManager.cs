using System.Collections.Generic; 
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<Enemy> enemyInTrigger = new List<Enemy>();

    public void AddEnemy(Enemy enemy)
    {
        enemyInTrigger.Add(enemy);
    }

    public void RemoveEnemy(Enemy enemy)
    {
        enemyInTrigger.Remove(enemy);
    }
}
