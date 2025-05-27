using UnityEngine;

public class Gun : MonoBehaviour
{
    public float range = 1f;
    public float verticalRange = 0.2f;
    public float gunShotRadius = 1f;

    public float bigDamage = 20f;
    public float smallDamage = 1f;

    public float fireRate = 1f;
    private float nextTimeToFire;

    public LayerMask raycastLayerMask;
    public LayerMask enemyLayerMask;

    private BoxCollider gunTrigger;
    public EnemyManager enemyManager;
    
    void Start()
    {
        gunTrigger = GetComponent<BoxCollider>();
        gunTrigger.size = new Vector3(0.05f, verticalRange, range);
        gunTrigger.center = new Vector3(0, -0.05f, range * 0.5f);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)&& Time.time > nextTimeToFire)
        {
            Fire();
        }
    }

    void Fire()
    {
        Collider[] enemyColliders;
        enemyColliders = Physics.OverlapSphere(transform.position, gunShotRadius, enemyLayerMask);

        foreach (var enemyCollider in enemyColliders)
        {
            enemyCollider.GetComponent<EnemyAwareness>().isAggro = true;
        }

        GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().Play();
        foreach(var enemy in enemyManager.enemyInTrigger)
        {
            var dir = enemy.transform.position - transform.position;
            Debug.DrawRay(transform.position, dir.normalized * range * 1.5f, Color.red, 1f);

            RaycastHit hit;
            if (Physics.Raycast(transform.position, dir, out hit, range * 1.5f, raycastLayerMask))
            {
                if (hit.transform == enemy.transform)
                {
                    float dist = Vector3.Distance(enemy.transform.position, transform.position);

                    if (dist > range * 0.5f)
                    {
                        enemy.TakeDamage(smallDamage);
                    }
                    else
                    {
                        enemy.TakeDamage(bigDamage);
                    }
                }
            }
        }

        nextTimeToFire = Time.time + fireRate;
    }

    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.transform.GetComponent<Enemy>();

        if (enemy)
        {
            enemyManager.AddEnemy(enemy);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Enemy enemy = other.transform.GetComponent<Enemy>();
        if (enemy)
        {
            enemyManager.RemoveEnemy(enemy);
        }
    }

}