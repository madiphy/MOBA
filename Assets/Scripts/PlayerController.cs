using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerController : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackDamage = 25f;
    [SerializeField] private float attackCooldown = 1f;

    private NavMeshAgent _agent;
    private Camera _mainCamera;
    private Health _currentTarget;
    private float _nextAttackTime;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        HandleInput();
        HandleAttackLogic();
    }

    private void HandleInput()
    {
        if (Mouse.current != null && Mouse.current.rightButton.wasPressedThisFrame)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Ray ray = _mainCamera.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.TryGetComponent<Health>(out var targetHealth) && targetHealth.gameObject != gameObject)
                {
                    _currentTarget = targetHealth;
                }
                else
                {
                    _currentTarget = null;
                    _agent.SetDestination(hit.point);
                }
            }
        }
    }

    private void HandleAttackLogic()
    {
        if (_currentTarget == null) return;

        if (_currentTarget.IsDead)
        {
            _currentTarget = null;
            return;
        }

        float distanceToTarget = Vector3.Distance(transform.position, _currentTarget.transform.position);

        if (distanceToTarget > attackRange)
        {
            _agent.SetDestination(_currentTarget.transform.position);
        }
        else
        {
            _agent.ResetPath();

            if (Time.time >= _nextAttackTime)
            {
                _currentTarget.TakeDamage(attackDamage);
                _nextAttackTime = Time.time + attackCooldown;
                Debug.Log($"Атака по {_currentTarget.name}! Нанесено урона: {attackDamage}");
            }
        }
    }
}
