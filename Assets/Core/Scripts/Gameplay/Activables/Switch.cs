using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Switch:
/// Interruptor activado mediante trigger.
///
/// Cuando algo entra:
/// - activa GameObject on
/// - desactiva GameObject off
/// - ejecuta evento OnActivated
///
/// Cuando algo sale:
/// - desactiva GameObject on
/// - activa GameObject off
/// - ejecuta evento OnDeactivated
/// </summary>
public class Switch : MonoBehaviour
{
    // -------------------------
    // 🎮 Objetos controlados
    // -------------------------

    /// <summary>
    /// Objeto que se activa al entrar en el trigger.
    /// </summary>
    [SerializeField] private GameObject _on;

    /// <summary>
    /// Objeto que se desactiva al entrar en el trigger.
    /// </summary>
    [SerializeField] private GameObject _off;

    // -------------------------
    // 📡 Eventos opcionales
    // -------------------------

    [Header("Events")]
    [SerializeField] private UnityEvent _onActivated;
    [SerializeField] private UnityEvent _onDeactivated;

    [Header("Audio")]
    [SerializeField] private AudioEmitter _activatedAudioEmitter;
    [SerializeField] private AudioEmitter _deactivatedAudioEmitter;

    // -------------------------
    // 🟩 Trigger Enter
    // -------------------------

    private void OnTriggerEnter(Collider other)
    {
        // Activa objeto ON
        if (_on != null)
        {
            _on.SetActive(true);
        }

        // Desactiva objeto OFF
        if (_off != null)
        {
            _off.SetActive(false);
        }

        _activatedAudioEmitter?.Play();

        // Ejecuta eventos adicionales
        _onActivated?.Invoke();
    }

    // -------------------------
    // 🟥 Trigger Exit
    // -------------------------

    private void OnTriggerExit(Collider other)
    {
        // Desactiva objeto ON
        if (_on != null)
        {
            _on.SetActive(false);
        }

        // Activa objeto OFF
        if (_off != null)
        {
            _off.SetActive(true);
        }

        _deactivatedAudioEmitter?.Play();

        // Ejecuta eventos adicionales
        _onDeactivated?.Invoke();
    }
}