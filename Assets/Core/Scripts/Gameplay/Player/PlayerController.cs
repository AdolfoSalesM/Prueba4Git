using UnityEngine;
using Supercyan.FreeSample;

/// <summary>
/// PlayerController:
/// Actúa como capa intermedia entre:
/// - el sistema de movimiento (SimpleSampleCharacterControl)
/// - el sistema de gameplay (interacciones, cofres, estados del juego)
/// </summary>
public class PlayerController : MonoBehaviour
{
    // -------------------------
    // 🎮 Referencias internas
    // -------------------------

    /// <summary>
    /// Sistema de movimiento base del asset (controla desplazamiento y rotación).
    /// </summary>
    [SerializeField] private SimpleSampleCharacterControl _movement;

    /// <summary>
    /// Animator del personaje para controlar animaciones globales.
    /// </summary>
    [SerializeField] private Animator _anim;

    /// <summary>
    /// Dirección basada en input anterior.
    /// </summary>
    private Vector3 _lastCardinalDirection = Vector3.zero;

    // -------------------------
    // 📡 Eventos del juego
    // -------------------------

    private void OnEnable()
    {
        GameEvents.ChestOpened += OnChestOpened;
        GameEvents.LevelStarted += OnLevelStarted;
        GameEvents.GameOver += OnGameOver;
    }

    private void OnDisable()
    {
        GameEvents.ChestOpened -= OnChestOpened;
        GameEvents.LevelStarted -= OnLevelStarted;
        GameEvents.GameOver -= OnGameOver;
    }

    // -------------------------
    // 🎮 Dirección de interacción
    // -------------------------

    /// <summary>
    /// Devuelve la dirección cardinal en la que está mirando el jugador.
    /// Se basa en la rotación real del transform del movement.
    /// </summary>
    public Vector3 FacingDirection
    {
        get
        {
            Vector3 dir = _movement.transform.forward;
            dir.y = 0;

            // Si no hay dirección clara, por seguridad devuelve abajo
            if (dir == Vector3.zero)
                return Vector3.down;

            // Convierte dirección libre a cardinal (4 direcciones)
            if (Mathf.Abs(dir.x) > Mathf.Abs(dir.z))
                return new Vector3(Mathf.Sign(dir.x), 0, 0);
            else
                return new Vector3(0, 0, Mathf.Sign(dir.z));
        }
    }

    // -------------------------
    // 🧠 Control de estado del jugador
    // -------------------------

    /// <summary>
    /// Desactiva o activa el movimiento del jugador.
    /// Se usa en transiciones, cofres o game over.
    /// </summary>
    public void SetMovementEnabled(bool enabled)
    {
        _movement.enabled = enabled;
    }

    // -------------------------
    // 🎮 Eventos de gameplay
    // -------------------------

    private void OnChestOpened()
    {
        // Detiene animación de movimiento al abrir cofre
        _anim.SetFloat("MoveSpeed", 0f);

        // Bloquea movimiento del jugador
        SetMovementEnabled(false);
    }

    private void OnLevelStarted(int level)
    {
        SetMovementEnabled(true);
    }

    private void OnGameOver()
    {
        SetMovementEnabled(false);
    }

    /// <summary>
    /// Mantiene última dirección de input del jugador.
    /// Actualmente NO se usa para lógica de interacción,
    /// ya que puede desincronizarse con el facing real del personaje.
    /// </summary>
    public Vector3 GetCardinalDirection()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dir = Vector3.zero;

        if (Mathf.Abs(h) > Mathf.Abs(v))
        {
            dir = new Vector3(Mathf.Sign(h), 0, 0);
        }
        else if (Mathf.Abs(v) > 0)
        {
            dir = new Vector3(0, 0, Mathf.Sign(v));
        }

        // Guarda última dirección válida de input
        if (dir != Vector3.zero)
        {
            _lastCardinalDirection = dir;
        }

        return _lastCardinalDirection;
    }
}