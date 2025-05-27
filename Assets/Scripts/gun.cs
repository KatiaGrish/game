using UnityEngine;
using System.Collections.Generic;

public class Gun : MonoBehaviour
{
    [Header("Settings")]
    public float range = 1f;
    public float verticalRange = 0.2f;
    public float gunShotRadius = 1f;
    public float bigDamage = 20f;
    public float smallDamage = 1f;
    public float fireRate = 1f;
    
    [Header("Layers")]
    public LayerMask raycastLayerMask;
    public LayerMask enemyLayerMask;

    [Header("References")]
    public EnemyManager enemyManager;
    
    private BoxCollider gunTrigger;
    private AudioSource audioSource;
    private float nextTimeToFire;

    void Start()
    {
        gunTrigger = GetComponent<BoxCollider>();
        audioSource = GetComponent<AudioSource>();
        
        gunTrigger.size = new Vector3(0.05f, verticalRange, range);
        gunTrigger.center = new Vector3(0, -0.05f, range * 0.5f);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time > nextTimeToFire)
        {
            Fire();
        }
    }

    void Fire()
    {
        // Активация агрессии у врагов в радиусе
        Collider[] enemyColliders = Physics.OverlapSphere(transform.position, gunShotRadius, enemyLayerMask);
        foreach (var collider in enemyColliders)
        {
            EnemyAwareness awareness = collider.GetComponent<EnemyAwareness>();
            if (awareness != null)
            {
                awareness.isAggro = true;
            }
        }

        // Воспроизведение звука
        audioSource.Stop();
        audioSource.Play();

        // Обработка врагов
        List<Enemy> activeEnemies = GetValidEnemies();
        foreach (Enemy enemy in activeEnemies)
        {
            ProcessEnemyHit(enemy);
        }

        nextTimeToFire = Time.time + fireRate;
    }

    private List<Enemy> GetValidEnemies()
    {
        List<Enemy> validEnemies = new List<Enemy>();
        
        foreach (Enemy enemy in enemyManager.enemyInTrigger)
        {
            if (enemy != null && enemy.transform != null)
            {
                validEnemies.Add(enemy);
            }
            else
            {
                enemyManager.RemoveEnemy(enemy);
            }
        }
        
        return validEnemies;
    }

    private void ProcessEnemyHit(Enemy enemy)
    {
        Vector3 direction = enemy.transform.position - transform.position;
        Debug.DrawRay(transform.position, direction.normalized * range * 1.5f, Color.red, 1f);

        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, range * 1.5f, raycastLayerMask))
        {
            if (hit.transform == enemy.transform)
            {
                float distance = Vector3.Distance(enemy.transform.position, transform.position);
                float damage = distance > range * 0.5f ? smallDamage : bigDamage;
                enemy.TakeDamage(damage);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            enemyManager.AddEnemy(enemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            enemyManager.RemoveEnemy(enemy);
        }
    }
}