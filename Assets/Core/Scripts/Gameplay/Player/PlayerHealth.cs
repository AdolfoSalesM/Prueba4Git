using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;

/// <summary>
/// PlayerHealth:
/// Vida + daño + invencibilidad con efectos visuales + knockback.
/// </summary>
public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Life")]
    [SerializeField] private int _maxLife = 3;

    [Header("Invulnerability")]
    [SerializeField] private float _blinkInterval = 0.15f;

    [Header("Knockback")]
    [SerializeField] private float _knockbackDistance = 0.6f;
    [SerializeField] private float _knockbackDuration = 0.12f;

    [Header("Visual")]
    [SerializeField] private Renderer _renderer;

    private int _currentLife;
    private bool _isInvulnerable;

    private Material[] _materials;
    private Tween _knockbackTween;

    private void Awake()
    {
        _materials = _renderer.materials;
    }

    private void Start()
    {
        _currentLife = _maxLife;
        UIManager.Instance.AddLife(_currentLife);
    }

    public void TakeDamage(int amount)
    {
        if (_isInvulnerable)
            return;

        _currentLife -= amount;
        UIManager.Instance.AddLife(-amount);
        Debug.Log("Player DAMAGED -" + amount + " life. Current: " + _currentLife);

        if (_currentLife <= 0)
        {
            Die();
            return;
        }

        _isInvulnerable = true;

        ApplyKnockback();
    }

    public void StartInvulnerability(InvulnerabilityType type, float duration)
    {
        switch (type)
        {
            case InvulnerabilityType.Blink:
                StartBlink(duration).Forget();
                break;

            case InvulnerabilityType.Rainbow:
                StartRainbow(duration).Forget();
                break;
        }
    }

    private void ApplyKnockback()
    {
        _knockbackTween?.Kill();

        Vector3 dir = -transform.forward;
        dir.y = 0;

        Vector3 target = transform.position + dir.normalized * _knockbackDistance;

        _knockbackTween = transform.DOMove(target, _knockbackDuration)
            .SetEase(Ease.OutQuad);
    }

    private async UniTaskVoid StartBlink(float duration)
    {
        _isInvulnerable = true;

        float timer = 0f;
        bool visible = true;

        while (timer < duration)
        {
            visible = !visible;

            foreach (Material mat in _materials)
            {
                Color c = mat.color;
                c.a = visible ? 1f : 0.25f;
                mat.color = c;
            }

            await UniTask.Delay(TimeSpan.FromSeconds(_blinkInterval));
            timer += _blinkInterval;
        }

        ResetAlpha();
        _isInvulnerable = false;
    }

    private async UniTaskVoid StartRainbow(float duration)
    {
        _isInvulnerable = true;

        float timer = 0f;

        while (timer < duration)
        {
            foreach (Material mat in _materials)
            {
                mat.color = Color.HSVToRGB(
                    Mathf.PingPong(Time.time * 2f, 1f),
                    1f,
                    1f
                );
            }

            await UniTask.Delay(TimeSpan.FromSeconds(_blinkInterval));
            timer += _blinkInterval;
        }

        ResetColors();
        _isInvulnerable = false;
    }

    private void ResetAlpha()
    {
        foreach (Material mat in _materials)
        {
            Color c = mat.color;
            c.a = 1f;
            mat.color = c;
        }
    }

    private void ResetColors()
    {
        foreach (Material mat in _materials)
        {
            mat.color = Color.white;
        }
    }

    private void Die()
    {
        Debug.Log("PLAYER DEAD");
        GameEvents.RaiseGameOver();
    }

    public void Heal(int amount)
    {
        _currentLife += amount;

        Debug.Log("Player HEALED +" + amount + " life");

        UIManager.Instance.AddLife(amount);
    }

    public void ForceDamage(int amount)
    {
        _currentLife -= amount;

        UIManager.Instance.AddLife(-amount);

        Debug.Log("Player FORCED DAMAGE -" + amount + " life. Current: " + _currentLife);

        if (_currentLife <= 0)
        {
            Die();
            return;
        }
    }
}