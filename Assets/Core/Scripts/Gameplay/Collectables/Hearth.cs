using UnityEngine;
using DG.Tweening;

/// <summary>
/// Heart:
/// Coleccionable de vida.
/// - Flota en Y con movimiento tipo ola usando seno
/// - Rota en eje Y
/// - Al recogerlo, da vida al jugador
/// </summary>
public class Heart : MonoBehaviour, ICollectable
{
    [Header("Wobble")]
    [SerializeField] private float _amplitude = 0.25f;
    [SerializeField] private float _frequency = 2f;

    [Header("Rotation")]
    [SerializeField] private float _rotationSpeed = 90f;

    [Header("Audio")]
    [SerializeField] private AudioEmitter _audioEmitter;

    private Vector3 _startPosition;

    private void Start()
    {
        _startPosition = transform.position;
    }

    private void Update()
    {
        ApplyWobble();
        ApplyRotation();
    }

    /// <summary>
    /// Se llama cuando el jugador recoge el corazón.
    /// </summary>
    public void Collect()
    {
        PlayerHealth player = FindFirstObjectByType<PlayerHealth>();

        if (player != null)
        {
            _audioEmitter?.Play();

            player.Heal(1);
        }

        Destroy(gameObject);
    }

    /// <summary>
    /// Movimiento tipo ola en eje Y.
    /// </summary>
    private void ApplyWobble()
    {
        float yOffset = Mathf.Sin(Time.time * _frequency) * _amplitude;

        transform.position = new Vector3(
            _startPosition.x,
            _startPosition.y + yOffset,
            _startPosition.z
        );
    }

    /// <summary>
    /// Rotación constante en eje Y.
    /// </summary>
    private void ApplyRotation()
    {
        transform.Rotate(
            0f,
            _rotationSpeed * Time.deltaTime,
            0f,
            Space.World
        );
    }
}