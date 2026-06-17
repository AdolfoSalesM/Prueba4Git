using UnityEngine;
using DG.Tweening;

/// <summary>
/// Coin:
/// Objeto visual decorativo.
/// - Flota en Y con movimiento tipo ola con seno
/// - Rota solo en eje Y
/// </summary>
public class Coin : MonoBehaviour, ICollectable
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

    public void Collect()
    {
        _audioEmitter?.Play();

        UIManager.Instance.AddCoin(1);
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
        transform.Rotate(0f, _rotationSpeed * Time.deltaTime, 0f, Space.World);
    }
}