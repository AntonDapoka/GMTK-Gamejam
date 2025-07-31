using UnityEngine;
using UnityEngine.AI;

public class EnemyMovementScript : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private GameObject target;

    private void Update()
    {
        agent.SetDestination(target.transform.position);
    }
}
