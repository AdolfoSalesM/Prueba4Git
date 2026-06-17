using UnityEngine;

/// <summary>
/// LevelRoot:
/// Representa la raíz de un nivel instanciado en escena.
/// Contiene referencias a puntos clave del nivel.
/// </summary>
public class LevelRoot : MonoBehaviour
{
    /// <summary>
    /// Punto donde el jugador aparece al cargar este nivel.
    /// Es usado por el LevelManager para spawnear o reposicionar al jugador.
    /// </summary>
    public Transform playerSpawnPoint;
}