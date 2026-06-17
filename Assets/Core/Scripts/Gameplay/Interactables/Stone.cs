using UnityEngine;
using DG.Tweening;

/// <summary>
/// Stone:
/// Objeto interactuable que puede ser empujado por el jugador
/// en una dirección cardinal (tipo Sokoban / Pokémon puzzles).
/// </summary>
public class Stone : MonoBehaviour, IInteractable
{
    // -------------------------
    // 🎮 Configuración de movimiento
    // -------------------------

    [SerializeField] private float _moveDistance = 1f;
    [SerializeField] private float _moveDuration = 0.6f;

    // -------------------------
    // 📦 Estado interno
    // -------------------------

    /// <summary>
    /// Evita que la piedra se mueva mientras ya está en animación.
    /// </summary>
    private bool _isMoving;

    /// <summary>
    /// Referencia al PlayerController para obtener la dirección de facing.
    /// </summary>
    private PlayerController _playerController;

    // -------------------------
    // 🔧 Inicialización
    // -------------------------

    private void Start()
    {
        // Busca el player en escena (solución rápida de prototipo)
        _playerController = FindFirstObjectByType<PlayerController>();
    }

    // -------------------------
    // 🎮 Interacción
    // -------------------------

    /// <summary>
    /// Llamado por el sistema de interacción del jugador.
    /// Intenta empujar la piedra en la dirección en la que mira el jugador.
    /// </summary>
    public void Interact()
    {
        // Evita doble movimiento o referencias nulas
        if (_isMoving || _playerController == null)
            return;

        // Dirección en la que está mirando el jugador (estado real del personaje)
        Vector3 direction = _playerController.FacingDirection;

        // Si no hay dirección válida, no se mueve
        if (!CanMove(direction))
            return;

        Move(direction);
    }

    // -------------------------
    // 🚶 Movimiento
    // -------------------------

    /// <summary>
    /// Mueve la piedra suavemente usando DOTween.
    /// </summary>
    private void Move(Vector3 direction)
    {
        direction.y = 0;
        direction.Normalize();

        Vector3 targetPosition = transform.position + direction * _moveDistance;

        _isMoving = true;

        transform
            .DOMove(targetPosition, _moveDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                _isMoving = false;
            });
    }

    // -------------------------
    // 🚧 Validación de movimiento
    // -------------------------

    /// <summary>
    /// Comprueba si la piedra puede moverse en la dirección indicada.
    /// Usa un raycast para detectar obstáculos en la casilla destino.
    /// </summary>
    private bool CanMove(Vector3 direction)
    {
        Vector3 origin = transform.position + Vector3.up * 0.5f;

        float checkDistance = _moveDistance;

        // Si hay colisión delante, no se puede mover
        if (Physics.Raycast(origin, direction, checkDistance))
        {
            return false;
        }

        return true;
    }
}