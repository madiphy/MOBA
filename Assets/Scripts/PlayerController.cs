using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Health))]
public class PlayerController : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackDamage = 25f;
    [SerializeField] private float attackCooldown = 1f;

    [Header("Respawn Settings")]
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private float respawnTime = 3f;

    private NavMeshAgent _agent;
    private Camera _mainCamera;
    private Health _health;
    private Health _currentTarget;
    private float _nextAttackTime;
    private Vector3 _initialPosition;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _health = GetComponent<Health>();
        _mainCamera = Camera.main;
        _initialPosition = transform.position;
    }

    private void OnEnable()
    {
        if (_health != null)
            _health.OnDied += HandleDeath;
    }

    private void OnDisable()
    {
        if (_health != null)
            _health.OnDied -= HandleDeath;
    }

    private void Update()
    {
        if (_health.IsDead) return;

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

    private void HandleDeath()
    {
        Debug.Log("<color=orange>Игрок погиб! Начинается отсчет возрождения...</color>");
        StartCoroutine(RespawnRoutine());
    }

    private IEnumerator RespawnRoutine()
    {
        _currentTarget = null;
        _agent.isStopped = true;
        _agent.ResetPath();

        SetPlayerActive(false);

        yield return new WaitForSeconds(respawnTime);

        Vector3 targetPosition = respawnPoint != null ? respawnPoint.position : _initialPosition;
        _agent.Warp(targetPosition);

        _health.RespawnHealth();
        SetPlayerActive(true);
        _agent.isStopped = false;

        Debug.Log("<color=cyan>Игрок успешно возродился на базе!</color>");
    }

    private void SetPlayerActive(bool active)
    {
        if (TryGetComponent<Renderer>(out var renderer)) renderer.enabled = active;
        if (TryGetComponent<Collider>(out var col)) col.enabled = active;
    }
}
