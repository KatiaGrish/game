using UnityEngine;

public class EnemyAwareness : MonoBehaviour
{
    public bool isAggro;

    private void Update()
    {
        if (isAggro)
        {

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.CompareTag("Player"))
        {
            isAggro = true;
        }
    }
}
