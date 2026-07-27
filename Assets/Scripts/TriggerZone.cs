using UnityEngine;

public class TriggerZone : MonoBehaviour
{
    public enum ZoneType { Heal, Damage }

    [Header("Zone Settings")]
    [SerializeField] private ZoneType zoneType = ZoneType.Heal;
    [SerializeField] private float amountPerSecond = 20f;

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<Health>(out var health))
        {
            float amount = amountPerSecond * Time.deltaTime;

            if (zoneType == ZoneType.Heal)
            {
                health.Heal(amount);
            }
            else if (zoneType == ZoneType.Damage)
            {
                health.TakeDamage(amount);
            }
        }
    }
}