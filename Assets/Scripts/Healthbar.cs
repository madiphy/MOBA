using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Health targetHealth;
    [SerializeField] private Image healthFillImage;

    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;

        if (targetHealth == null)
        {
            targetHealth = GetComponentInParent<Health>();
        }
    }

    private void OnEnable()
    {
        if (targetHealth != null)
        {
            targetHealth.OnHealthChanged += UpdateHealthBar;
        }
    }

    private void OnDisable()
    {
        if (targetHealth != null)
        {
            targetHealth.OnHealthChanged -= UpdateHealthBar;
        }
    }

    private void Start()
    {
        if (targetHealth != null)
        {
            UpdateHealthBar(targetHealth.CurrentHealth, targetHealth.MaxHealth);
        }
    }

    private void LateUpdate()
    {
        if (_mainCamera != null)
        {
            transform.rotation = _mainCamera.transform.rotation;
        }
    }

    private void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        if (healthFillImage != null && maxHealth > 0f)
        {
            healthFillImage.fillAmount = currentHealth / maxHealth;
        }
    }
}
