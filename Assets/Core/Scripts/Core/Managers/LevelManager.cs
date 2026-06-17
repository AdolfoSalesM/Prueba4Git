using UnityEngine;

/// <summary>
/// LevelManager:
/// Se encarga de la gestión de niveles en runtime.
/// - Carga niveles desde ScriptableObject (LevelDefinition)
/// - Instancia y destruye el nivel actual
/// - Spawnea y reposiciona al jugador
/// - Mantiene referencia al nivel actual
/// </summary>
public class LevelManager : MonoBehaviour
{
    // -------------------------
    // 📦 Configuración
    // -------------------------

    [SerializeField] private LevelDefinition _levelDefinition;
    [SerializeField] private GameObject _playerPrefab;

    // -------------------------
    // 🧩 Estado del nivel actual
    // -------------------------

    private LevelRoot _currentLevelInstance;
    private GameObject _playerInstance;

    /// <summary>
    /// Referencia pública al jugador actual en escena.
    /// Otros sistemas pueden usarlo sin depender del prefab.
    /// </summary>
    public GameObject CurrentPlayer { get; private set; }

    private int _currentLevelIndex;

    // -------------------------
    // 🚀 Carga de nivel
    // -------------------------

    public void LoadLevel(int levelIndex)
    {
        // Limpia el nivel anterior antes de cargar uno nuevo
        UnloadCurrentLevel();

        // Obtiene el prefab del nivel desde la definición
        LevelRoot levelPrefab = _levelDefinition.levels[levelIndex];

        // Instancia el nivel en escena
        _currentLevelInstance = Instantiate(levelPrefab);

        // Spawnea al jugador en el punto definido por el nivel
        SpawnPlayer(_currentLevelInstance.playerSpawnPoint);

        _currentLevelIndex = levelIndex;
    }

    // -------------------------
    // 👤 Spawn / reposicionamiento del jugador
    // -------------------------

    private void SpawnPlayer(Transform spawnPoint)
    {
        // Si no existe jugador aún, lo crea
        if (_playerInstance == null)
        {
            _playerInstance = Instantiate(
                _playerPrefab,
                spawnPoint.position,
                spawnPoint.rotation
            );
        }
        // Si ya existe, solo lo reposiciona (reutilización)
        else
        {
            _playerInstance.transform.position = spawnPoint.position;
            _playerInstance.transform.rotation = spawnPoint.rotation;
        }

        // Actualiza referencia pública
        CurrentPlayer = _playerInstance;
    }

    // -------------------------
    // 🧹 Limpieza de nivel
    // -------------------------

    private void UnloadCurrentLevel()
    {
        if (_currentLevelInstance != null)
        {
            Destroy(_currentLevelInstance.gameObject);
            _currentLevelInstance = null;
        }
    }

    // -------------------------
    // 📊 Utilidades
    // -------------------------

    /// <summary>
    /// Devuelve el índice del nivel actual cargado.
    /// </summary>
    public int GetCurrentLevelIndex()
    {
        return _currentLevelIndex;
    }

    /// <summary>
    /// Comprueba si existe un nivel en el índice indicado.
    /// Evita errores de out-of-range en el GameManager.
    /// </summary>
    public bool HasLevel(int index)
    {
        return index >= 0 && index < _levelDefinition.levels.Length;
    }
}