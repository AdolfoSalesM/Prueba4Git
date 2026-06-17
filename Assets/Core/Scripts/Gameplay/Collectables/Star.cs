using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

/// <summary>
/// Star:
/// Collectable que da invencibilidad temporal con efecto arcoíris.
/// </summary>
public class Star : MonoBehaviour, ICollectable
{
    [Header("Wobble")]
    [SerializeField] private float _amplitude = 0.25f;
    [SerializeField] private float _frequency = 2f;

    [Header("Rotation")]
    [SerializeField] private float _rotationSpeed = 90f;

    [Header("Effect")]
    [SerializeField] private float _invincibilityDuration = 3f;
    [SerializeField] private float _colorChangeSpeed = 0.1f;

    [Header("Audio")]
    [SerializeField] private AudioEmitter _audioEmitter;

    private Vector3 _startPosition;
    private bool _collected;

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
        if (_collected)
            return;

        _collected = true;

        PlayerHealth player = FindFirstObjectByType<PlayerHealth>();

        if (player != null)
        {
            _audioEmitter?.Play();

            player.StartInvulnerability(InvulnerabilityType.Rainbow, _invincibilityDuration);
        }

        Destroy(gameObject);
    }

    private void ApplyWobble()
    {
        float yOffset = Mathf.Sin(Time.time * _frequency) * _amplitude;

        transform.position = new Vector3(
            _startPosition.x,
            _startPosition.y + yOffset,
            _startPosition.z
        );
    }

    private void ApplyRotation()
    {
        transform.Rotate(0f, _rotationSpeed * Time.deltaTime, 0f, Space.World);
    }
}