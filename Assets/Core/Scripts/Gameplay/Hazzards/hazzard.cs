using UnityEngine;

/// <summary>
/// Hazzard:
/// Objeto peligroso del escenario.
/// - Hace daño al entrar en trigger
/// </summary>
public class Hazzard : MonoBehaviour
{
    [SerializeField] private int _damage = 1;
    [SerializeField] private float _invulnerabilityDuration = 1.5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerHealth>(out var player))
        {
            player.TakeDamage(_damage);
            player.StartInvulnerability(InvulnerabilityType.Blink, _invulnerabilityDuration);
        }
    }
}