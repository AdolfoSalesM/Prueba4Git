using UnityEngine;

/// <summary>
/// LevelDefinition:
/// ScriptableObject que define la estructura global de niveles del juego.
/// Actúa como una lista ordenada de prefabs de niveles.
/// </summary>
[CreateAssetMenu(menuName = "Game/Level Definition")]
public class LevelDefinition : ScriptableObject
{
    // -------------------------
    // 🧭 Lista de niveles
    // -------------------------

    /// <summary>
    /// Array ordenado de niveles del juego.
    /// Cada LevelRoot representa un nivel completo como prefab.
    /// </summary>
    [Header("Levels")]
    public LevelRoot[] levels;
}