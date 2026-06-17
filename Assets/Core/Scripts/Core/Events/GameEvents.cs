using System;

/// <summary>
/// Sistema global de eventos del juego.
/// Funciona como un "bus de eventos" estático para comunicar sistemas
/// sin que dependan directamente entre sí.
/// </summary>
public static class GameEvents
{
    // -------------------------
    // 🎮 Gameplay Events
    // -------------------------

    /// <summary>
    /// Se dispara cuando el jugador abre un cofre.
    /// </summary>
    public static event Action ChestOpened;

    // -------------------------
    // 🧭 Flow de niveles
    // -------------------------

    /// <summary>
    /// Se dispara cuando empieza un nivel.
    /// Recibe el índice del nivel actual.
    /// </summary>
    public static event Action<int> LevelStarted;

    /// <summary>
    /// Se dispara cuando se completa un nivel.
    /// Recibe el índice del nivel completado.
    /// </summary>
    public static event Action<int> LevelCompleted;

    // -------------------------
    // 💀 Estado del juego
    // -------------------------

    /// <summary>
    /// Se dispara cuando el juego termina (game over o final).
    /// </summary>
    public static event Action GameOver;

    // -------------------------
    // 📡 Métodos para lanzar eventos
    // -------------------------

    /// <summary>
    /// Lanza el evento de cofre abierto.
    /// </summary>
    public static void RaiseChestOpened()
    {
        ChestOpened?.Invoke();
    }

    /// <summary>
    /// Lanza el evento de inicio de nivel.
    /// </summary>
    public static void RaiseLevelStarted(int level)
    {
        LevelStarted?.Invoke(level);
    }

    /// <summary>
    /// Lanza el evento de nivel completado.
    /// </summary>
    public static void RaiseLevelCompleted(int level)
    {
        LevelCompleted?.Invoke(level);
    }

    /// <summary>
    /// Lanza el evento de game over.
    /// </summary>
    public static void RaiseGameOver()
    {
        GameOver?.Invoke();
    }
}