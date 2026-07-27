using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class CripAI : MonoBehaviour
{
    private NavMeshAgent _agent;
    private Transform _targetBase;

    public void Initialize(Transform targetBase)
    {
        _agent = GetComponent<NavMeshAgent>();
        _targetBase = targetBase;

        if (_targetBase != null)
        {
            _agent.SetDestination(_targetBase.position);
        }
    }

    private void Update()
    {
        if (_targetBase != null && !_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
        {
            if (_targetBase.TryGetComponent<Health>(out var baseHealth))
            {
                baseHealth.TakeDamage(10f);
            }

            Destroy(gameObject);
        }
    }
}