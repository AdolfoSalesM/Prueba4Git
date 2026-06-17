using UnityEngine;

/// <summary>
/// PlayerInteraction:
/// Sistema encargado de gestionar la interacción del jugador con objetos del mundo.
/// Detecta objetos interactuables delante del jugador usando raycast.
/// </summary>
public class PlayerInteraction : MonoBehaviour
{
    // -------------------------
    // 🎮 Configuración
    // -------------------------

    /// <summary>
    /// Distancia máxima a la que el jugador puede interactuar.
    /// </summary>
    [SerializeField] private float _interactionDistance = 2f;

    /// <summary>
    /// Capa que define qué objetos pueden ser interactuados.
    /// </summary>
    [SerializeField] private LayerMask _interactableLayer;

    /// <summary>
    /// Referencia al PlayerController para obtener dirección de facing.
    /// </summary>
    [SerializeField] private PlayerController _playerController;

    // -------------------------
    // 🎮 Input
    // -------------------------

    private void Update()
    {
        // Cuando el jugador pulsa "E", intenta interactuar
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteract();
        }
    }

    // -------------------------
    // 🎯 Lógica de interacción
    // -------------------------

    /// <summary>
    /// Lanza un raycast en la dirección en la que está mirando el jugador
    /// para detectar objetos interactuables.
    /// </summary>
    private void TryInteract()
    {
        // Dirección real del personaje (no depende del input)
        Vector3 direction = _playerController.FacingDirection;

        // Punto de origen del raycast (ligeramente elevado para evitar colisiones con el suelo)
        Vector3 origin = transform.position + Vector3.up * 0.5f;

        // Raycast hacia delante del jugador
        if (Physics.Raycast(origin, direction, out RaycastHit hit, _interactionDistance, _interactableLayer))
        {
            // Si el objeto implementa IInteractable, se ejecuta la interacción
            if (hit.collider.TryGetComponent<IInteractable>(out var interactable))
            {
                interactable.Interact();
            }
        }
    }

    // -------------------------
    // 🐛 Debug visual
    // -------------------------

    private void OnDrawGizmosSelected()
    {
        if (_playerController == null)
            return;

        Gizmos.color = Color.yellow;

        Vector3 origin = transform.position + Vector3.up * 0.5f;
        Vector3 direction = _playerController.FacingDirection * _interactionDistance;

        Gizmos.DrawLine(origin, origin + direction);
    }
}