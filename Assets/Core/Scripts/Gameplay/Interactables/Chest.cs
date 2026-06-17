using UnityEngine;

/// <summary>
/// Chest:
/// Objeto interactuable del nivel.
/// Cuando el jugador lo activa:
/// - ejecuta animación
/// - notifica al sistema global que el nivel se ha completado
/// </summary>
public class Chest : MonoBehaviour, IInteractable
{
    // -------------------------
    // 🎬 Referencias visuales
    // -------------------------

    [SerializeField] private Animator _animator;
    [SerializeField] private AudioEmitter _audioEmitter;

    // -------------------------
    // 📦 Estado interno
    // -------------------------

    /// <summary>
    /// Evita que el cofre pueda abrirse más de una vez.
    /// </summary>
    private bool _isOpened;

    // -------------------------
    // 🎮 Interacción
    // -------------------------

    /// <summary>
    /// Método llamado por el sistema de interacción del jugador.
    /// </summary>
    public void Interact()
    {
        Open();
    }

    // -------------------------
    // 🪙 Lógica de apertura
    // -------------------------

    private void Open()
    {
        // Evita reapertura y doble trigger
        if (_isOpened)
            return;

        _isOpened = true;

        // Reproduce animación del cofre
        _animator.Play("Chest_Shake");

        // Reproduce sonido de apertura
        _audioEmitter?.Play();

        // Notifica al sistema global que el cofre ha sido abierto
        // Esto normalmente desencadena:
        // → fin de nivel
        // → transición en GameManager
        GameEvents.RaiseChestOpened();
    }
}