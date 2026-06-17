using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

/// <summary>
/// GameManager:
/// Controla el flujo global del juego:
/// - inicio de partida
/// - progreso de niveles
/// - transición entre niveles
/// - reinicio de nivel
/// - final del juego
/// </summary>
public class GameManager : MonoBehaviour
{
    // -------------------------
    // 🔗 Referencias externas
    // -------------------------

    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private float _nextLevelDelay = 2f;

    // -------------------------
    // 📊 Estado interno del juego
    // -------------------------

    private int _currentLevel;
    private CancellationTokenSource _cts;

    /// <summary>
    /// Estados posibles del juego para evitar acciones inválidas
    /// </summary>
    private enum GameState
    {
        Playing,        // Jugando normalmente
        Transitioning,  // Entre niveles (bloquea input y acciones)
        Completed       // Juego terminado
    }

    private GameState _state;

    // -------------------------
    // 📡 Suscripción a eventos
    // -------------------------

    private void OnEnable()
    {
        GameEvents.ChestOpened += OnChestOpened;
        GameEvents.GameOver += OnGameOver;
    }

    private void OnDisable()
    {
        GameEvents.ChestOpened -= OnChestOpened;
        GameEvents.GameOver -= OnGameOver;
    }

    // -------------------------
    // 🚀 Inicio del juego
    // -------------------------

    private void Start()
    {
        StartGame();
    }

    // -------------------------
    // ⌨️ Input global
    // -------------------------

    private void Update()
    {
        // Reinicio manual del nivel
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartLevel();
            DamagePlayerOnRestart();
        }
    }

    // -------------------------
    // 🎮 Inicio de partida
    // -------------------------

    private void StartGame()
    {
        _state = GameState.Playing;

        _currentLevel = 0;

        // Carga el primer nivel
        _levelManager.LoadLevel(_currentLevel);

        // Notifica a otros sistemas
        GameEvents.RaiseLevelStarted(_currentLevel);
    }

    // -------------------------
    // 🪙 Evento: cofre abierto
    // -------------------------

    private void OnChestOpened()
    {
        // Evita doble ejecución o ejecución en transición
        if (_state != GameState.Playing)
            return;

        HandleLevelComplete().Forget();
    }

    // -------------------------
    // 🧭 Flujo de cambio de nivel
    // -------------------------

    private async UniTaskVoid HandleLevelComplete()
    {
        // Bloquea el juego durante la transición
        _state = GameState.Transitioning;

        // Cancela cualquier transición previa
        _cts?.Cancel();
        _cts = new CancellationTokenSource();

        // Espera antes de cargar el siguiente nivel (feedback visual)
        await UniTask.Delay(
            System.TimeSpan.FromSeconds(_nextLevelDelay),
            cancellationToken: _cts.Token
        );

        // Siguiente nivel
        _currentLevel++;

        // Si no hay más niveles → fin del juego
        if (!_levelManager.HasLevel(_currentLevel))
        {
            OnGameCompleted();
            return;
        }

        // Carga nuevo nivel
        _levelManager.LoadLevel(_currentLevel);

        // Notifica sistema global
        GameEvents.RaiseLevelCompleted(_currentLevel);
        GameEvents.RaiseLevelStarted(_currentLevel);

        // Vuelve a estado normal
        _state = GameState.Playing;
    }

    // -------------------------
    // 🔄 Reinicio de nivel
    // -------------------------

    private void RestartLevel()
    {
        // No permitir reinicio si el juego terminó
        if (_state == GameState.Completed)
            return;

        // No permitir reinicio durante transición
        if (_state == GameState.Transitioning)
            return;

        // Cancela cualquier delay activo
        _cts?.Cancel();

        // Recarga nivel actual
        _levelManager.LoadLevel(_currentLevel);

        // Notifica inicio de nivel
        GameEvents.RaiseLevelStarted(_currentLevel);

        _state = GameState.Playing;
    }

    private void DamagePlayerOnRestart()
    {
        if (_levelManager.CurrentPlayer == null)
            return;

        PlayerHealth player = _levelManager.CurrentPlayer.GetComponent<PlayerHealth>();

        if (player == null)
            return;

        player.ForceDamage(1);
    }

    // -------------------------
    // 🏁 Fin del juego
    // -------------------------

    private void OnGameCompleted()
    {
        _state = GameState.Completed;

        Debug.Log("GAME COMPLETED 🎉");
    }

    private void OnGameOver()
    {
        HandleGameOver().Forget();
    }

    private async UniTaskVoid HandleGameOver()
    {
        _state = GameState.Completed;

        _cts?.Cancel();
        _cts = new CancellationTokenSource();

        Debug.Log("GAME OVER 💀 RESTARTING GAME...");

        await UniTask.Delay(System.TimeSpan.FromSeconds(1f), cancellationToken: _cts.Token);

        // Recarga escena completa
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}