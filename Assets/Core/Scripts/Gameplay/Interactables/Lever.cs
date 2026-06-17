using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

/// <summary>
/// Lever:
/// Palanca interactuable con animación.
/// Al interactuar:
/// - alterna entre ON/OFF
/// - rota visualmente la palanca usando DOTween
/// - ejecuta eventos
/// </summary>
public class Lever : MonoBehaviour, IInteractable
{
    // -------------------------
    // 🎮 Referencias visuales
    // -------------------------

    /// <summary>
    /// Parte visual de la palanca que rota.
    /// </summary>
    [SerializeField] private Transform _leverVisual;

    // -------------------------
    // 🎬 Configuración animación
    // -------------------------

    [SerializeField] private float _rotationAngle = 28.168f;
    [SerializeField] private float _rotationDuration = 0.25f;

    // -------------------------
    // 📡 Eventos
    // -------------------------

    [Header("Events")]
    [SerializeField] private UnityEvent _onActivated;
    [SerializeField] private UnityEvent _onDeactivated;

    // -------------------------
    // 📦 Estado interno
    // -------------------------

    /// <summary>
    /// Estado actual de la palanca.
    /// </summary>
    private bool _isOn;

    // -------------------------
    // 🎮 Interacción
    // -------------------------

    public void Interact()
    {
        Toggle();
    }

    // -------------------------
    // 🔄 Cambio de estado
    // -------------------------

    private void Toggle()
    {
        _isOn = !_isOn;

        RotateLever();

        if (_isOn)
        {
            _onActivated?.Invoke();
        }
        else
        {
            _onDeactivated?.Invoke();
        }
    }

    // -------------------------
    // 🎬 Animación visual
    // -------------------------

    /// <summary>
    /// Rota suavemente la palanca usando DOTween.
    /// </summary>
    private void RotateLever()
    {
        if (_leverVisual == null)
            return;

        float targetX = _isOn
            ? _rotationAngle
            : -_rotationAngle;

        Vector3 targetRotation = new Vector3(
            targetX,
            _leverVisual.localEulerAngles.y,
            _leverVisual.localEulerAngles.z
        );

        _leverVisual
            .DOLocalRotate(targetRotation, _rotationDuration)
            .SetEase(Ease.OutQuad);
    }
}